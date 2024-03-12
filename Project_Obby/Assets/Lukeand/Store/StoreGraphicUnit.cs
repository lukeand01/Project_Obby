using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreGraphicUnit : StoreBaseUnit
{

    StoreGraphicData data;
    StoreUI handler;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image icon;
    [SerializeField] GameObject gemSymbol;
    [SerializeField] GameObject goldSymbol;
    [SerializeField] TextMeshProUGUI valueText;
    [SerializeField] Transform selected;
    [SerializeField] GameObject priceHolder;
    [SerializeField] TextMeshProUGUI stateText;

    public bool isAlreadyOwned {  get; private set; }
    bool isSelected;
    bool isCurrentlyUsing; //different dates will ask different questions.


   


    private void Awake()
    {
        selected.gameObject.SetActive(true);
        selected.transform.localScale = Vector3.zero;
    }

    public void SetUp(StoreGraphicData data, StoreUI handler)
    {
        this.data = data;
        this.handler = handler;

        

        UpdateUI();
    }

    public void UpdateOwnership()
    {
        isAlreadyOwned = PlayerHandler.instance.HasStoreItem(data.storeIndex);
        isCurrentlyUsing = PlayerHandler.instance.graphic.graphicIndex == (int)data.graphicType;

        priceHolder.SetActive(!isAlreadyOwned);
        stateText.gameObject.SetActive(isAlreadyOwned);

        if (isCurrentlyUsing)
        {
            stateText.text = "Using";
        }
        else
        {
            stateText.text = "Bought";
        }
    }

    void UpdateUI()
    {
        nameText.text = data.storeItemName;
        icon.sprite = data.storeItemSprite;



        gemSymbol.SetActive(data.currencyType == CurrencyType.Gem);
        goldSymbol.SetActive(data.currencyType == CurrencyType.Coin);
        valueText.text = data.storePrice.ToString();

        UpdateOwnership();


    }

    public void Select()
    {
        isSelected = true;
        float time = 0.15f;
        selected.transform.DOKill();
        selected.transform.DOScale(new Vector3(2.1f, 1.12f, 0), time);

        //StopAllCoroutines();
        //StartCoroutine(IsSelectedProcess());
    }



    public void UnSelect()
    {
        isSelected = false;
        float time = 0.15f;
        selected.transform.DOKill();
        selected.transform.DOScale(0, time);

       // StopAllCoroutines();
        //UnselectedOrder();
    }

    void ChangeGraphical()
    {
        //we give the information to the player.

        //and i need to change teh currently wearing thing.
        //so maybe i just do a wide update. just teh ui.


        PlayerHandler.instance.graphic.SetGraphicIndex(GetGraphicIndex());
        handler.UpdateAllGraphicUnits();
    }



    public override void UpdateAfterBuying()
    {
        base.UpdateAfterBuying();
        UnSelect();
        isAlreadyOwned = true;
        isCurrentlyUsing = true;

        //we need to give that skin to the player but that is done in the handler.

        UpdateUI();

    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        Debug.Log("click");

        if (isSelected)
        {
            if (isAlreadyOwned)
            {
                //then we start using this thing.

                ChangeGraphical();
            }
            else
            {
                //then we ask for confirmation to buy it.
                Debug.Log("start buy item");
                handler.StartBuyItem(data, this);
            }
        }
        else
        {
            //then we select it.
            Debug.Log("select");
            handler.SelectGraphical(this);
        }



    }


    IEnumerator IsSelectedProcess()
    {
        //keeps getting bigger and smaller.
        transform.DOKill();

        float time = 5f;

        transform.DOScale(new Vector3(0.4f, 0.7f, 0), time);
        
        yield return new WaitForSeconds(time);

        transform.DOScale(new Vector3(0.3f, 0.6f, 0), time);


        if(isSelected)
        {
            StartCoroutine(IsSelectedProcess());
        }
        else
        {

        }

    }

    void UnselectedOrder()
    {
        transform.DOKill();
        transform.DOScale(new Vector3(0.3f, 0.6f, 0), 0.3f);
    }


    public int GetGraphicIndex()
    {
        return (int)data.graphicType;
    }
}
