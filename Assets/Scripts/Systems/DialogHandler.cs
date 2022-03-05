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


    public void Show(List<string> dialog)
    {
        // Lerp the textbox in, and then start the dialog coroutine..
        StartCoroutine(ShowDialogAsync(dialog));
    }

    public void Clear()
    {
        textBox.text = string.Empty;
    }

    private IEnumerator ShowDialogAsync(List<string> dialog)
    {
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
                yield return new WaitForSeconds(timePerCharacter);
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1);
        Clear();
        timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            background.color = Color.Lerp(endColour, startColour, t);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
