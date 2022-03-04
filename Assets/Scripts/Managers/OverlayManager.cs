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

    private void Awake()
    {
       DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Prompt the user to interact with something
    /// </summary>
    /// <param name="householdName"></param>
    public void ShowInteractionDialog(string householdName)
    {
        if(m_interactionPrompt != null) Destroy(m_interactionPrompt);
        string text = $"Press E to interact with the {householdName}s's homestead....";
        m_interactionPrompt = Instantiate(interactionPromptPrefab, canvas.transform);
        m_interactionPrompt.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    /// <summary>
    /// Remove any existing interaction dialog
    /// </summary>
    public void HideInteractionDialog()
    {
        if(m_interactionPrompt != null) { Destroy(m_interactionPrompt); }
    }
}
