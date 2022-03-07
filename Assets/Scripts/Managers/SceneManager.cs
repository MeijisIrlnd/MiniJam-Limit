using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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

    [SerializeField] private HouseData introData;

    [SerializeField] AudioSource switchSource;
    [SerializeField] private List<Light> carLights;
    [SerializeField] private float streetlightChangeTime;
    [SerializeField] private List<GameObject> streetlightLights;
    [SerializeField] private GameObject nightLightState;
    [SerializeField] private GameObject dayLightState;
    [SerializeField] private PostProcessVolume postProcessing;

    [SerializeField] private float vignetteFadeTime;
    public static bool justPlayedPoliceCall = false;
    public static bool inEndingDialog = false;
    public static bool shouldPlayFinalDialog = false;
    [SerializeField] DialogHandler dialogHandler;
    private Dictionary<string, int> m_houseMappings;

    private bool m_currentlySwitchingTimeOfDay = false;
    public static event Action<TimeOfDay> OnTimeOfDaySwitched;
    public static event Action<CameraMode> OnHouseCameraChanged;
    public static event Action OnClick;
    public static event Action StartEndSequence;


    private bool m_inPhonebox = false;
    private bool m_cameraSetToPhonebox = false;

    public static bool m_playingIntro = true;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        m_houseMappings = new Dictionary<string, int>
        {
            {"Berwyn", 0 },
            {"Klebble", 1 },
            {"Bramwell", 2 },
            {"Cooper", 3 },
            {"Pelgati", 4 }
        };

        PhoneboxBoundaryDetector.OnPhoneboxTriggerEnter += PhoneboxEntered;
        PhoneboxBoundaryDetector.OnPhoneboxTriggerExit += PhoneboxExited;
        CameraConfigs.TriggerEnding += EndingResponder;
    }

    private void OnDestroy()
    {
        PhoneboxBoundaryDetector.OnPhoneboxTriggerEnter -= PhoneboxEntered;
        PhoneboxBoundaryDetector.OnPhoneboxTriggerExit -= PhoneboxExited;
        CameraConfigs.TriggerEnding -= EndingResponder;
    }

    private void Start()
    {
        // Fade vignette in..    
        StartCoroutine(FadeVignetteIn());
        dialogHandler.ShowIntroDialog(introData.GetDialogForTime(TimeOfDay.Night));
       
    }

    private void PhoneboxEntered()
    {
        m_inPhonebox = true;
        Debug.Log("Phonebox entered");
    }

    private void PhoneboxExited()
    {
        m_inPhonebox = false;
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

    private void EndingResponder()
    {
        StartCoroutine(FadeVignetteOut());
    }

    private IEnumerator FadeVignetteOut()
    {
        Vignette vignette;
        AutoExposure autoExposure;
        postProcessing.profile.TryGetSettings(out vignette);
        postProcessing.profile.TryGetSettings(out autoExposure);
        float startVignette = vignette.intensity;
        float startMinEv = autoExposure.minLuminance;
        float endVignette = 1;
        float endMinEv = 9;
        float timer = 0;
        while(timer < vignetteFadeTime)
        {
            float current = startVignette + (endVignette - startVignette) * (timer / vignetteFadeTime);
            vignette.intensity.value = current;
            autoExposure.minLuminance.value = startMinEv + (endMinEv - startMinEv) * (timer / vignetteFadeTime);
            
            //a + (b - a) * x;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }
        yield return null;
    }

    private IEnumerator FadeVignetteIn()
    {
        Vignette vignette;
        AutoExposure autoExposure;
        postProcessing.profile.TryGetSettings(out vignette);
        postProcessing.profile.TryGetSettings(out autoExposure);
        float startVignette = 1;
        float startMinEv = 68;
        float endVignette = 0.542f;
        float endMinEv = -44;
        float timer = 0;
        while (timer < vignetteFadeTime)
        {
            float current = startVignette + (endVignette - startVignette) * (timer / vignetteFadeTime);
            vignette.intensity.value = current;
            autoExposure.minLuminance.value = startMinEv + (endMinEv - startMinEv) * (timer / vignetteFadeTime);

            //a + (b - a) * x;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }
        yield return null;
    }

    private void Update()
    {
        if (!SceneManager.shouldPlayFinalDialog && !SceneManager.m_playingIntro)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !m_currentlySwitchingTimeOfDay && (CameraConfigs.currentMode == CameraMode.Exterior || CameraConfigs.currentMode == CameraMode.Overworld))
            {
                SwitchTimeOfDay();
            }
            if ((focussedHouse != null || m_inPhonebox) && CameraConfigs.currentMode != CameraMode.Interior)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (CameraConfigs.currentMode == CameraMode.Exterior)
                    {
                        ShowOverworld();
                        OnHouseCameraChanged?.Invoke(CameraConfigs.currentMode);
                    }
                    else if (CameraConfigs.currentMode == CameraMode.Overworld && !m_inPhonebox)
                    {
                        ShowHouseExterior(focussedHouse.houseName);
                        OnHouseCameraChanged?.Invoke(CameraConfigs.currentMode);
                    }
                    else if (m_inPhonebox && !m_cameraSetToPhonebox)
                    {
                        // Transform camera to phonebox
                        Camera.main.GetComponent<CameraConfigs>().SetPhoneboxCamera();
                        m_cameraSetToPhonebox = true;
                    }
                    else if (m_inPhonebox && m_cameraSetToPhonebox)
                    {
                        Cursor.visible = false;
                        Camera.main.GetComponent<CameraConfigs>().SetOverworldCamera();
                        m_cameraSetToPhonebox = false;
                    }
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                OnClick?.Invoke();
            }
        }


    }
}
