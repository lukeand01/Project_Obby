using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPower : ButtonBase
{
    //when this is pressed we spend the money direcctly.
    //maybe give the opportunity for watchign an ad?
    //maybe that is in the pause

    public PowerData power {  get; private set; }
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] Image selected;

    public override void OnPointerClick(PointerEventData eventData)
    {
        //if has that power already;
        PlayerHandler handler = PlayerHandler.instance;



        if(handler.HasPower(power))
        {
            Debug.Log("has power already");
            return;
        }

        power.AddPower();

        if(!handler.HasEnoughGold(power.temporaryPowerPrice))
        {
            Debug.Log("not enough money");
            return;
        }

        base.OnPointerClick(eventData);
    }



    public void SetUpPower(PowerData power)
    {
        this.power = power;
        UpdateUI();
    }

    void UpdateUI()
    {
        //portrait.sprite = power.powerSprite;
        nameText.text = power.powerName;
        priceText.text = power.temporaryPowerPrice.ToString();
    }

    public void Select()
    {
        selected.gameObject.SetActive(true);    
    }
    public void Unselect()
    {
        selected.gameObject.SetActive(false);
    }

}
