using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPower : MonoBehaviour
{
    bool hasTouched;
    public StoreData data;
    StorePowerData powerStoreData;

    //need to take it from thing. need to show how much coins the player has.

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Player") return;
        if (hasTouched) return;

        hasTouched = true;

        ConfirmationWindowUI confirmationWindow = UIHandler.instance.uiConfirmationWindow;

        if(confirmationWindow == null)
        {
            Debug.LogError("COULDNT FIND CONFIRMATION WINDOW");
            return;
        }


        //we ask here if it has the money already.

        powerStoreData = data.GetPower();

        //i need to decide here.

       confirmationWindow.StartConfirmationWindow("Purchase", data.storeDescription);
       confirmationWindow.ChangeConfirmTextValue(CurrencyType.Coin, powerStoreData.tempPrice.ToString());
        confirmationWindow.StartCoinHolder();

       confirmationWindow.eventConfirm += OnConfirmed;
       confirmationWindow.eventCancel += OnCancelled;
        //also if you move the buttons or do anything you get close it.
    }

    private void Update()
    {
        if(hasTouched)
        {
            float distance = Vector3.Distance(transform.position, PlayerHandler.instance.transform.position);

            if(distance > 4)
            {
                ResetTouch();
            }


        }
    }


    void OnConfirmed()
    {

        if(data.GetPower() == null)
        {
            Debug.Log("problem here");
            return;
        }


        bool canBuy = PlayerHandler.instance.coins >= powerStoreData.tempPrice;


        if(canBuy)
        {
            UIHandler.instance.uiConfirmationWindow.CloseConfirmationWindow();
            ResetTouch();
            PlayerHandler.instance.AddTempPower(powerStoreData.powerData);
            Destroy(gameObject);
        }
        else
        {
            //then we shake the gold holder. and thats it.
            Debug.Log("on confirmed");
            UIHandler.instance.uiConfirmationWindow.ShakeGoldHolder();
        }

    }

    void OnCancelled()
    {
        ResetTouch();
        //and we close it.

        UIHandler.instance.uiConfirmationWindow.CloseConfirmationWindow();
    }

    void ResetTouch()
    {
        //
        hasTouched = false;

        ConfirmationWindowUI confirmationWindow = UIHandler.instance.uiConfirmationWindow;

        confirmationWindow.eventConfirm -= OnConfirmed;
        confirmationWindow.eventCancel -= OnCancelled;

        confirmationWindow.CloseConfirmationWindow();

    }

}
