using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCollisionDetector : MonoBehaviour
{
    [SerializeField] public OverlayManager overlayManager;
    [SerializeField] public GameObject m_playerObject;
    [SerializeField] public string m_houseName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == m_playerObject)
        {
            Debug.Log("Collision!");
            // Tell the player it's in the boundary.. 
            overlayManager.ShowInteractionDialog(m_houseName);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
