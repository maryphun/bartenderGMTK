using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DrinkManager : MonoBehaviour
{
    [SerializeField] string windowNameBase;
    [SerializeField] string windowNameLayer;
    [SerializeField, Range(0.0f, 1.0f)] float alphaAfterDisable = 0.15f;
    [SerializeField, Range(0.0f, 1.0f)] float fadeTime = 0.45f;

    private string currentWindowName;

    public void Initialization()
    {
        // create window
        Vector2 size = new Vector2(Screen.width / 5.25f, Screen.height);
        Vector2 pos = new Vector2((Screen.width/2f) - size.x / 2f, (Screen.height/2f) - size.y / 2f);
        float offsetY = 5f;
        WindowManager.Instance.CreateWindow(windowNameBase, pos, size);

        //for (int i = 0; i < 5; i++)
        //{
        //    pos.y = (Screen.height / 2f) - ((size.y) * i+1) - size.y / 2f;
        //    WindowManager.Instance.CreateWindow(windowName + i.ToString(), pos, size);
        //    WindowManager.Instance.Open(windowName + i.ToString(), 0.0f);
        //}

        currentWindowName = windowNameBase;
    }

    public void OpenWindow(bool boolean)
    {
        if (boolean)
        {
            WindowManager.Instance.Open(currentWindowName, 0.0f);
        }
        else
        {
            WindowManager.Instance.Close(currentWindowName, 0.0f, false);
        }
    }

    public void EnableDrink(bool boolean)
    {
        Window window = WindowManager.Instance.GetWindowObject(currentWindowName);
        float alpha = boolean ? 1.0f : alphaAfterDisable;

        Transform tmp = window.transform.Find("Alpha");
        if (tmp == null)
        {
            tmp = Instantiate(window.transform.Find("Image"), window.transform);
            tmp.name = "Alpha";
            tmp.GetComponent<Image>().material = null;
            tmp.GetComponent<Image>().color = new Color(0,0,0,0);
            tmp.GetComponent<Canvas>().sortingOrder = 1000;
        }

        tmp.GetComponent<Image>().DOFade(1f-alpha, fadeTime);
    }
}
