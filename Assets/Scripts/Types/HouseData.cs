using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class HouseData : MonoBehaviour
{
    [SerializeField] public string houseName;
    [SerializeField] public Vector3 rotation;
    [SerializeField] public string dialogFile;
    [SerializeField] public List<AudioSource> audioSourcesDay;
    [SerializeField] public List<AudioSource> audioSourcesNight;

    private JsonTypes.HouseholdJson m_householdJson;
    private void Awake()
    {
        SceneManager.OnTimeOfDaySwitched += TimeOfDayChangedCallback;
        var path = Path.Combine(Application.dataPath, "StreamingAssets", dialogFile);
        var json = File.ReadAllText(path);
        // Load the File contents to a string..
        m_householdJson = JsonConvert.DeserializeObject<JsonTypes.HouseholdJson>(json);
    }

    private void OnDestroy()
    {
        SceneManager.OnTimeOfDaySwitched -= TimeOfDayChangedCallback;
    }
    public bool HasDialogForTime(TimeOfDay timeOfDay)
    {
        return timeOfDay == TimeOfDay.Day ? m_householdJson.day.has_dialog : m_householdJson.night.has_dialog;
    }

    public List<string> GetDialogForTime(TimeOfDay timeOfDay)
    {
        if (HasDialogForTime(timeOfDay))
        {
            return timeOfDay == TimeOfDay.Day ? null : m_householdJson.night.dialog;
        }
        else
        {
            return null;
        }
    }

    void TimeOfDayChangedCallback(TimeOfDay timeOfDay)
    {
        foreach (var audioSource in audioSourcesDay) { if (audioSource.isPlaying) { audioSource.Stop(); } }
        foreach (var audioSource in audioSourcesNight) { if (audioSource.isPlaying) { audioSource.Stop(); } }
    }

}
