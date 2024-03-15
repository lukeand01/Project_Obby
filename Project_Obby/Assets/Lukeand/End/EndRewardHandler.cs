using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndRewardHandler : MonoBehaviour
{
    [SerializeField] EndRewardUnit rewardGold;
    [SerializeField] ButtonBase rewardGoldAdButton; 
    [SerializeField] EndRewardUnit rewardGem;

    public void AddToRewardGold(int value)
    {
        rewardGold.gameObject.SetActive(true);
        rewardGold.Add(value);
    }

    public void CreateAdButton(bool gotAllCoin)
    {
        //we need to know if it got all coins or not.
        string adString = "";

        if (gotAllCoin)
        {
            adString = "3X Your Coin";
        }
        else
        {
            adString = "2X Your Coin";
        }

        rewardGoldAdButton.SetText(adString);
        rewardGoldAdButton.gameObject.SetActive(true);

       StartCoroutine(AdButtonScaleProcess());
       StartCoroutine(AdButtonRotateProcess());
    }


    public void ResetAdButton()
    {
        rewardGoldAdButton.DOKill();
        rewardGoldAdButton.transform.DOScale(0.45f, 0);
        rewardGoldAdButton.transform.DOLocalRotate(new Vector3(0, 0, 0), 0);
    }

    IEnumerator AdButtonScaleProcess()
    {
        //i will shake it and increase it.
        float timer = 1.5f;

        rewardGoldAdButton.transform.DOScale(0.5f, timer);

        yield return new WaitForSeconds(timer);

        rewardGoldAdButton.transform.DOScale(0.45f, timer);

        yield return new WaitForSeconds(timer);

        StartCoroutine(AdButtonScaleProcess());

    }
    IEnumerator AdButtonRotateProcess()
    {
        float timer = 1;
        rewardGoldAdButton.transform.DOLocalRotate(new Vector3(0, 0, 3), timer);

        yield return new WaitForSeconds(timer);

        rewardGoldAdButton.transform.DOLocalRotate(new Vector3(0, 0, -3), timer);

        yield return new WaitForSeconds(timer);

        StartCoroutine(AdButtonRotateProcess());
    }

   
    public void AddToRewardGem(int value)
    {
        rewardGem.gameObject.SetActive(true);
        rewardGem.Add(value);   
    }

    


}
