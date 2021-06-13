using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class OnHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] UnityEvent onHold,onRelease;

    private bool isPressed;
    private bool isEnabled;

    private void Update()
    {
        if (isPressed && isEnabled)
        {
            onHold.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (isPressed && isEnabled)
        {
            onRelease.Invoke();
        }
        isPressed = false;
    }

    public void SetEnable(bool boolean)
    {
        isEnabled = boolean;
    }
}