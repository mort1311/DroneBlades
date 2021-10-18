using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public bool isPointerUp = false;
    public bool isPointerDown = false;

    public override void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
        
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        isPointerUp = true;
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}