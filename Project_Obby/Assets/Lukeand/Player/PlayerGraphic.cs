using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraphic : MonoBehaviour
{
    //do i load all the ref here?
    [SerializeField] RuntimeAnimatorController fallAnimation;
    [SerializeField] GameObject playerBodyHolder;
    GameObject graphic;
    Animator graphicAnimator;
    RuntimeAnimatorController currentVictoryAnimation;

    public int graphicIndex;
    public int animationIndex;

    private void Start()
    {
        ChangeGraphic();
        ChangeAnimation();
    }

    void ChangeGraphic()
    {
        if(graphic != null)
        {
            Debug.Log("there was a graphic here before");
            Destroy(graphic);
            graphic = null; 
            graphicAnimator = null;
        }



        GameObject template =  GameHandler.instance.graphicalHandler.GetNewGraphic(graphicIndex);

        if(template != null )
        {
            Debug.Log("found template");
        }
        else
        {
            Debug.LogError("DID NOT FIND GRAPHIC FROM INDEX " + graphicIndex);
        }


        GameObject newObject = Instantiate(template, Vector3.zero, Quaternion.identity);

        Animator newAnimator = newObject.GetComponent<Animator>();

        if(newAnimator != null )
        {
            newAnimator.avatar = null;
        }
        else
        {
            Debug.Log("no new animator");
        }

        newObject.transform.SetParent(playerBodyHolder.transform);
        newObject.transform.localPosition = Vector3.zero;

        graphic = newObject;
        graphicAnimator = newAnimator;
    }
    void ChangeAnimation()
    {
        RuntimeAnimatorController newAnimation = GameHandler.instance.graphicalHandler.GetNewAnimation(animationIndex);

        if (newAnimation != null)
        {
            currentVictoryAnimation = newAnimation;
        }
        else
        {
            Debug.LogError("FOUND NOTHING FROM ANIMATION INDEX " + animationIndex);
        }

    }

    [ContextMenu("Debug Play Victory Animation")]
    public void PlayVictoryAnimation()
    {
        //this alone should start the animation

        if (!HasGraphicAnimation())
        {
            Debug.Log("tried to start but it failed");
            return;
        }

        graphicAnimator.runtimeAnimatorController = currentVictoryAnimation;
    }

    [ContextMenu("Debug Stop Animation")]
    public void StopAnimation()
    {
        if (!HasGraphicAnimation())
        {
            Debug.Log("tried to stop but it failed");
            return;
        }
        graphicAnimator.runtimeAnimatorController = null;
    }

    public void PlayFallAnimation()
    {
        if (!HasGraphicAnimation())
        {
            Debug.Log("tried to stop but it failed");
            return;
        }
        graphicAnimator.runtimeAnimatorController = fallAnimation;
    }


    bool HasGraphicAnimation()
    {
        return graphicAnimator != null;
    }



    public void SetGraphicIndex(int newValue)
    {
        graphicIndex = newValue;
        ChangeGraphic();
    }
    public void SetAnimationIndex(int newValue)
    {
        animationIndex = newValue;
        ChangeAnimation();
    }
}
