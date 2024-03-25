using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreBaseUnit : ButtonBase
{


    [Separator("BASE")]
    [SerializeField] protected ButtonBase storeActButton;


    public virtual void UpdateAfterBuying()
    {
        //after buying then we should put this item first in the list just for simplifyiung it.

        transform.SetAsFirstSibling();
    }

    //need tor esolve the problem with not being able to click;

    protected void StartStoreActButton(bool isOwned, bool isUsing)
    {
        //i just want it to keep scaling up and down.

        if(storeActButton == null)
        {
            Debug.Log("this does not have storeact");
            return;
        }

        

        storeActButton.gameObject.SetActive(true);

        if (isUsing)
        {
            storeActButton.SetText("USING");

        }
        else if (isOwned)
        {
            storeActButton.SetText("USE");
        }
        if (!isOwned)
        {
            storeActButton.SetText("BUY");
        }


        StopCoroutine(nameof(StoreActButtonProcess));
        storeActButton.transform.DOScale(0.36f, 0);
        StartCoroutine(StoreActButtonProcess());
    }
    protected void CloseStoreActButton()
    {

        if (storeActButton == null)
        {
            Debug.Log("this does not have storeact");
            return;
        }

        storeActButton.gameObject.SetActive(false);

        StopCoroutine(nameof(StoreActButtonProcess));
        storeActButton.transform.DOScale(0.36f, 0);
    }

    IEnumerator StoreActButtonProcess()
    {
        float timer = 1;

        storeActButton.transform.DOScale(0.45f, timer);

        yield return new WaitForSeconds(timer);

        storeActButton.transform.DOScale(0.36f, timer);

        yield return new WaitForSeconds(timer);

        StartCoroutine(StoreActButtonProcess());
    }



}
