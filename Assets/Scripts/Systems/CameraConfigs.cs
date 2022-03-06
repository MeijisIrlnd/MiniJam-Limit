using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
{
    Overworld, 
    Exterior, 
    Interior,
    Phonebox, 
    YellowPages
};

public class CameraConfigs : MonoBehaviour
{
    [SerializeField] List<GameObject> houses;
    [SerializeField] DialogHandler dialogHandler;
    public static event Action HideUI;
    public static event Action ShowUI;

    private CameraConfig m_overworldConfig;
    private List<CameraConfig> m_houseExteriorConfigs;
    private List<CameraConfig> m_houseInteriorConfigs;
    private readonly Vector3 m_exteriorCameraDelta = new Vector3(-7.13f, 0.1f, -0.06f);
    private readonly Vector3 m_interiorCameraDelta = new Vector3(-2.34f, 1.41f, -2.15f);
    private CameraConfig m_phoneboxCameraConfig = new CameraConfig(new Vector3(0, -90, 0), new Vector3(-5.257f, 1.06f, 2.23f), false);
    private CameraConfig m_yellowPagesCameraConfig = new CameraConfig(new Vector3(0, -90, 0), new Vector3(-4.49f, 17.68f, 2.2f), false);
    public static CameraMode currentMode = CameraMode.Overworld;
    private int m_currentHouseIndex = 0;

    private void Awake()
    {
        SceneManager.OnTimeOfDaySwitched += TimeOfDayChangedCallback;
        SceneManager.OnClick += ClickCallback;

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

    private void OnDestroy()
    {
        SceneManager.OnTimeOfDaySwitched -= TimeOfDayChangedCallback;
        SceneManager.OnClick -= ClickCallback;
    }

    void TimeOfDayChangedCallback(TimeOfDay timeOfDay)
    {

    }

    public void SetOverworldCamera() { currentMode = CameraMode.Overworld; m_overworldConfig.Apply(); }

    public void SetElevationCamera(int houseNumber) {
        ShowUI?.Invoke();
        currentMode = CameraMode.Exterior;
        m_currentHouseIndex = houseNumber;
        m_houseExteriorConfigs[houseNumber].Apply(); 
    }

    public void SetInteriorCamera() {
        HideUI?.Invoke();
        currentMode = CameraMode.Interior;
        m_houseInteriorConfigs[m_currentHouseIndex].Apply();
        HouseData currentHouseData = houses[m_currentHouseIndex].GetComponent<HouseData>();
        if (currentHouseData.HasDialogForTime(SceneManager.timeOfDay))
        {
            SceneManager.instance.ShowDialog(currentHouseData.GetDialogForTime(SceneManager.timeOfDay));
        }
        if(SceneManager.timeOfDay == TimeOfDay.Day)
        {
            foreach (var audioSource in currentHouseData.audioSourcesDay)
            {
                audioSource.Play();
            }
        }
        else
        {
            foreach (var audioSource in currentHouseData.audioSourcesNight)
            {
                audioSource.Play();
            }
        }
    }

    public void SetPhoneboxCamera()
    {
        Cursor.visible = true;
        ShowUI?.Invoke();
        m_phoneboxCameraConfig.Apply();
        currentMode = CameraMode.Phonebox;
    }

    public void SetYellowPagesCamera()
    {
        HideUI?.Invoke();
        m_yellowPagesCameraConfig.Apply();
        currentMode = CameraMode.YellowPages;

    }

    void ClickCallback()
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
        else if(currentMode == CameraMode.Phonebox)
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
                    //SetInteriorCamera();
                    Debug.Log("Phonebook found!");
                    SetYellowPagesCamera();
                }
            }
        }
        else if(currentMode == CameraMode.YellowPages)
        {
            //RaycastHit[] hits;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool didHit = Physics.Raycast(ray, out hit);
            // if hits.Length == 0, return...
            if (!didHit)
            {
                SetPhoneboxCamera();
                dialogHandler.Cancel();
            }
            else
            {
                var entry = hit.transform.GetComponentInChildren<YellowPagesEntry>();
                if (entry != null)
                {
                    Debug.Log("Phonebook found!");
                    dialogHandler.Show(entry.linkedHouseData.GetPhoneDialog());
                }
            }
        }

        // Raycast for phonebook, and translate camera to Yellow Pages...
        else if (currentMode == CameraMode.Interior)
        {
            if (SceneManager.instance.GetIsDialogShowing()) { SceneManager.instance.CancelDialog(); }
            SetElevationCamera(m_currentHouseIndex);
            SceneManager.instance.focussedHouse.StopAudio();
        }
    }
    
}
