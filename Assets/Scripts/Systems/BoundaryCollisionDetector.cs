using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCollisionDetector : MonoBehaviour
{
    public static event Action<HouseData> OnHouseTriggerEnter;
    public static event Action OnHouseTriggerExit;
    [SerializeField] public SceneManager sceneManager;
    [SerializeField] public GameObject m_playerObject;
    [SerializeField] public HouseData m_houseData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == m_playerObject)
        {
            SceneManager.instance.focussedHouse = m_houseData;
            Debug.Log($"Press E to interact with {m_houseData.houseName}'s house..");
            OnHouseTriggerEnter?.Invoke(m_houseData);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SceneManager.instance.focussedHouse = null;
        OnHouseTriggerExit?.Invoke();
    }
}
