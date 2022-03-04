using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    private static OverlayManager m_instance;
    public static OverlayManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = GameObject.FindObjectOfType<OverlayManager>();
            }
            return m_instance;
        }
    }
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject dialogBoxPrefab;
    [SerializeField] private GameObject interactionPromptPrefab;
    private GameObject m_interactionPrompt;

    private Dictionary<string, int> m_houseMappings;
    private void Awake()
    {
       DontDestroyOnLoad(gameObject);
        m_houseMappings = new Dictionary<string, int>
        {
            {"Johnson", 0 },
            {"Bovril", 1 }
        };
    }

    /// <summary>
    /// Prompt the user to interact with something
    /// </summary>
    /// <param name="householdName"></param>
    public void ShowInteractionDialog(string householdName)
    {
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
}
