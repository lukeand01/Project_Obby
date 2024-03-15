using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreButton : ButtonBase
{
    [Separator("STORE")]
    [SerializeField] TextMeshProUGUI storeText;

    Color selectedColor;
    Color originalColor;

    StoreUI handler;

    [SerializeField] StoreCategoryType categoryType;

    public void SetStoreUI(StoreUI handler)
    {
        this.handler = handler; 
    }

    private void Awake()
    {
        selectedColor = Color.white;
        originalColor = storeText.color;


        
    }

    public void SelectThisCategory()
    {
        storeText.color = selectedColor;

        transform.DOScale(1.2f, 0.3f);

    }
    public void UnselectThisCategory()
    {
        storeText.color = originalColor;

        transform.DOScale(1f, 0.3f);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);


        handler.SelectThisCategory(this, categoryType);

    }

    public float GetXValueFromText() => storeText.transform.position.x;

}
