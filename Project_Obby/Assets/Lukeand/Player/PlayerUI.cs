using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    //this shows the powers, health and moeny

    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI livesText;

    public void UpdateGold(int amount, int change = 0)
    {
        goldText.text = "Gold: " + amount.ToString();
    }
    public void UpdateLives(int amount)
    {
        livesText.text = "Lives: " + amount.ToString();
    }


    [Separator("TIMER")]
    [SerializeField] TextMeshProUGUI timerText;
    

    public void UpdateTimerUI(int minutes, int seconds)
    {
        timerText.text = minutes.ToString() + ":" + seconds.ToString();
    }

    public void TriggerTimerRedWarning()
    {
        StopCoroutine(TimerRedWarningProcess());
        StartCoroutine(TimerRedWarningProcess());
    }
    public void ResetTimerColor()
    {
        timerText.DOColor(Color.white, 0);
    }
    public void LeaveTimerRed()
    {
        timerText.DOColor(Color.red, 0);
    }



    IEnumerator TimerRedWarningProcess()
    {
        timerText.DOColor(Color.red, 0.5f);
        yield return new WaitForSeconds(0.5f);
        timerText.DOColor(Color.white, 0.5f);
    }

    public void ShowTimer()
    {
        timerText.transform.DOMoveY(462, 2.5f);     
    }
    public void ResetTimer()
    {
        timerText.transform.DOMoveY(1000, 0.01f);
    }

    

}
