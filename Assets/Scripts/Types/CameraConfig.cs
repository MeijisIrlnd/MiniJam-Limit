using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConfig
{
    public Vector3 rotation;
    public Vector3 position;
    public bool orthographic;

    public CameraConfig(Vector3 rotation, Vector3 position, bool orthographic)
    {
        this.rotation = rotation;
        this.position = position;
        this.orthographic = orthographic;
    }
    public void Apply()
    {
        Camera.main.transform.rotation = Quaternion.Euler(rotation);
        Camera.main.transform.position = position;
        Camera.main.orthographic = orthographic;
    }
}
