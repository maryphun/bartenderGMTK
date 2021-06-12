using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] Image mainAlpha;
    [SerializeField] TMP_Text textobject;
    [SerializeField, TextArea(15, 20)] string text;

    private void Start()
    {
        // screen should be black(?)
        //mainAlpha.DOFade(0f, 0f);

        // Add text
        StartCoroutine(TypeWriter(text, 0.075f));
    }

    private IEnumerator TypeWriter(string newText, float interval)
    {
        int wordCount = newText.Length - 1;
        int currentCount = 0;
        string textTmp = string.Empty;

        while (currentCount <= wordCount)
        {
            textTmp = textTmp + newText[currentCount];
            // initiate wait time
            textobject.SetText(textTmp);

            // update count at the last
            currentCount++;
            yield return new WaitForSeconds(interval);
        }
    }
}
