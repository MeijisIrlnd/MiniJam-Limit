using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogHandler : MonoBehaviour
{
    [SerializeField] float fadeDuration;
    [SerializeField] float timePerCharacter;
    [SerializeField] Image background;
    [SerializeField] TextMeshProUGUI textBox;
    private bool m_shouldCancel = false;
    private bool m_isShowing = false;


    public bool IsShowing() { return m_isShowing; }
    public void Show(List<string> dialog)
    {
        // Lerp the textbox in, and then start the dialog coroutine..
        StartCoroutine(ShowDialogAsync(dialog));
    }

    public void Cancel() { m_shouldCancel = true; }
    public void Clear()
    {
        textBox.text = string.Empty;
    }

    private IEnumerator ShowDialogAsync(List<string> dialog)
    {
        m_isShowing = true;
        var startColour = background.color;
        var endColour = new Color(background.color.r, background.color.g, background.color.b, 0.25f);
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            background.color = Color.Lerp(startColour, endColour, t);
            yield return new WaitForEndOfFrame();
        }

        foreach (string line in dialog)
        {
            Clear();
            foreach(char c in line)
            {
                textBox.text = $"{textBox.text}{c}";
                if (m_shouldCancel) break;
                yield return new WaitForSeconds(timePerCharacter);
            }
            if (m_shouldCancel) break;
            yield return new WaitForSeconds(0.5f);
        }
        m_shouldCancel = false;
        Clear();
        timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            background.color = Color.Lerp(endColour, startColour, t);
            yield return new WaitForEndOfFrame();
        }
        m_isShowing = false;
        yield return null;
    }
}
