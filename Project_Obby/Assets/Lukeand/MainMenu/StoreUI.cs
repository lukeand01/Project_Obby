using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{

    //when nothing is select the button wont show.
    //when the currently wearing is selected then it wont show
    //


    [SerializeField] GameObject focus;

    [SerializeField] StoreButton[] buttons;

    private Dictionary<StoreCategoryType, GameObject> categoryHolderDictionary = new();
     List<GameObject> allHolders = new();

    [SerializeField] ConfirmationWindowUI confirmationWindow;
    [SerializeField] Image emptyImage; // we always put 4 images in them.

    [Separator("CHARACTER SHADOW AND PREVIEW")]
    [SerializeField] GameObject shadowPreview;
    [SerializeField] Transform previewHolder;
    Animator currentModelAnimator;
    GameObject currentModel;

    


    [Separator("GRAPHIC")] 
    [SerializeField] GameObject graphicHolder;
    [SerializeField] Transform graphicContainer;
    [SerializeField] StoreGraphicUnit graphicUnitTemplate;

    [Separator("ANIMATION")]
    [SerializeField] GameObject animationHolder;
    [SerializeField] Transform animationContainer;
    [SerializeField] StoreAnimationUnit animatationUnitTemplate;

    [Separator("POWER")]
    [SerializeField] GameObject powerHolder;
    [SerializeField] Transform powerContainer;
    [SerializeField] StorePowerUnit powerUnitTemplate; 

    [Separator("COIN")]
    [SerializeField] GameObject coinHolder;

    [Separator("GEM")]
    [SerializeField] GameObject gemHolder;

    bool cannotSelect;

    private void Awake()
    {

        SetUp();
        
    }

    private void Start()
    {
       

        Invoke(nameof(OrderToGenerateAllUnits), 0.1f);
    }

    void OrderToGenerateAllUnits()
    {
        GenerateGraphicalUnits();
        GenerateAnimationlUnits();
        GeneratePowerUnits();

    }

    int categoryCurrentIndex;

    public void OpenUI(int index)
    {
        //we always select the first cateogry.
      if(index == 0)  SelectThisCategory(buttons[0], StoreCategoryType.Graphical);
      if(index == 1) SelectThisCategory(buttons[1], StoreCategoryType.Animation);
      if(index == 2) SelectThisCategory(buttons[2], StoreCategoryType.Power);
      if (index == 3) SelectThisCategory(buttons[3], StoreCategoryType.Coin);
      if (index == 4) SelectThisCategory(buttons[4], StoreCategoryType.Gem);


      categoryCurrentIndex = index;
    }

    //need to first do this. gems and coins are not dinamicallty set they are always the same thing.

    #region CORE FUNCTIONS FOR SET UP AND BUYING ITEMS

    void SetUp()
    {
        //we get the lists


        allHolders = new List<GameObject>
        {
            graphicHolder,
            animationHolder,
            powerHolder,
            gemHolder,
            coinHolder
        };

        categoryHolderDictionary.Add(StoreCategoryType.Graphical, graphicHolder);
        categoryHolderDictionary.Add(StoreCategoryType.Animation, animationHolder);
        categoryHolderDictionary.Add(StoreCategoryType.Power, powerHolder);
        categoryHolderDictionary.Add(StoreCategoryType.Gem, gemHolder);
        categoryHolderDictionary.Add(StoreCategoryType.Coin, coinHolder);


        //and now we get teach list and put in the right container. the only who dont do it are the gem and coins containers.
        foreach (var button in buttons)
        {
            button.SetStoreUI(this);
        }

    }

    //either this or we get the new one.

    //we also need to inform the curernt using animation or graphic to change.
    //

    public void StartBuyItem(StoreData data, StoreBaseUnit storeUnit)
    {
        confirmationWindow.eventConfirm = delegate { };
        confirmationWindow.eventCancel = delegate { };

        if (!data.CanBuy())
        {
            //then we show something else.
            //i would like to know whats lacking.


            string text = "";

            if(data.currencyType == CurrencyType.Coin)
            {
                text = "You dont have enough Coins!";
            }

            if(data.currencyType == CurrencyType.Gem)
            {
                text = "You dont have enough Gems!";
            }


            confirmationWindow.StartConfirmationWindow("Purchase", text);

            confirmationWindow.eventConfirm += CloseBuyItem;

            if (data.currencyType == CurrencyType.Coin)
            {
                confirmationWindow.eventConfirm += TakeToCoin;
                confirmationWindow.ChangeConfirmTextString("Go to the Gold Section");
            }

            if (data.currencyType == CurrencyType.Gem)
            {
                confirmationWindow.eventConfirm += TakeToGem;
                confirmationWindow.ChangeConfirmTextString("Go to the Gem Section");
            }


            confirmationWindow.eventCancel += CloseBuyItem;
            return;
        }






        cannotSelect = true;

        //also we need to update the itens to put the newly acquired stuff first in the list.


        confirmationWindow.StartConfirmationWindow("Purchase", data.storePurchaseDescription);
        confirmationWindow.ChangeConfirmTextValue(data.currencyType, data.storePrice.ToString());

        confirmationWindow.eventConfirm += data.Buy;
        confirmationWindow.eventConfirm += storeUnit.UpdateAfterBuying;
        confirmationWindow.eventConfirm += CloseBuyItem;

        confirmationWindow.eventCancel += CloseBuyItem;


        //also if you do buy something

    }

    void UpdateNewlyAcquiredItemPositionInRightOrder()
    {
        //we put it first in the list for now.



    }


    void TakeToCoin()
    {
        //then we move the character to it.
        SelectThisCategory(buttons[2], StoreCategoryType.Coin);
    }
    void TakeToGem()
    {
        SelectThisCategory(buttons[3], StoreCategoryType.Gem);
    }


    void CloseBuyItem()
    {
        //we close the confirmation and remove the events.
        cannotSelect = false;

        confirmationWindow.CloseConfirmationWindow();

        confirmationWindow.eventConfirm = delegate { };
        confirmationWindow.eventCancel = delegate { };
    }

    public void ResetStoreUI()
    {
        //close confirmation
        CloseBuyItem();
        DestroyPreview();
    }

    #endregion

    #region CATEGORY
    StoreButton currentSelect;
    public void SelectThisCategory(StoreButton target, StoreCategoryType store)
    {
        //we play animation for it.

        if(currentSelect != null)
        {
            currentSelect.UnselectThisCategory();
        }

        currentSelect = target;
        currentSelect.SelectThisCategory();

        OrderFocusMove(target.GetXValueFromText());


        //and now we open the bastard.

        CloseAllHolders();
        CloseBuyItem();

        bool shouldPreviewBeVisible = store == StoreCategoryType.Graphical || store == StoreCategoryType.Animation;

        ControlPreview(shouldPreviewBeVisible);

        switch (store)
        {
            case StoreCategoryType.Graphical:
                OpenGraphical(); 
                break;
            case StoreCategoryType.Animation:
                OpenAnimation();
                break;
            case StoreCategoryType.Power:
                OpenPower();
                break;
            case StoreCategoryType.Coin:
                OpenCoin();
                break;
            case StoreCategoryType.Gem:
                OpenGem();
                break;
        }
   

    }

    void OrderFocusMove(float xValue)
    {
        focus.transform.DOKill();
        focus.transform.DOMoveX(xValue, 0.5f);

    }

    void CloseAllHolders()
    {
        foreach (var item in allHolders)
        {
            item.SetActive(false);
        }
    }

    #endregion

    #region PREVIEW
    //

    void ControlPreview(bool shouldBeVisible)
    {
        shadowPreview.gameObject.SetActive(shouldBeVisible);

        //everytime this called we will load the base for the preview.

        DestroyPreview();

        if (!shouldBeVisible)
        {
            //we also destroy the model.          
            return;
        }

        //graphic and animation
        int graphicIndexCurrent = PlayerHandler.instance.graphic.graphicIndex;
        int animationIndexCurrent = PlayerHandler.instance.graphic.animationIndex;

        ChangePreviewGraphic(graphicIndexCurrent);
        ChangePreviewAnimation(animationIndexCurrent);

       
    }

    void ChangePreviewAnimation(int animationIndex)
    {
        //we get an index.
        //we get the current body and we change stuff.
        if(currentModel == null)
        {
            Debug.Log("there is something wrong");
            return;
        }

        RuntimeAnimatorController animation = GameHandler.instance.graphicalHandler.GetNewAnimation(animationIndex);
        currentModelAnimator.runtimeAnimatorController = animation;

    }
    void ChangePreviewGraphic(int graphicIndex)
    {
        //we first get the index of the player. its current skin.
        //we load a new body from the gamehandler.
        //then we will load animation as well.

        RuntimeAnimatorController currentAnimation = null;


        if (currentModel != null)
        {
            if(currentModelAnimator == null)
            {
                Debug.Log("that should never be the case");
                return;
            }

            currentAnimation = currentModelAnimator.runtimeAnimatorController;
            Destroy(currentModel);
            currentModel = null;
            currentModelAnimator = null;
        }


        GameObject model = GameHandler.instance.graphicalHandler.GetCopyNewGraphic(graphicIndex);
        model.transform.localScale = new Vector3(75, 75, 75);
        model.transform.localRotation = Quaternion.Euler(0, 180, 0);


        model.transform.SetParent(previewHolder);
        model.transform.localPosition = Vector3.zero;

        //model.transform.position = previewHolder.transform.position;
        currentModel = model;
        currentModelAnimator = model.GetComponent<Animator>();
        currentModelAnimator.runtimeAnimatorController = currentAnimation;
    }

   

    void DestroyPreview()
    {
        if(currentModel != null)
        {
            Destroy(currentModel);
            currentModel = null;
            currentModelAnimator = null;
        }
    }

    #endregion

    #region GRAPHICAL
    StoreGraphicUnit currentGraphic;
    List<StoreGraphicUnit> graphicUnitList = new();
    void GenerateGraphicalUnits()
    {


        List<StoreData> graphicalList = GameHandler.instance.storeHandler.GetStoreItemListByStoreByCategory(StoreType.Graphic);
        List<StoreGraphicUnit> organizationList = new();
        graphicUnitList.Clear();

        foreach (var item in graphicalList)
        {
            StoreGraphicData graphicData = item.GetGraphic();

            if(graphicData == null)
            {
                Debug.LogError("this is not graphic " + item.name);
                return;
            }

            StoreGraphicUnit newObject = Instantiate(graphicUnitTemplate, new Vector3(0, 0, 0), Quaternion.identity);
            newObject.SetUp(item.GetGraphic(), this);
            newObject.transform.SetParent(graphicContainer);

            if (newObject.isAlreadyOwned)
            {
                organizationList.Add(newObject);
            }

           

            graphicUnitList.Add(newObject);
        }


        for (int i = 0; i < organizationList.Count; i++)
        {
            organizationList[i].transform.SetSiblingIndex(i);
        }

        //we need to get the list and a way to tell who is owned.


        //each item will ask the player if he owns him.

        CreateEmptyImages(graphicContainer, 4);
    }

    void OpenGraphical()
    {

        graphicHolder.SetActive(true);  
        RemoveCurrentGraphical();
    }

    public void SelectGraphical(StoreGraphicUnit newGraphic)
    {
        //make it appear in the preview.

        if (cannotSelect) return;

        //when you select you get information about it.

        ChangePreviewGraphic(newGraphic.GetGraphicIndex());


        if(currentGraphic != null)
        {
            currentGraphic.UnSelect();
        }

        currentGraphic = newGraphic;
        currentGraphic.Select();


    } 

    public void UpdateAllGraphicUnits()
    {
        foreach (var item in graphicUnitList)
        {
            item.UpdateOwnership();
        }
    }
    
    void RemoveCurrentGraphical()
    {
        if(currentGraphic != null)
        {
            currentGraphic.UnSelect();
            currentGraphic = null;
        }
    }




    #endregion

    #region ANIMATION
    StoreAnimationUnit currentAnimation;
    List<StoreAnimationUnit> animationUnitList = new();

    void GenerateAnimationlUnits()
    {
        List<StoreData> animationList = GameHandler.instance.storeHandler.GetStoreItemListByStoreByCategory(StoreType.Animation);
        animationUnitList.Clear();
        List<StoreAnimationUnit> organizationList = new();


        foreach (var item in animationList)
        {
            StoreAnimationData animationData = item.GetAnimation();

            if (animationData == null)
            {
                Debug.LogError("this is not animation " + item.name);
                return;
            }

            StoreAnimationUnit newObject = Instantiate(animatationUnitTemplate, new Vector3(0,0,0), Quaternion.identity);
            newObject.SetUp(item.GetAnimation(), this);
            newObject.transform.SetParent(animationContainer);


            if (newObject.isAlreadyOwned)
            {
                organizationList.Add(newObject);
            }
            
            

            animationUnitList.Add(newObject);
        }

        //each item will ask the player if he owns him.

        for (int i = 0; i < organizationList.Count; i++)
        {
            organizationList[i].transform.SetSiblingIndex(i);
        }


        CreateEmptyImages(animationContainer, 4);
    }

    void OpenAnimation()
    {
        animationHolder.SetActive(true);
        RemoveCurrentAnimation();
    }


    public void SelectAnimation(StoreAnimationUnit newAnimation)
    {
        if (cannotSelect) return;

        //when you select you get information about it.

        ChangePreviewAnimation(newAnimation.GetAnimationIndex());

        if (currentAnimation != null)
        {
            currentAnimation.UnSelect();
        }

        currentAnimation = newAnimation;
        currentAnimation.Select();
    }

    public void UpdateAllAnimation()
    {
        foreach (var item in animationUnitList)
        {
            item.UpdateOwnership();
        }
    }

    void RemoveCurrentAnimation()
    {
        if(currentAnimation != null)
        {
            currentAnimation.UnSelect();
        }
    }

    public void UpdateAllAnimationUnit()
    {
        foreach (var item in animationUnitList)
        {
            item.UpdateOwnership();
        }
    }

    #endregion

    #region POWER
    StorePowerUnit currentPower;
    [Separator("POWER")]
    [SerializeField] TextMeshProUGUI powerSelectText;
    //buy this fella.

    void GeneratePowerUnits()
    {
        List<StoreData> powerList = GameHandler.instance.storeHandler.GetStoreItemListByStoreByCategory(StoreType.Power);
        List<StorePowerUnit> organizationList = new();



        foreach (var item in powerList)
        {
            StorePowerData powerData = item.GetPower();

            if (powerData == null)
            {
                Debug.LogError("this is not power " + item.name);
                return;
            }

            StorePowerUnit newObject = Instantiate(powerUnitTemplate);
            newObject.SetUp(item.GetPower(), this);
            newObject.transform.SetParent(powerContainer);

            if(newObject.isAlreadyOwned)
            {
                organizationList.Add(newObject);
            }
        }

        for (int i = 0; i < organizationList.Count; i++)
        {
            organizationList[i].transform.SetSiblingIndex(i);
        }

        CreateEmptyImages(animationContainer, 4);
    }

    public void SelectPower(StorePowerUnit newPower)
    {
        if (cannotSelect) return;

        //when you select you get information about it.
        //we do nothing for preview at the moment.

        powerSelectText.text = newPower.GetDescription();

        if (currentPower != null)
        {
            currentPower.UnSelect();
        }

        currentPower = newPower;
        currentPower.Select();
    }

    

    void OpenPower()
    {
        powerHolder.SetActive(true);
        powerSelectText.text = "";
        RemoveCurrentPower();
    }

    void RemoveCurrentPower()
    {
        if (currentPower != null)
        {
            currentPower.UnSelect();
        }
    }


    #endregion

    #region COIN

    StoreCurrencyUnit currentCoin;



    void OpenCoin()
    {
        coinHolder.SetActive(true);
        RemoveCurrentCoin();
    }

    void RemoveCurrentCoin()
    {
        if (currentCoin != null)
        {
            currentCoin.UnSelected();
        }
    }
    

    #endregion

    #region GEM
    StoreCurrencyUnit currentGem;

    void OpenGem()
    {
        gemHolder.SetActive(true);
        RemoveCurrentGem();
    }

    void RemoveCurrentGem()
    {
        if (currentGem != null)
        {
            currentGem.UnSelected();
        }
    }


    #endregion


    public void CallStoreButton()
    {
        //it does whatever is required. can stilll double click

        if (categoryCurrentIndex == 0)
        {
            Debug.Log("you used action for graphical");

            //now we check if we have someone selected.

        }
        if (categoryCurrentIndex == 1)
        {

        }
        if (categoryCurrentIndex == 2)
        {

        }



    }

    void CreateEmptyImages(Transform container, int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            Image newObject = Instantiate(emptyImage, Vector3.zero, Quaternion.identity);
            newObject.transform.SetParent(container);
        }
    }
}

public enum StoreCategoryType
{
    Graphical,
    Animation, 
    Power,
    Coin,
    Gem
}



//FIRST
//skin are able to spawn from the store itens in the gamehandler.
//when tehre are no itens selected we show the player�s current skin dancingn to the player�s currentdance
//when you select an item it shows the new skin with the current dance.