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
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject dialogBoxPrefab;
    [SerializeField] private GameObject interactionPromptPrefab;

    [SerializeField] AudioSource switchSource;
    [SerializeField] private List<Light> carLights;
    [SerializeField] private float streetlightChangeTime;
    [SerializeField] private List<GameObject> streetlightLights;
    [SerializeField] private GameObject nightLightState;
    [SerializeField] private GameObject dayLightState;

    [SerializeField] DialogHandler dialogHandler;
    private GameObject m_interactionPrompt;

    private Dictionary<string, int> m_houseMappings;
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

    /// <summary>
    /// Prompt the user to interact with something
    /// </summary>
    /// <param name="householdName"></param>
    public void ShowInteractionDialog(string householdName)
    {
        // Prompt for E press 
        Camera.main.gameObject.GetComponent<CameraConfigs>().SetElevationCamera(m_houseMappings[householdName]);
        Cursor.visible = true;
        //if(m_interactionPrompt != null) Destroy(m_interactionPrompt);
        //string text = $"Press E to interact with the {householdName}s's homestead....";
        //m_interactionPrompt = Instantiate(interactionPromptPrefab, canvas.transform);
        //m_interactionPrompt.GetComponentInChildren<TextMeshProUGUI>().text = text;

    }

    /// <summary>
    /// Remove any existing interaction dialog
    /// </summary>
    public void HideInteractionDialog()
    {
        Cursor.visible = false;
        Camera.main.GetComponent<CameraConfigs>().SetOverworldCamera();
        //if(m_interactionPrompt != null) { Destroy(m_interactionPrompt); }
    }

    private void SwitchTimeOfDay()
    {
        timeOfDay = timeOfDay == TimeOfDay.Day ? TimeOfDay.Night : TimeOfDay.Day;
        switchSource.Play();
        switch(timeOfDay)
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
        yield return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchTimeOfDay();
        }

    }
}
