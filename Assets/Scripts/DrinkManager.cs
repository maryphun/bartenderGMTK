using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DrinkManager : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] string windowNameBase;
    [SerializeField] string windowNameLayer;
    [SerializeField, Range(0.0f, 1.0f)] float alphaAfterDisable = 0.15f;
    [SerializeField, Range(0.0f, 1.0f)] float liquidFillPercentageMin = 0.9f;

    [Header("References")]
    [SerializeField] GameObject buttonsBase;
    [SerializeField] GameObject buttonsLayer;
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] RectTransform liquidFill;

    [Header("Debug")]
    [SerializeField] private float liquidFillPercentage;
    [SerializeField] private float _happiness;
    [SerializeField] private float _sadness;
    [SerializeField] private float _surprise;
    [SerializeField] private float _excited;
    [SerializeField] private float _anger;

    // privates
    private string currentWindowName;
    private float liquidCount;

    public void Initialization()
    {
        // initialize pos and size
        Vector2 size = GetSize();
        Vector2 pos = GetPos();

        // create window
        WindowManager.Instance.CreateWindow(windowNameBase, pos, size);
        WindowManager.Instance.CreateWindow(windowNameLayer, new Vector2(pos.x + size.x, pos.y), size);

        // setup buttons
        GameObject[] tmp = new GameObject[2];
        tmp[0] = Instantiate(buttonsBase, WindowManager.Instance.GetWindowObject(windowNameBase).transform);
        tmp[0].GetComponent<DrinkMenu>().Setup(windowNameBase, this);

        tmp[1] = Instantiate(buttonsLayer, WindowManager.Instance.GetWindowObject(windowNameLayer).transform);
        tmp[1].GetComponent<DrinkMenu>().Setup(windowNameLayer, this);

        // set default
        currentWindowName = windowNameBase;
        liquidFillPercentage = 0.0f;
    }

    public Vector2 GetSize()
    {
        return new Vector2(Screen.width / 5.25f, Screen.height);
    }
    public Vector2 GetPos()
    {
        return new Vector2((Screen.width / 2f) - GetSize().x / 2f, (Screen.height / 2f) - GetSize().y / 2f);
    }

    public void OpenWindow(bool boolean, string name = "")
    {
        if (name == "")
        {
            // affect both
            if (boolean)
            {
                WindowManager.Instance.Open(windowNameBase, 0.0f);
                WindowManager.Instance.Open(windowNameLayer, 0.0f);
            }
            else
            {
                WindowManager.Instance.Close(windowNameBase, 0.0f, false);
                WindowManager.Instance.Close(windowNameLayer, 0.0f, false);
            }
            return;
        }

        if (boolean)
        {
            WindowManager.Instance.Open(name, 0.0f);
        }
        else
        {
            WindowManager.Instance.Close(name, 0.0f, false);
        }
    }

    public void EnableDrink(bool boolean, string name = "default", float time = 0.45f)
    {
        if (name == "default")
        {
            name = windowNameBase;
        }

        Window window = WindowManager.Instance.GetWindowObject(name);
        float alpha = boolean ? 1.0f : alphaAfterDisable;

        Transform tmp = window.transform.Find("Alpha");
        if (tmp == null)
        {
            Debug.Log("Alpha not found!");
        }

        if (boolean)
        {
            window.transform.SetSiblingIndex(1);
            WindowManager.Instance.GetWindowObject(GetOtherName(name)).transform.SetSiblingIndex(0);
        }

        tmp.SetSiblingIndex(window.transform.childCount);
        tmp.GetComponent<Image>().DOFade(1f-alpha, time);
    }

    public string GetOtherName(string name)
    {
        string rtn = "";

        if (name == windowNameBase)
        {
            rtn = windowNameLayer;
        }
        if (name == windowNameLayer)
        {
            rtn = windowNameBase;
        }

        return rtn;
    }

    public void Mix()
    {
        if (liquidFillPercentage >= liquidFillPercentageMin)
        {

        }
        else
        {
            // not enough.
            // TODO: play failed SE
            if (!dialogueManager.IsShowingDialogue())
            {
                // Add dialog if possible.
                string[] randomString = new string[3];
                randomString[0] = "Not enough ingredients, add more!";
                randomString[1] = "The drink is not finish yet.";
                randomString[2] = "No, not yet.";
                dialogueManager.RegisterDialogue(randomString[Random.Range(0, randomString.Length-1)]);
            }
        }
    }

    public void ChangeParameter(float happy, float sad, float surprise, float excited, float anger)
    {
        _happiness += happy;
        _sadness += sad;
        _surprise += surprise;
        _excited += excited;
        _anger += anger;
    }

    public void FillShaker(float value)
    {
        liquidFillPercentage = Mathf.Clamp(liquidFillPercentage + value, 0.0f, 1.0f);
    }

    public DrinkMenu GetDrinkMenu(string name = "")
    {
        if (name == "")
        {
            name = currentWindowName;
        }

        return WindowManager.Instance.GetWindowObject(name).GetComponentInChildren<DrinkMenu>();
    }

    private void Update()
    {
        // control fill
        float fillSize = 180f;
        liquidCount += Time.deltaTime;
        float fill = liquidFillPercentage + ((liquidFillPercentage * 0.25f) * Mathf.Sin(liquidCount));
        Debug.Log(Mathf.Sin(liquidCount));
        liquidFill.sizeDelta = new Vector2(liquidFill.sizeDelta.x, fill * fillSize);
        liquidFill.anchoredPosition = new Vector2(0, (-fillSize / 2f) + (fill * fillSize /2f));
    }
}
