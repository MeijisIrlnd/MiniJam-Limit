using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooperHouseController : MonoBehaviour
{
    [SerializeField] private GameObject dayModel;
    [SerializeField] private GameObject nightModel;

    private void Awake()
    {
        SceneManager.OnTimeOfDaySwitched += OnTimeOfDayChanged;
    }

    private void OnDestroy()
    {
        SceneManager.OnTimeOfDaySwitched -= OnTimeOfDayChanged;
    }

    private void OnTimeOfDayChanged(TimeOfDay timeOfDay)
    {
        dayModel.SetActive(timeOfDay == TimeOfDay.Day);
        nightModel.SetActive(timeOfDay == TimeOfDay.Night);
    }
}
