using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    private void Awake()
    {
        SceneManager.OnTimeOfDaySwitched += TimeOfDaySwitched;
    }

    private void OnDestroy()
    {
        SceneManager.OnTimeOfDaySwitched -= TimeOfDaySwitched;
    }

    private void TimeOfDaySwitched(TimeOfDay timeOfDay)
    {
        for(var i = 0; i < gameObject.transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(timeOfDay == TimeOfDay.Night);
        }
           
    }


}
