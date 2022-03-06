using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TimeOfDay
{
    Day, 
    Night
};

public class SceneManager : MonoBehaviour
{
    private static SceneManager m_instance;
    public static SceneManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<SceneManager>();
            }
            return m_instance;
        }
    }
    public static TimeOfDay timeOfDay = TimeOfDay.Day;
    public HouseData focussedHouse = null;

    [SerializeField] AudioSource switchSource;
    [SerializeField] private List<Light> carLights;
    [SerializeField] private float streetlightChangeTime;
    [SerializeField] private List<GameObject> streetlightLights;
    [SerializeField] private GameObject nightLightState;
    [SerializeField] private GameObject dayLightState;

    [SerializeField] DialogHandler dialogHandler;
    private Dictionary<string, int> m_houseMappings;

    private bool m_currentlySwitchingTimeOfDay = false;
    public static event Action<TimeOfDay> OnTimeOfDaySwitched;
    public static event Action<CameraMode> OnHouseCameraChanged;
    public static event Action OnClick;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        m_houseMappings = new Dictionary<string, int>
        {
            {"Berwyn", 0 },
            {"Klebble", 1 },
            {"Bramwell", 2 },
            {"Cooper", 3 }
        };
    }

    public void ShowDialog(List<string> dialog)
    {
        dialogHandler.Show(dialog);
    }

    public bool GetIsDialogShowing() { return dialogHandler.IsShowing(); }

    public void CancelDialog() { dialogHandler.Cancel(); }

    /// <summary>
    /// Prompt the user to interact with something
    /// </summary>
    /// <param name="householdName"></param>
    public void ShowHouseExterior(string householdName)
    {
        Camera.main.gameObject.GetComponent<CameraConfigs>().SetElevationCamera(m_houseMappings[householdName]);
        Cursor.visible = true;
    }

    /// <summary>
    /// Remove any existing interaction dialog
    /// </summary>
    public void ShowOverworld()
    {
        Cursor.visible = false;
        Camera.main.GetComponent<CameraConfigs>().SetOverworldCamera();
    }

    private void SwitchTimeOfDay()
    {
        timeOfDay = timeOfDay == TimeOfDay.Day ? TimeOfDay.Night : TimeOfDay.Day;
        switchSource.Play();
        m_currentlySwitchingTimeOfDay = true;
        OnTimeOfDaySwitched?.Invoke(timeOfDay);
        switch (timeOfDay)
        {
            case TimeOfDay.Day:
                {
                    nightLightState.SetActive(false);
                    dayLightState.SetActive(true);
                    StartCoroutine(SwitchStreetLights());
                    break;
                }
            case TimeOfDay.Night:
                {
                    nightLightState.SetActive(true);
                    dayLightState.SetActive(false);
                    StartCoroutine(SwitchStreetLights());
                    break;
                }
        }
    }

    private IEnumerator SwitchStreetLights()
    {
        bool on = timeOfDay == TimeOfDay.Night;
        float timePerLight = streetlightChangeTime / (streetlightLights.Count / 2.0f);
        for (var i = 0; i < streetlightLights.Count; i += 2)
        {
            streetlightLights[i].SetActive(on);
            streetlightLights[i + 1].SetActive(on);
            yield return new WaitForSeconds(timePerLight);
        }
        foreach(var l in carLights) {  l.gameObject.SetActive(on); }
        m_currentlySwitchingTimeOfDay = false;
        yield return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !m_currentlySwitchingTimeOfDay)
        {
            SwitchTimeOfDay();
        }
        if(focussedHouse != null && CameraConfigs.currentMode != CameraMode.Interior)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(CameraConfigs.currentMode == CameraMode.Exterior)
                {
                    ShowOverworld();
                    OnHouseCameraChanged?.Invoke(CameraConfigs.currentMode);
                }
                else
                {
                    ShowHouseExterior(focussedHouse.houseName);
                    OnHouseCameraChanged?.Invoke(CameraConfigs.currentMode);
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            OnClick?.Invoke();
        }


    }
}
