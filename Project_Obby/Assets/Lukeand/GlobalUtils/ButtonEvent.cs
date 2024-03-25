using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonEvent : ButtonBase
{
    public UnityEvent unityEvent;

    [Separator("EVENT")]
    [SerializeField] GameObject lockedHolder;
    
    public void ControlLocked(bool isVisible)
    {
        lockedHolder.SetActive(isVisible);
    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        unityEvent.Invoke();
    }
}
