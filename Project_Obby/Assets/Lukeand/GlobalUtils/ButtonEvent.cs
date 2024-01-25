using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonEvent : ButtonBase
{
    public UnityEvent unityEvent;


    
    public override void OnPointerClick(PointerEventData eventData)
    {
        unityEvent.Invoke();
    }
}
