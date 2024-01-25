using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputButton : ButtonBase
{
    public float value;

    public UnityEvent unityEvent;

    Vector2 activePos;
    Vector2 inactivePos;
    GameObject holder;
    [SerializeField] Animator inputButtonAnimation;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;

        if (holder.name != "Holder") holder = null;

        if (holder == null) return;

        inactivePos = holder.transform.localPosition;
        activePos = holder.transform.localPosition + new Vector3(0, -25, 0);

    }


    #region EVENT
    public event Action EventPressed;
    public void OnPressed() => EventPressed?.Invoke();

    public event Action EventReleased;
    public void OnReleased() => EventReleased?.Invoke();
    #endregion

    private void OnDisable()
    {
        value = 0;      
    }


    private void FixedUpdate()
    {

        if(value == 1)
        {
            holder.transform.localPosition = Vector3.MoveTowards(holder.transform.localPosition, activePos, 15);

            if(inputButtonAnimation != null)
            {
                inputButtonAnimation.enabled = true;
            }

        }
        else
        {
            holder.transform.localPosition = Vector3.MoveTowards(holder.transform.localPosition, inactivePos, 15);

            if (inputButtonAnimation != null)
            {
                inputButtonAnimation.enabled = false;
            }
        }
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        value = 1;
        unityEvent.Invoke();
        OnPressed();

    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (value == 1) OnReleased();
        value = 0;


    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }


}
