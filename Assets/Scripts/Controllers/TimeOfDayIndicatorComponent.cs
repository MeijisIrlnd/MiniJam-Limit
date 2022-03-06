using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeOfDayIndicatorComponent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private Image dayImage;
    [SerializeField] private Image nightImage;

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
        dayImage.gameObject.SetActive(timeOfDay == TimeOfDay.Day);
        nightImage.gameObject.SetActive(timeOfDay == TimeOfDay.Night);
        string otherTimeOfDay = timeOfDay == TimeOfDay.Day ? "night" : "day";
        textComponent.text = $"press space to change time to {otherTimeOfDay}";
    }
}
