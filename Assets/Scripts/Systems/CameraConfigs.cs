using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraConfigs : MonoBehaviour
{
    [SerializeField] List<GameObject> m_houses;
    private CameraConfig m_overworldConfig;
    private List<CameraConfig> m_houseConfigs;

    private void Awake()
    {
        m_overworldConfig = new CameraConfig(new Vector3(38.3f, 0, 0), new Vector3(0, 7.8f, -10.75f), false);
        m_houseConfigs = new List<CameraConfig>
        {
            new CameraConfig(new Vector3(0, -90, 0), new Vector3(0, 2.17f, -0.94f), false)
        };
        SetOverworldCamera();
    }

    public void SetOverworldCamera() { m_overworldConfig.Apply(); }

    public void SetElevationCamera(int houseNumber) { m_houseConfigs[houseNumber].Apply(); }

    // Update is called once per frame
    void Update()
    {
        
    }
}
