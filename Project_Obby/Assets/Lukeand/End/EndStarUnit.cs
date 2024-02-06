using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EndStarUnit : MonoBehaviour
{

    [SerializeField] Transform starHolder;
    [SerializeField] UnityEngine.UI.Image[] allStars;
    
    Color fullStarColor = Color.white;
    Color emptyStarColor = Color.black;

    Vector3 coinPos;
    Vector3 heartPos;
    Vector3 timerPos;

    [SerializeField] UnityEngine.UI.Image templateStar;

    private void Start()
    {


        timerPos = UIHandler.instance.uiPlayer.GetTimerPos();
        heartPos = UIHandler.instance.uiPlayer.GetLifePos();
        coinPos = UIHandler.instance.uiEnd.GetCoinPos();
    }

    //this needs the ref for all places it wants to use.
    //coin pos, heartpos, timerpos




    public IEnumerator GetStarFromPlacesProcess()
    {
        //here we will shot a star from the regions that we gained it and take it to the right place.
        //now we need to get the value and the reasons for the stars.

        //we check every single one.
        //we instantiate and make it move
        //and wait for it to complete.

        LocalHandler handler  = LocalHandler.instance;

        

        if (handler.GainedStarByCoin())
        {
            Debug.Log("gained star by coin");
            yield return StartCoroutine(OrderStarAnimation(coinPos));
        }

        if (handler.GainedStarByHealth())
        {
            Debug.Log("gained star by health");
            yield return StartCoroutine(OrderStarAnimation(heartPos));
            
        }

        if (handler.GainedStarByTimer())
        {
            Debug.Log("gained star by timer");
            yield return StartCoroutine(OrderStarAnimation(timerPos));
        }

       
    }


    IEnumerator ReceiveAnimationProcess()
    {
        float timer = 0.3f;
        starHolder.DOScale(1.3f, timer);
        yield return new WaitForSeconds(timer);
        starHolder.DOScale(1, timer);

    }
    IEnumerator ShakeProcess()
    {

        Debug.Log("shake process");

        Vector3 originalPos = starHolder.transform.position;
        //Vector3 offsetPos = 
        float intensity = 1.5f;

        for (int i = 0; i < 25; i++)
        {
            
            float x = Random.Range(-intensity, intensity);
            float y = Random.Range(-intensity, intensity);

            Vector3 newPos = new Vector3(originalPos.x + x, originalPos.y + y , originalPos.z);

            starHolder.transform.position = newPos;

            yield return new WaitForSeconds(0.05f);
        }
        Debug.Log("end");
        starHolder.transform.position = originalPos;

    }

    IEnumerator OrderStarAnimation(Vector3 pos)
    {
        int targetIndex = GetNextEmptyStar();

        if (targetIndex == -1)
        {
            Debug.Log("there was a problem here");
        }

        float timeToReach = 1f;
        GameObject newObject = GetStarObject(pos);


        yield return new WaitForSeconds(0.2f);

        newObject.transform.DOMove(allStars[targetIndex].transform.position, timeToReach);

        yield return new WaitForSeconds(timeToReach);

        StartCoroutine(ReceiveAnimationProcess());

        Destroy(newObject);
        ActiveTheFirstEmptyStar();
    }


    
    GameObject GetStarObject(Vector3 pos)
    {

       
        GameObject newObject = Instantiate(templateStar.gameObject, Vector3.zero, Quaternion.identity);
        newObject.SetActive(true);
        newObject.transform.SetParent(transform);
        newObject.transform.position = pos;

        newObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        newObject.transform.DOScale(1, 0.3f);


        return newObject;

    }


    int GetNextEmptyStar()
    {
        for (int i = 0; i < allStars.Length; i++)
        {
            if (allStars[i].color == emptyStarColor)
            {
                return i;
            }
        }
        return -1;
    }

    void ActiveTheFirstEmptyStar()
    {
        //we get the first star that is empty and make it full.
        //everytime we do this we 
        foreach (var item in allStars)
        {
            if(item.color == emptyStarColor)
            {
                item.color = fullStarColor;
                break;
            }
        }

    }

    public void MakeAllStarsEmpty()
    {
        foreach (var item in allStars)
        {
            item.color = emptyStarColor;
        }
    }


}
