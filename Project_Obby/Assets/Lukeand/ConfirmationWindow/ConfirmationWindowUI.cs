using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class ConfirmationWindowUI : MonoBehaviour
{
    //this receives informations and links itself to whatever sent the request for confirmation. then it send the choice back.

    GameObject holder;

    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI confirmText;
    [SerializeField] TextMeshProUGUI cancelText;
    [SerializeField] GameObject screenButton;

    bool inProcess = false;

    public Action eventConfirm;
    public Action eventCancel;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    public void StartConfirmationWindow(string title, bool hasScreenButton = false)
    {
        //you assign something and then send the information to show here.
        if (holder.activeInHierarchy)
        {
            Debug.Log("there is holder already");
            return;
        }

        screenButton.SetActive(hasScreenButton);

        ChangeConfirmText("Confirm");
        ChangeCancelText("Cancel");

        titleText.text = title;
        StopAllCoroutines();
        StartCoroutine(OpenProcess());

    }

    


    public void ChangeConfirmText(string text)
    {
        confirmText.text = text;
    }
    public void ChangeCancelText(string text)
    {
        cancelText.text = text;
    }
    
    //if move away from the thing it closes.

    public void CloseConfirmationWindow()
    {
        StopAllCoroutines();
        StartCoroutine(CloseProcess());
    }

    IEnumerator OpenProcess()
    {
        holder.SetActive(true);
        holder.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        float timeForAnimation = 0.5f;
        holder.transform.DOScale(1, timeForAnimation);

        yield return new WaitForSeconds(timeForAnimation);
    }

    IEnumerator CloseProcess()
    {
        float timeForAnimation = 0.5f;
        holder.transform.DOScale(0.1f, timeForAnimation);
        yield return new WaitForSeconds(timeForAnimation);
        holder.SetActive(false);

    }


    public void Confirm()
    {
        if(eventConfirm != null)
        {
            eventConfirm.Invoke();
        }
    }
    public void Cancel()
    {
        if (eventCancel != null)
        {
            eventCancel.Invoke();
        }
    }

    public void ForceClose()
    {
        Debug.Log("force close was clicked");
        StopAllCoroutines();
        StartCoroutine(CloseProcess());
    }

}
