using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class EndStarUnit : MonoBehaviour
{

    [SerializeField] Transform starHolder;
    [SerializeField] EndStarSingle[] allStars;
    [SerializeField] AudioClip newStarSound;
    [SerializeField] AudioClip oldStarSound;
    [SerializeField] AudioClip createStarSound;
    [SerializeField] GameObject gemTemplate;

    Color fullStarColor = Color.white;
    Color emptyStarColor = Color.black;



    [SerializeField] Image templateStar;


    //this needs the ref for all places it wants to use.
   
  


    IEnumerator ReceiveAnimationProcess(Transform targetTransform, float scaleTarget = 1.3f)
    {

        float timer = 0.3f;
        targetTransform.DOScale(scaleTarget, timer);
        yield return new WaitForSeconds(timer);
        targetTransform.DOScale(1, timer);

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


    //
    public IEnumerator OrderStarAnimation(Vector3 pos)
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

        StartCoroutine(ReceiveAnimationProcess(newObject.transform));
        //we check if the stage data can be
        ActiveTheFirstEmptyStar();
        Destroy(newObject);


        StageData data = LocalHandler.instance.data;

        //call fade ui.

        FadeUI newObjectFade = Instantiate(UIHandler.instance.uiFade);
        newObjectFade.transform.SetParent(transform);
        //then we need information in relation
        
       
    }


    
    GameObject GetStarObject(Vector3 pos)
    {     
        GameObject newObject = Instantiate(templateStar.gameObject, Vector3.zero, Quaternion.identity);
        newObject.SetActive(true);
        newObject.transform.SetParent(transform);
        newObject.transform.position = pos;

        newObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        newObject.transform.DOScale(1, 0.6f);


        return newObject;

    }


    int GetNextEmptyStar()
    {
        for (int i = 0; i < allStars.Length; i++)
        {
            if (allStars[i].GetColor() == emptyStarColor)
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

        for (int i = 0; i < allStars.Length; i++)
        {
            if (allStars[i].GetColor() == emptyStarColor)
            {
                allStars[i].SetColor(fullStarColor);
                allStars[i].transform.GetChild(0).gameObject.SetActive(true);

                //then we check here if its a new star.

                break;
            }
        }


        

    }
    public void MakeAllStarsEmpty()
    {
        foreach (var item in allStars)
        {
            item.SetColor(emptyStarColor);
            item.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    //instead we will directly call this

    public bool CallNewStar()
    {
        //we return if this is a new coin and should also ad gem.

        //we active the first emptystar and make the effect.
        //also a sound effect.

        int targetEmptyStar = GetNextEmptyStar();
        StageData data = LocalHandler.instance.data;

        bool isNewStar = data.stageStarGained > targetEmptyStar;

        float scalingModifier = 0;


        return isNewStar;

      
    }

    

    public IEnumerator CallStarProcess(Vector3 pos)
    {
        float timeToReach = 1;

        int targetIndex = GetNextEmptyStar();
        GameObject newObject = GetStarObject(pos);
        StageData data = LocalHandler.instance.data;
        bool isNewStar = targetIndex + 1 > data.stageStarGained;

        GameHandler.instance.soundHandler.CreateSFX(createStarSound);

        yield return new WaitForSeconds(0.5f);

        newObject.transform.DOMove(allStars[targetIndex].transform.position, timeToReach);

        yield return new WaitForSeconds(timeToReach);

        Destroy(newObject);
        ActiveTheFirstEmptyStar();
        StartCoroutine(ReceiveAnimationProcess(allStars[targetIndex].transform, 1.4f));


        if (isNewStar)
        {
            GameHandler.instance.soundHandler.CreateSFX(newStarSound);
            allStars[targetIndex].CallGem();
            //THIS IS DONE TERRIBLY BUT DOESNT MATTER NOW
            UIHandler.instance.uiEnd.rewardHolder.AddToRewardGem(5);

        }
        else
        {
            GameHandler.instance.soundHandler.CreateSFX(oldStarSound);
        }


    }

}
