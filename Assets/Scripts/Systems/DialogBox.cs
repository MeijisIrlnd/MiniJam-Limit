using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;

public class DialogBox : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI m_textBox;
    [SerializeField] public float m_waitInterval;
    private DialogJson m_dialog;

    // Start is called before the first frame update
    void Start()
    {
        LoadJson("Dialog/SampleHouse.json");
    }

    void LoadJson(string filenameToLoad)
    {
        var path = Path.Combine(Application.dataPath, "StreamingAssets", filenameToLoad);
        var json = File.ReadAllText(path);
        m_dialog = JsonConvert.DeserializeObject<DialogJson>(json);
        StartCoroutine(Show(0));
    }

    public IEnumerator Show(int conversationIndex)
    {
        foreach (var page in m_dialog.conversations[conversationIndex])
        {
            clear();
            foreach (char c in page)
            {
                // Press A to continue..
                string currentText = m_textBox.text;
                currentText = string.Concat(currentText, c);
                m_textBox.text = currentText;
                yield return new WaitForSeconds(m_waitInterval);
            }
            yield return new WaitForSeconds(m_waitInterval * 4);
        }
    }

    public void clear()
    {
        m_textBox.text = "";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
