using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OnHover : MonoBehaviour
{
    [SerializeField, TextArea(15, 20)] string description;

    bool mousehovering, isShowingExplaination;
    float hoveringTimeCount;
    Vector2 lastMousePoint;
    Vector2 size = new Vector2(700f, 300f);
    const float hoveringTimeRequired = 0.385f;

    // Start is called before the first frame update
    void OnMouseEnter()
    {
        mousehovering = true;
        hoveringTimeCount = -0.0f;
        lastMousePoint = Input.mousePosition;
    }

    void OnMouseOver()
    {
        if (!mousehovering)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            hoveringTimeCount = 0.0f;
            mousehovering = false;
            if (isShowingExplaination)
            {
                isShowingExplaination = false;
                EnableExplaination(isShowingExplaination);
            }
            return;
        }

        Vector2 tmp = Input.mousePosition;
        if (Vector2.Distance(lastMousePoint, tmp) < 2f)
        {
            hoveringTimeCount += Time.deltaTime;
        }
        else
        {
            hoveringTimeCount = 0.0f;
        }
        lastMousePoint = Input.mousePosition;

        if (hoveringTimeCount >= hoveringTimeRequired)
        {
            if (!isShowingExplaination)
            {
                // show explaination
                isShowingExplaination = true;
                EnableExplaination(isShowingExplaination);
            }
        }
        else
        {
            if (isShowingExplaination)
            {
                isShowingExplaination = false;
                EnableExplaination(isShowingExplaination);
            }
        }
    }

    void OnMouseExit()
    {
        mousehovering = false;
        hoveringTimeCount = 0.0f;
        lastMousePoint = Input.mousePosition;

        // close
        if (isShowingExplaination)
        {
            isShowingExplaination = false;
            EnableExplaination(isShowingExplaination);
        }
    }

    private void EnableExplaination(bool boolean)
    {
        string windowName = description.Substring(0, 5);
        if (boolean)
        {
            Vector2 pos = new Vector2(lastMousePoint.x - (Screen.width / 2) - size.x / 2, lastMousePoint.y - (Screen.height / 2) - size.y / 2);
            pos = ProcessPos(pos);

            if (WindowManager.Instance.IsWindowExist(windowName))
            {
                WindowManager.Instance.Open(windowName, 0.1f);
                WindowManager.Instance.GetWindowObject(windowName).transform.DOMove(pos, 0.01f);
            }
            else
            {
                WindowManager.Instance.CreateWindow(windowName, pos, size);
                WindowManager.Instance.Open(windowName, 0.05f);
                WindowManager.Instance.SetText(windowName, description, 0.005f);
                WindowManager.Instance.SetTextSize(windowName, 45f);
                WindowManager.Instance.SetTextAlignment(windowName, CustomTextAlignment.center);
                WindowManager.Instance.SetTextWrappingMode(windowName, true);
            }
        }
        else
        {
            if (WindowManager.Instance.IsWindowExist(windowName))
            {
                WindowManager.Instance.Close(windowName, 0.1f, true);
            }
        }
    }

    Vector2 ProcessPos(Vector2 originalPos)
    {
        Vector2 rtn = originalPos;

        if (rtn.y - size.y / 2f < -Screen.height/2f)
        {
            rtn = new Vector2(rtn.x, -Screen.height / 2f + size.y / 2f);
        }

        return rtn;
    }
}
