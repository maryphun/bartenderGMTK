using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DrinkManager : MonoBehaviour
{
    [SerializeField] string windowNameBase;
    [SerializeField] string windowNameLayer;
    [SerializeField] GameObject buttonsBase;
    [SerializeField] GameObject buttonsLayer;
    [SerializeField, Range(0.0f, 1.0f)] float alphaAfterDisable = 0.15f;

    private string currentWindowName;

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
}
