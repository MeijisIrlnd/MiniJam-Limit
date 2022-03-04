using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraView
{
    Overworld, 
    Elevation
}

public class CameraConfigs : MonoBehaviour
{
    private Dictionary<CameraView, CameraConfig> m_configs;

    private void Awake()
    {
          
        m_configs = new Dictionary<CameraView, CameraConfig> {
            { CameraView.Overworld, new CameraConfig(new Vector3(38.3f, 0, 0), new Vector3(0, 7.8f, -10.75f), false)},
        };
        //SetView(CameraView.Overworld);
    }

    void SetView(CameraView newView)
    {
        if(m_configs.ContainsKey(newView))
        {
            m_configs[newView].Apply();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
