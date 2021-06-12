using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CustomTextAlignment
{
    topLeft,
    topCenter,
    topRight,
    middleLeft,
    center,
    middleRight,
    bottomLeft,
    bottomCenter,
    bottomRight
}

public class Window : MonoBehaviour
{
    public enum WindiowState
    {
        opening,
        opened,
        closing,
        closed
    }

    [SerializeField] private Image baseImage;
    private WindiowState windowState;
    private RectTransform rect;
    private Vector2 windowSize;
    private TMP_Text dialogText;
    private bool isInitialized;
    private bool isTypeWrting;
    private bool isSkipTypeWriter;


    private List<TMP_Text> extraTextList = new List<TMP_Text>();
    private List<Image> extraImageList = new List<Image>();

    public void Initialize()
    {
        if (isInitialized) return;

        rect = GetComponent<RectTransform>();
        windowSize = rect.sizeDelta;
        dialogText = GetComponentInChildren<TMP_Text>(true);
        extraTextList.Clear();
        windowState = WindiowState.closed;

        isInitialized = true;
    }

    public void ResizeX(float sizeX, float time)
    {
        StartCoroutine(Resize(new Vector2(sizeX, rect.sizeDelta.y), time));
    }

    public void ResizeX(float sizeX)
    {
        rect.sizeDelta = new Vector2(sizeX, rect.sizeDelta.y);
        if (GetIsOpen())
        {
            DoneOpen();
        }
        else
        {
            DoneClose();
        }
    }

    private IEnumerator Resize(Vector2 target, float time)
    {
        Vector2 origin = GetComponent<RectTransform>().sizeDelta;
        float lerp = 0;

        while (lerp < time)
        {
            lerp = Mathf.Clamp(lerp + Time.deltaTime, 0.0f, time);
            rect.sizeDelta = Vector2.Lerp(origin, target, ParametricBlend(lerp/time));
            yield return new WaitForEndOfFrame();
        }

        rect.sizeDelta = target;

        if (GetIsOpen())
        {
            DoneOpen();
        }
        else
        {
            DoneClose();
        }
    }

    public void SetSizeAfterDelay(Vector2 size, float time)
    {
        StartCoroutine(SetSizeAfterDelayLoop(size, time));
    }

    private IEnumerator SetSizeAfterDelayLoop(Vector2 size, float time)
    {
        yield return new WaitForSeconds(time);

        rect.sizeDelta = size;
    }

    public void SetActiveAfterDelay(bool active, float time)
    {
        StartCoroutine(SetActiveAfterDelayLoop(active, time));
    }

    private IEnumerator SetActiveAfterDelayLoop(bool active, float time)
    {
        yield return new WaitForSeconds(time);

        gameObject.SetActive(active);
    }

    // formulae for ease in and ease out
    private float ParametricBlend(float t)
    {
        float sqt = t * t;
        return sqt / (2.0f * (sqt - t) + 1.0f);
    }

    public bool GetIsOpen()
    {
        return windowState == WindiowState.opened || windowState == WindiowState.opening;
    }

    private void DoneOpen()
    {
        windowState = WindiowState.opened;

        // show extra texts
        EnableAllEntraText(true);
        EnableAllEntraImage(true);
    }

    private void DoneClose()
    {
        windowState = WindiowState.closed;
    }

    public void SetWindowState(WindiowState state)
    {
        windowState = state;

        dialogText.gameObject.SetActive(GetIsOpen());

        if (!GetIsOpen())
        {
            // hide extra texts
            EnableAllEntraText(false);
            EnableAllEntraImage(false);
        }
    }

    public Vector2 GetWindowSize()
    {
        return windowSize;
    }

    // Text
    public void SetTextSize(float newSize)
    {
        dialogText.fontSize = newSize;
    }
    public void SetTextColor(Color color)
    {
        dialogText.color = color;
    }

    public void SetText(string newText)
    {
        dialogText.SetText(newText);
    }

    public void SetText(string newText, float interval)
    {
        if (isTypeWrting) return;

        StartCoroutine(SetTextLoop(newText, interval));
    }

    private IEnumerator SetTextLoop(string newText, float interval)
    {
        int wordCount = newText.Length-1;
        int currentCount = 0;
        string patternDetect = string.Empty;
        string text = string.Empty;

        isTypeWrting = true;

        while (currentCount <= wordCount)
        {
            // initiate wait time
            float waitTime = isSkipTypeWriter ?  0.0f : interval;

            // update text
            text = text + newText[currentCount];
            SetText(text);

            // check pattern
            patternDetect = patternDetect + newText[currentCount];

            if (CheckPatterns(patternDetect, waitTime, out float newWaitTime))
            {
                patternDetect = string.Empty;
                waitTime = newWaitTime;
            }

            // update count at the last
            currentCount++;
            yield return new WaitForSeconds(waitTime);
        }

        isTypeWrting = false;
        isSkipTypeWriter = false;
    }

    public void SkipText()
    {
        if (isTypeWrting)
        {
            isSkipTypeWriter = true;
        }
    }

    public bool GetIsTypeWriting()
    {
        if (isSkipTypeWriter) return false;

        return isTypeWrting;
    }

    private bool CheckPatterns(string pattern, float originalWaitTime, out float newWaitTime)
    {
        newWaitTime = originalWaitTime;

        if (CheckComma(pattern))
        {
            newWaitTime = originalWaitTime * 3.5f;
            return true;
        }

        if (CheckSpace(pattern))
        {
            newWaitTime = 0.0f;
            return true;
        }

        if (CheckPeriod(pattern))
        {
            newWaitTime = originalWaitTime * 7.5f;
            return true;
        }

        return false;
    }

    private bool CheckComma(string pattern)
    {
        return (pattern.Contains(",") || pattern.Contains("、") || pattern.Contains("，"));
    }

    private bool CheckSpace(string pattern)
    {
        return (pattern.Contains(" "));
    }

    private bool CheckPeriod(string pattern)
    {
        return (pattern.Contains(".") || pattern.Contains("。") || pattern.Contains("?") || pattern.Contains("!"));
    }

    public void SetTextOffset(Vector2 offset)
    {
        dialogText.GetComponent<RectTransform>().anchoredPosition = offset;
    }

    public void SetTextAlignment(CustomTextAlignment alignment)
    {
        switch (alignment)
        {
            case CustomTextAlignment.topLeft:
                dialogText.alignment = TextAlignmentOptions.TopLeft;
                break;
            case CustomTextAlignment.topCenter:
                dialogText.alignment = TextAlignmentOptions.TopGeoAligned;
                break;
            case CustomTextAlignment.topRight:
                dialogText.alignment = TextAlignmentOptions.TopRight;
                break;
            case CustomTextAlignment.middleLeft:
                dialogText.alignment = TextAlignmentOptions.Left;
                break;
            case CustomTextAlignment.center:
                dialogText.alignment = TextAlignmentOptions.Center;
                break;
            case CustomTextAlignment.middleRight:
                dialogText.alignment = TextAlignmentOptions.Right;
                break;
            case CustomTextAlignment.bottomLeft:
                dialogText.alignment = TextAlignmentOptions.BottomLeft;
                break;
            case CustomTextAlignment.bottomCenter:
                dialogText.alignment = TextAlignmentOptions.BottomGeoAligned;
                break;
            case CustomTextAlignment.bottomRight:
                dialogText.alignment = TextAlignmentOptions.BottomRight;
                break;
        }
    }

    public void SetTextWrappingMode(bool enable)
    {
        dialogText.enableWordWrapping = enable;
    }

    // Add new texts
    public void AddNewText(string newText, Vector2 location, float size, Color color)
    {
        // create text
        TMP_Text tmp = Instantiate(dialogText.gameObject, transform).GetComponent<TMP_Text>();
        extraTextList.Add(tmp);

        Vector2 bottomleft = -GetWindowSize() / 2f;
        tmp.GetComponent<RectTransform>().anchoredPosition = bottomleft + location;
        tmp.fontSize = size;
        tmp.text = newText;
        tmp.color = color;
        tmp.gameObject.SetActive(windowState == WindiowState.opened);
    }

    public void EnableAllEntraText(bool boolean)
    {
        foreach (TMP_Text txt in extraTextList)
        {
            txt.gameObject.SetActive(boolean);
        }
    }

    public void AddNewImage(string path, Vector2 location, Vector2 size, bool behindWindow)
    {
        // create text
        Image tmp = Instantiate(baseImage.gameObject, transform).GetComponent<Image>();
        extraImageList.Add(tmp);

        Vector2 bottomleft = -GetWindowSize() / 2f;
        tmp.GetComponent<RectTransform>().anchoredPosition = bottomleft + location;
        tmp.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        tmp.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        tmp.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        tmp.GetComponent<RectTransform>().sizeDelta = size;

        int sorting = behindWindow ? -1 : 1;
        tmp.GetComponent<Canvas>().sortingOrder = sorting * (extraImageList.Count+1);

        tmp.color = Color.white;
        tmp.sprite = Resources.Load<Sprite>(path);

        tmp.gameObject.SetActive(windowState == WindiowState.opened);
    }


    public void EnableAllEntraImage(bool boolean)
    {
        foreach (Image img in extraImageList)
        {
            img.gameObject.SetActive(boolean);
        }
    }
}
