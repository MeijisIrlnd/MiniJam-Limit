using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverworldController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private Vector3 m_position;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        gameObject.transform.position = new Vector3(transform.position.x + (h * movementSpeed), transform.position.y, transform.position.z + (v * movementSpeed));
    }

}
