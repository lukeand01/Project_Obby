using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using MyBox;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class ConfirmationWindowUI : MonoBehaviour
{
    //this receives informations and links itself to whatever sent the request for confirmation. then it send the choice back.

    GameObject holder;


    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] GameObject buttonHolder;

    [Separator("CHANGE CONFIRM BUTTON")]
    [SerializeField] ConfirmationWindowButton confirmationButton;

    [Separator("COIN")]
    [SerializeField] GameObject coinHolder;
    [SerializeField] TextMeshProUGUI coinText;


    bool inProcess = false;

    public Action eventConfirm;
    public Action eventCancel;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }


     void ClearEvents()
    {
        eventCancel = delegate { };
        eventConfirm = delegate { };    
    }

    public void StartConfirmationWindow(string title, string description, bool hasScreenButton = false)
    {
        //you assign something and then send the information to show here.
        if (holder.activeInHierarchy)
        {
            Debug.Log("there is holder already");
            return;
        }

        //screenButton.SetActive(hasScreenButton);

        //i also need to know the currency.
 
        coinHolder.SetActive(false);

        titleText.text = title;
        descriptionText.text = description; 
        StopAllCoroutines();
        StartCoroutine(OpenProcess());

    }


    public void StartCoinHolder()
    {
        coinHolder.SetActive(true);

        int currentCoin = PlayerHandler.instance.coins;
        coinText.text = currentCoin.ToString();
    }

    public void ShakeGoldHolder()
    {
        StopCoroutine(nameof(ShakeGoldHolderProcess));
        StartCoroutine(ShakeGoldHolderProcess());
    }

    IEnumerator ShakeGoldHolderProcess()
    {

        float colorTimer = 0.8f;

        coinText.DOKill();

        coinText.DOColor(Color.white, 0); //set up in the righ5t place.
        coinText.DOColor(Color.red, colorTimer);

        coinText.transform.localPosition = Vector3.one;

        for (int i = 0; i < 30; i++)
        {
            //and we shaek the bastard randoly in X
            float randomValueX = UnityEngine.Random.Range(-0.65f, 0.65f);
            coinText.transform.localPosition = Vector3.zero + new Vector3(randomValueX, 0, 0);
            yield return new WaitForSeconds(0.02f);
        }

        coinText.transform.localPosition = Vector3.one;

        coinText.DOColor(Color.white, colorTimer / 2);




    }


    public void ChangeConfirmTextString(string text)
    {
        confirmationButton.UpdateString(text);
    }
      
    public void ChangeConfirmTextValue(CurrencyType currency, string text)
    {
        confirmationButton.UpdateValueText(currency, text);
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
        ClearEvents();

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

public enum ConfirmationType
{
    String,
    Gem,
    Coin,
    Warning
}