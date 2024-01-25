using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    //bar that handles fill.
    [SerializeField] Image fillImage;
    [SerializeField] TextMeshProUGUI text;
    //increase in a dir.




    public void HandleFill(float current, float total)
    {
        fillImage.fillAmount = current / total;
        text.text = current.ToString("F1");
    }

    public void IncreaseFill(Vector3 dir, float quantity)
    {
        fillImage.transform.localScale = dir * quantity;
    }

    public void IncreaseScale(Vector3 increase)
    {
        transform.localScale += increase;
        //we also move a bit up.
    }
    public void MoveItUp(Vector3 increase)
    {
        transform.position += increase;
    }

}
