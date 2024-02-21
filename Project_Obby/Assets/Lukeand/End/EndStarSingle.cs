using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndStarSingle : MonoBehaviour
{
    [SerializeField] GameObject gem;
    [SerializeField] Image starImage;
    Vector3 gemOriginalPos;

    private void Awake()
    {      
        gem.SetActive(false);
        gemOriginalPos = gem.transform.localPosition;
    }

    public void CallGem()
    {
        Debug.Log("call gem");
        //just make it go up.
        gem.SetActive(true);
        gem.transform.DOLocalMove(gemOriginalPos + new Vector3(0, 150, 0), 1);
        StartCoroutine(GemProcess());
    }

    IEnumerator GemProcess()
    {
        yield return new WaitForSeconds(1);

        //then we start fading to disappear.

        gem.SetActive(false);
        gem.transform.localPosition = gemOriginalPos;
    }


    public void SetColor(Color color)
    {
        starImage.color = color;
    }

    public Color GetColor() => starImage.color;

}
