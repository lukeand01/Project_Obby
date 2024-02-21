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

    [Separator("SOUND")]
    [SerializeField] AudioClip gainCoinSound;


    float offset = 100;

    public void PutAllPiecesInStartingPos()
    {

        offset = 500;

        //titleText.transform.position += Vector3.left * offset;
        //coinText.transform.position += Vector3.left * offset;
        //timerText.transform.position += Vector3.left * offset;
        //heartHolder.transform.position += Vector3.left * offset;

    }


    public IEnumerator StartAchievementProcess(EndUI handler)
    {
        //first it says "you completed" and that gives a star.
        //then it shows all the coins grabbed.
        //then we pass the coins to the reward.
        //rewards already creates ad.
        //then we show the timer concluded. that might earn another star.
        //then we show the health. that might earn another star.

        float speed = 1;


        titleText.transform.DOMove(titleText.transform.position + Vector3.right * offset, speed);

        yield return handler.StarHolder.OrderStarAnimation(titleText.transform.position);

        coinText.transform.DOMove(coinText.transform.position + Vector3.right * offset, speed);

        //and now i need to start counting the coin.
        //so i need the number of obtained coins.
        




        yield break;
    }


    public bool CallTitle()
    {
        float speed = 1;
        titleText.transform.DOMove(titleText.transform.position + Vector3.right * offset, speed);
        return true;
    }

    public IEnumerator CallCoinProcess()
    {
        int obtainedCoins = LocalHandler.instance.gainedCoin;
        int totalCoins = LocalHandler.instance.coins.Length;
        int currentCoins = 0;

        float speed = 1;

        coinText.text = $"Coins: {currentCoins} / {totalCoins}";
        coinText.transform.DOMove(coinText.transform.position + Vector3.right * offset, speed);

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

        timerText.transform.DOMove(timerText.transform.position + Vector3.right * offset, timer);
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
        heartHolder.transform.DOMove(heartHolder.transform.position + Vector3.right * offset, timer);
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
