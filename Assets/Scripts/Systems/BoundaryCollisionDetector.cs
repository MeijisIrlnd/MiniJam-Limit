using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCollisionDetector : MonoBehaviour
{
    [SerializeField] public OverlayManager overlayManager;
    [SerializeField] public GameObject m_playerObject;
    [SerializeField] public HouseData m_houseData;

    /// <summary>
    /// Tell the overlay manager to show the interaction prompt with this household name
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == m_playerObject)
        {
            overlayManager.ShowInteractionDialog(m_houseData.houseName);
        }
    }

    /// <summary>
    /// Tell the overlay manager to hide the interaction prompt with this household name
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == m_playerObject)
        {
            overlayManager.HideInteractionDialog();
        }
    }
    
}
