using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoreData : ScriptableObject
{
    //this will tell teh store button what to do.

    public string storeItemName;
    public Sprite storeItemSprite;
    public int goldPrice;


    public abstract void Buy();
    public abstract bool CanBuy();
    
}
   
