using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraConfigs : MonoBehaviour
{
    [SerializeField] List<GameObject> houses;
    private CameraConfig m_overworldConfig;
    private List<CameraConfig> m_houseExteriorConfigs;
    private List<CameraConfig> m_houseInteriorConfigs;
    private readonly Vector3 m_exteriorCameraDelta = new Vector3(-7.13f, 0.1f, -0.06f);
    private readonly Vector3 m_interiorCameraDelta = new Vector3(-2.34f, 1.41f, -2.15f);
    private void Awake()
    {
        m_overworldConfig = new CameraConfig(new Vector3(38.3f, 0, 0), new Vector3(0, 7.8f, -10.75f), false);
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
                extCameraPos = new Vector3(pos.x + m_exteriorCameraDelta.x, pos.y + m_exteriorCameraDelta.y, pos.z + m_exteriorCameraDelta.z);
                intCameraPos = new Vector3(pos.x + m_interiorCameraDelta.x, pos.y + m_interiorCameraDelta.y, pos.z + m_interiorCameraDelta.z);
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

    public void SetOverworldCamera() { m_overworldConfig.Apply(); }

    public void SetElevationCamera(int houseNumber) { m_houseExteriorConfigs[houseNumber].Apply(); }

    // Update is called once per frame
    void Update()
    {
        
    }
}
