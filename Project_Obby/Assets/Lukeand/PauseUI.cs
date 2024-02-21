using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    GameObject holder;


    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    public void StartPause()
    {
        GameHandler.instance.PauseTimeScale();
        holder.SetActive(true);

    }
    public void StopPause()
    {
        GameHandler.instance.ResumeTimeScale();
        holder.SetActive(false);
    }


    public void OrderReturnToMainMenu()
    {

        


        GameHandler.instance.sceneLoader.ChangeToMainMenu();
    }

}
