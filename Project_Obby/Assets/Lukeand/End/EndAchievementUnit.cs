using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndAchievementUnit : MonoBehaviour
{
    //all achievements possible
    //completed the stage! 
    //

    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject heartHolder;
    [SerializeField] Image[] hearts;


    Vector3 titleOriginalPos;
    Vector3 coinOriginalPos;
    Vector3 timerOriginalPos;
    Vector3 heartOriginalPos;

    [Separator("SOUND")]
    [SerializeField] AudioClip gainCoinSound;


    float offset = 100;


    //I need to show 
    //i want to constantly showing the stars you can gain
    //


    private void Awake()
    {
        titleOriginalPos = titleText.transform.position;
        coinOriginalPos = coinText.transform.position;
        timerOriginalPos = timerText.transform.position;
        heartOriginalPos = heartHolder.transform.position;




        //then we move awayu fromt the screen.
        //and we need to rest position.

    }

    public void PutAllPiecesInStartingPos()
    {

        float offset = Screen.width * 1.4f;

        titleText.transform.DOMove(titleOriginalPos + Vector3.left * offset, 0);
        coinText.transform.DOMove(coinOriginalPos + Vector3.left * offset, 0);
        timerText.transform.DOMove(timerOriginalPos + Vector3.left * offset, 0);
        heartHolder.transform.DOMove(heartOriginalPos + Vector3.left * offset, 0);



    }


 

    public bool CallTitle()
    {
        float speed = 1;
        titleText.transform.DOMove(titleOriginalPos, speed);
        return true;
    }

    public IEnumerator CallCoinProcess()
    {
        int obtainedCoins = LocalHandler.instance.gainedCoin;
        int totalCoins = LocalHandler.instance.coins.Length;
        int currentCoins = 0;

        float speed = 1;

        coinText.text = $"Coins: {currentCoins} / {totalCoins}";
        coinText.transform.DOMove(coinOriginalPos, speed);

        yield return new WaitForSeconds(speed);

       
        while (obtainedCoins > currentCoins)
        {
            currentCoins += 1;

            coinText.transform.DOScale(1.2f, 0.25f);

            coinText.text = $"Coins: {currentCoins} / {totalCoins}";

            GameHandler.instance.soundHandler.CreateSFX(gainCoinSound);

            yield return new WaitForSeconds(0.25f);

            coinText.transform.DOScale(1, 0.25f);

            yield return new WaitForSeconds(0.25f);
        }


        


        //then here we send all the coins towards the reward.
        //and it stacks in the 
        


     

    }


    public bool CallTimer(float timer)
    {
        //
        StageTimeClass currentTimeClass = LocalHandler.instance.currentTimer;
        bool isSuccess = currentTimeClass.IsCurrentMoreThanHalfTheOriginal();

        timerText.transform.DOMove(timerOriginalPos, timer);
        timerText.text = $"Timer {currentTimeClass.minutes} : {currentTimeClass.seconds}";

        return isSuccess;
    }

    IEnumerator CallTimerProcess()
    {
        //this is the actuall efffect.
        yield break; 
    }


    public bool CallHeart(float timer)
    {
        heartHolder.transform.DOMove(heartOriginalPos, timer);
        bool hasAllHearts = PlayerHandler.instance.currentHealth >= 3;

        StartCoroutine(CallHeartProcess());

        return hasAllHearts;
    }

     IEnumerator CallHeartProcess()
    {
        //get the quantity of hearts and call each increasing each and then by the end increase everything.
        int heartQuantity = PlayerHandler.instance.currentHealth;

        //this resets the heart.
        foreach (var item in hearts)
        {
            item.color = Color.black;
        }

        //each hearts does a little dance.
        //if all hearts it does a big 
        float speed = 0.2f;

        for (int i = 0; i < heartQuantity; i++)
        {
            hearts[i].color = Color.white;
            hearts[i].transform.DOScale(1.3f, speed);
            yield return new WaitForSeconds(speed);
            hearts[i].transform.DOScale(1, speed);
            yield return new WaitForSeconds(speed);
        }

        if(heartQuantity == 3)
        {
            //it does a big 
            heartHolder.transform.DOScale(1.3f, speed);
            yield return new WaitForSeconds(speed);
            heartHolder.transform.DOScale(1f, speed);
        }


    }



    public Vector3 GetTitlePos() => titleText.transform.position;
    public Vector3 GetTimerPos() => timerText.transform.position;   

    public Vector3 GetCoinPos() => coinText.transform.position;

    public Vector3 GetHeartPos() => heartHolder.transform.position;
}
