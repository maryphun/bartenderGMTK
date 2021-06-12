using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkManager : MonoBehaviour
{
    [SerializeField] string windowName;

    public void Initialization()
    {
        // create window
        Vector2 size = new Vector2(Screen.width / 5.25f, Screen.height);
        Vector2 pos = new Vector2((Screen.width/2f) - size.x / 2f, (Screen.height/2f) - size.y / 2f);
        float offsetY = 5f;
        WindowManager.Instance.CreateWindow(windowName, pos, size);

        //for (int i = 0; i < 5; i++)
        //{
        //    pos.y = (Screen.height / 2f) - ((size.y) * i+1) - size.y / 2f;
        //    WindowManager.Instance.CreateWindow(windowName + i.ToString(), pos, size);
        //    WindowManager.Instance.Open(windowName + i.ToString(), 0.0f);
        //}
    }

    private void OpenWindow(bool boolean)
    {
        if (boolean)
        {
            WindowManager.Instance.Open(windowName, 0.0f);
        }
        else
        {
            WindowManager.Instance.Close(windowName, 0.0f, false);
        }
    }

    private void EnableDrink(bool boolean)
    {
        
    }
}
