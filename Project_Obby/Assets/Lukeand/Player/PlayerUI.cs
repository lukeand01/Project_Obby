using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    //this shows the powers, health and moeny
    GameObject holder;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] GameObject heartHolder;
    [SerializeField] Transform heartPosRef;
    [SerializeField] Image[] hearts;

    Color fullHeartColor;
    Color emptyHeartColor;

    private void Awake()
    {
        fullHeartColor = Color.white;
        emptyHeartColor = Color.black;

        timerOriginalPos = timerHolder.transform.position;

        holder = transform.GetChild(0).gameObject;
    }

    public void ControlHolder(bool shouldBeVisible)
    {
        holder.SetActive(shouldBeVisible);
    }


    public void UpdateLives(int amount)
    {

        for (int i = 0; i < hearts.Length; i++)
        {

            if(amount > i)
            {
                hearts[i].color = fullHeartColor;
            }
            else
            {
                hearts[i].color = emptyHeartColor;
            }
        }



    }

    public Vector3 GetLifePos() => heartPosRef.position;


    [Separator("COIN")]
    [SerializeField] GameObject coinHolder;
    [SerializeField] TextMeshProUGUI coinText;

    //the problem is that localcoins are not counted to the real thing only when you finish the thing.
    //

    public void UpdateCoin(int total, int change = 0)
    {
        //update this fella.
        if(change != 0)
        {

        }

        coinText.text = total.ToString();

        StopCoroutine(nameof(CoinProcess));
        StartCoroutine(CoinProcess());

    }

    IEnumerator CoinProcess()
    {
        //increase it. only that.
        float timer = 0.2f;

        coinText.transform.DOKill();

        coinText.transform.DOScale(1, 0); //we put it in the right position
        coinText.transform.DOScale(1.3f, timer);

        yield return new WaitForSeconds(timer);

        coinText.transform.DOScale(1, timer);


    }

    [Separator("TIMER")]
    [SerializeField] GameObject timerHolder;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Transform timerPosRefForShow;
    [SerializeField] Transform timerPosRefForHide;
    Vector3 timerOriginalPos;

    public void UpdateTimerUI(int minutes, int seconds)
    {
        timerText.text = minutes.ToString() + ":" + seconds.ToString();
    }

    public void UpdateTimerStringUI(string value)
    {
        timerText.text = value;
    }

    public IEnumerator TimerAnimationProcess()
    {
        float timer = 0.5f;
        timerHolder.transform.DOScale(1.3f, timer);
        yield return new WaitForSeconds(timer);
        timerHolder.transform.DOScale(1, timer);
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
        
        timerHolder.transform.DOMoveY(timerPosRefForShow.position.y, 1.5f);     
    }
    public void ResetTimer()
    {
        timerHolder.transform.DOMoveY(timerPosRefForHide.position.y, 0.01f);
    }

    public Vector3 GetTimerPos() => timerText.transform.position;   
    

}


//