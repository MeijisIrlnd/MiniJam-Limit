using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverworldController : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Rigidbody rb;

    private Vector3 m_position;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CameraConfigs.currentMode == CameraMode.Overworld)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            gameObject.transform.Rotate(new Vector3(0, h, 0));
            var lookVector = gameObject.transform.forward;
            transform.position += v * lookVector * Time.deltaTime * movementSpeed;
        }
    }

}
