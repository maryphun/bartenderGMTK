using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class OnHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] UnityEvent onHold;

    private bool isPressed;

    private void Update()
    {
        if (isPressed)
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
        isPressed = false;
    }
}