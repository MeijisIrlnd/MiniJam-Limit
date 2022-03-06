using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutterController : MonoBehaviour
{
    [SerializeField] List<GameObject> openShutters;
    [SerializeField] List<GameObject> closedShutters;

    private void Awake()
    {
        SceneManager.OnTimeOfDaySwitched += TimeOfDaySwitchedCallback;
    }

    private void OnDestroy()
    {
        SceneManager.OnTimeOfDaySwitched -= TimeOfDaySwitchedCallback;
    }

    void TimeOfDaySwitchedCallback(TimeOfDay timeOfDay)
    {
        foreach(GameObject go in openShutters)
        {
            go.SetActive(timeOfDay == TimeOfDay.Day);
        }
        foreach(GameObject go in closedShutters)
        {
            go.SetActive(timeOfDay == TimeOfDay.Night);
        }
    }

}
