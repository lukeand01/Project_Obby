using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfirmationWindowButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textString;
    [SerializeField] TextMeshProUGUI textValue;
    [SerializeField] GameObject iconGem;
    [SerializeField] GameObject iconCoin;


    public void UpdateString(string text)
    {
        textString.gameObject.SetActive(true);
        textValue.gameObject.SetActive(false);

        iconCoin.SetActive(false);
        iconGem.SetActive(false);

        textString.text = text; 

    }
    public void UpdateValueText(CurrencyType currencyType, string text)
    {
        textString.gameObject.SetActive(false);
        textValue.gameObject.SetActive(true);

        textValue.text = text;

        iconCoin.SetActive(currencyType == CurrencyType.Coin);
        iconGem.SetActive(currencyType == CurrencyType.Gem);

    }
}
