using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPower : MonoBehaviour
{
    bool hasTouched;
    [SerializeField] PowerData power;

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


        confirmationWindow.eventConfirm += OnConfirmed;
        confirmationWindow.eventCancel += OnCancelled;

        confirmationWindow.StartConfirmationWindow(power.temporaryPowerDescription);

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
        ResetTouch();
        power.AddPower();
        Destroy(gameObject);
        Debug.Log("confimed");
    }

    void OnCancelled()
    {
        ResetTouch();

        Debug.Log("cancelled");
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
