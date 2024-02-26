using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class StoreUI : MonoBehaviour
{

    [SerializeField] GameObject focus;

    [SerializeField] StoreButton[] buttons;

    private Dictionary<StoreCategoryType, GameObject> categoryHolderDictionary = new();
     List<GameObject> allHolders = new();

    [SerializeField] ConfirmationWindowUI confirmationWindow;

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
    [SerializeField] GameObject powerContainer;
    

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
       GenerateGraphicalUnits();
        GenerateAnimationlUnits();
    }

    public void OpenUI()
    {
        //we always select the first cateogry.
        SelectThisCategory(buttons[0], StoreCategoryType.Graphical);

        
    }

    //need to first do this. gems and gold are not dinamicallty set they are always the same thing.

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


        //and now we get teach list and put in the right container. the only who dont do it are the gem and gold containers.
        foreach (var button in buttons)
        {
            button.SetStoreUI(this);
        }

    }

    public void StartBuyItem(StoreData data, StoreBaseUnit storeUnit)
    {
        

        //

        if (!data.CanBuy())
        {
            //then we show something else.
            //i would like to know whats lacking.

            string text = "";

            if(data.currencyType == CurrencyType.Gold)
            {
                text = "You dont have enough Coins!";
            }

            if(data.currencyType == CurrencyType.Gem)
            {
                text = "You dont have enough Gems!";
            }


            confirmationWindow.StartConfirmationWindow(text);

            confirmationWindow.eventConfirm += CloseBuyItem;

            if (data.currencyType == CurrencyType.Gold)
            {
                confirmationWindow.eventConfirm += TakeToCoin;
                confirmationWindow.ChangeConfirmText("Go to the Gold Section");
            }

            if (data.currencyType == CurrencyType.Gem)
            {
                confirmationWindow.eventConfirm += TakeToGem;
                confirmationWindow.ChangeConfirmText("Go to the Gem Section");
            }


            confirmationWindow.eventCancel += CloseBuyItem;
            return;
        }

        confirmationWindow.StartConfirmationWindow(data.storePurchaseDescription);

        cannotSelect = true;

        confirmationWindow.eventConfirm += data.Buy;
        confirmationWindow.eventConfirm += storeUnit.UpdateAfterBuying;
        confirmationWindow.eventConfirm += CloseBuyItem;

        confirmationWindow.eventCancel += CloseBuyItem;

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
        graphicUnitList.Clear();

        foreach (var item in graphicalList)
        {
            StoreGraphicData graphicData = item.GetGraphic();

            if(graphicData == null)
            {
                Debug.LogError("this is not graphic " + item.name);
                return;
            }

            StoreGraphicUnit newObject = Instantiate(graphicUnitTemplate);
            newObject.SetUp(item.GetGraphic(), this);
            newObject.transform.SetParent(graphicContainer);

            graphicUnitList.Add(newObject);
        }

        //each item will ask the player if he owns him.

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

        foreach (var item in animationList)
        {
            StoreAnimationData animationData = item.GetAnimation();

            if (animationData == null)
            {
                Debug.LogError("this is not animation " + item.name);
                return;
            }

            StoreAnimationUnit newObject = Instantiate(animatationUnitTemplate);
            newObject.SetUp(item.GetAnimation(), this);
            newObject.transform.SetParent(graphicContainer);

            animationUnitList.Add(newObject);
        }

        //each item will ask the player if he owns him.

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

    void RemoveCurrentAnimation()
    {
        if(currentAnimation != null)
        {
            currentAnimation.UnSelect();
        }
    }
    #endregion

    #region POWER
    StorePowerUnit currentPower;

    void OpenPower()
    {
        powerHolder.SetActive(true);
        RemoveCurrentPower();
    }

    void RemoveCurrentPower()
    {
        if (currentPower != null)
        {
            currentPower.UnSelected();
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