using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    GameObject holder;
    [SerializeField] Image rewardIcon;
    [SerializeField] TextMeshProUGUI rewardText;



    public void StartRewardUI(RewardData data)
    {
        StopAllCoroutines();
        StartCoroutine(OpenProcess());
    }

    public void ForceCloseUI()
    {
        holder.SetActive(false);
    }

    public void CloseRewardUI()
    {
        StopAllCoroutines();
        StartCoroutine(CloseProcess());
    }

    IEnumerator OpenProcess()
    {
        

        yield return null;  
    }

    IEnumerator CloseProcess()
    {
        yield break;
    }

}
