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
        var path = Path.Combine(Application.dataPath, "StreamingAssets", dialogFile);
        var json = File.ReadAllText(path);
        // Load the File contents to a string..
        m_householdJson = JsonConvert.DeserializeObject<JsonTypes.HouseholdJson>(json);
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

}
