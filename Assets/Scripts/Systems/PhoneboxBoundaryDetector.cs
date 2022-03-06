using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneboxBoundaryDetector : MonoBehaviour
{
    public static event Action OnPhoneboxTriggerEnter;
    public static event Action OnPhoneboxTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerOverworldController>() != null)
        {
            OnPhoneboxTriggerEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerOverworldController>() != null)
        {
            OnPhoneboxTriggerExit?.Invoke();
        }
    }
}
