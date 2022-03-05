using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
{
    Overworld, 
    Exterior, 
    Interior
};

public class CameraConfigs : MonoBehaviour
{
    [SerializeField] List<GameObject> houses;
    private CameraConfig m_overworldConfig;
    private List<CameraConfig> m_houseExteriorConfigs;
    private List<CameraConfig> m_houseInteriorConfigs;
    private readonly Vector3 m_exteriorCameraDelta = new Vector3(-7.13f, 0.1f, -0.06f);
    private readonly Vector3 m_interiorCameraDelta = new Vector3(-2.34f, 1.41f, -2.15f);
    public static CameraMode currentMode = CameraMode.Overworld;
    private int m_currentHouseIndex = 0;
    private void Awake()
    {
        m_overworldConfig = new CameraConfig(new Vector3(38.3f, 0, 0), new Vector3(0, 7.8f, -24.15f), false);
        m_houseExteriorConfigs = new List<CameraConfig>();
        m_houseInteriorConfigs = new List<CameraConfig>();
        foreach(GameObject house in houses)
        {
            HouseData houseData = house.GetComponent<HouseData>();
            var pos = house.transform.position;
            //Vector3 cameraPos = new Vector3(pos.x - cameraDelta.x, pos.y - cameraDelta.y, pos.z - cameraDelta.z);
            var sign = Mathf.Sign(houseData.rotation.y);
            // If sign is positive, add... [ kill me ] 
            Vector3 extCameraPos, intCameraPos;
            if (sign == 1)
            {
                // WHAT THE FUCK 
                extCameraPos = new Vector3(pos.x + m_exteriorCameraDelta.x, pos.y + m_exteriorCameraDelta.y, pos.z + m_exteriorCameraDelta.z);
                intCameraPos = new Vector3(pos.x + m_interiorCameraDelta.x, pos.y - m_interiorCameraDelta.y, pos.z + m_interiorCameraDelta.z);
            }
            else
            {
                extCameraPos = new Vector3(pos.x - m_exteriorCameraDelta.x, pos.y - m_exteriorCameraDelta.y, pos.z - m_exteriorCameraDelta.z);
                intCameraPos = new Vector3(pos.x - m_interiorCameraDelta.x, pos.y - m_interiorCameraDelta.y, pos.z - m_interiorCameraDelta.z);
            }
            m_houseExteriorConfigs.Add(new CameraConfig(houseData.rotation, extCameraPos, false));
            m_houseInteriorConfigs.Add(new CameraConfig(houseData.rotation, intCameraPos, false));
        }
        SetOverworldCamera();
    }

    public void SetOverworldCamera() { currentMode = CameraMode.Overworld; m_overworldConfig.Apply(); }

    public void SetElevationCamera(int houseNumber) { 
        currentMode = CameraMode.Exterior;
        m_currentHouseIndex = houseNumber;
        m_houseExteriorConfigs[houseNumber].Apply(); 
    }

    public void SetInteriorCamera() { 
        currentMode = CameraMode.Interior;
        m_houseInteriorConfigs[m_currentHouseIndex].Apply();
        HouseData currentHouseData = houses[m_currentHouseIndex].GetComponent<HouseData>();
        if (currentHouseData.HasDialogForTime(SceneManager.timeOfDay))
        {
            SceneManager.instance.ShowDialog(currentHouseData.GetDialogForTime(SceneManager.timeOfDay));
        }
        foreach(var audioSource in currentHouseData.audioSources)
        {
            audioSource.Play();
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentMode == CameraMode.Exterior)
            {
                RaycastHit[] hits;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                hits = Physics.RaycastAll(ray, 1000.0f);
                Debug.Log($"Num Hits: {hits.Length}");
                for (int i = 0; i < hits.Length; i++)
                {
                    var clickable = hits[i].transform.GetComponentInChildren<Clickable>();
                    if (clickable != null)
                    {
                        SetInteriorCamera();
                        Debug.Log("Component found!");
                    }
                }
            }
            else if(currentMode == CameraMode.Interior)
            {
                HouseData currentHouseData = houses[m_currentHouseIndex].GetComponent<HouseData>();
                foreach(var audioSource in currentHouseData.audioSources)
                {
                    audioSource.Stop();
                }

                SetElevationCamera(m_currentHouseIndex);
            }
        }
    }
}
