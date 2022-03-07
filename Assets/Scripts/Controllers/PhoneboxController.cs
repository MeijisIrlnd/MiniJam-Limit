using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneboxController : MonoBehaviour
{
    [SerializeField] GameObject dayPhonebox;
    [SerializeField] GameObject nightPhonebox;
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
        dayPhonebox.SetActive(timeOfDay == TimeOfDay.Day);
        nightPhonebox.SetActive(timeOfDay == TimeOfDay.Night);
    }
}
