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

    private void Awake()
    {
       DontDestroyOnLoad(gameObject);
    }

    public void ShowInteractionDialog(string householdName)
    {
        string text = $"Press E to interact with the {householdName}s's homestead....";
        GameObject prompt = Instantiate(interactionPromptPrefab, canvas.transform);
        prompt.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
