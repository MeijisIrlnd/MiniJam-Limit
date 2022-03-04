using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCollisionDetector : MonoBehaviour
{
    [SerializeField] public GameObject m_playerObject;
    [SerializeField] public string m_houseName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == m_playerObject)
        {
            // Tell the player it's in the boundary.. 

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
