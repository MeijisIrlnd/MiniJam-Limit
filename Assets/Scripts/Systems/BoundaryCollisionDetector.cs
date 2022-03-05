using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCollisionDetector : MonoBehaviour
{
    [SerializeField] public SceneManager sceneManager;
    [SerializeField] public GameObject m_playerObject;
    [SerializeField] public HouseData m_houseData;

    private bool m_awaitingSelect = false;
    private bool m_showingExterior = false;
    /// <summary>
    /// Tell the overlay manager to show the interaction prompt with this household name
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == m_playerObject)
        {
            Debug.Log($"Press E to interact with {m_houseData.houseName}'s house..");
            m_awaitingSelect = true;
        }
    }

    /// <summary>
    /// Tell the overlay manager to hide the interaction prompt with this household name
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        m_awaitingSelect = false;
        if(other.gameObject == m_playerObject)
        {
            //sceneManager.HideInteractionDialog();
        }
    }

    private void Update()
    {
        if(m_awaitingSelect && CameraConfigs.currentMode != CameraMode.Interior)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if (!m_showingExterior)
                {
                    sceneManager.ShowInteractionDialog(m_houseData.houseName);
                    m_showingExterior = true;
                }
                else
                {
                    sceneManager.HideInteractionDialog();
                    m_showingExterior = false;
                }
            }
        }
    }

}
