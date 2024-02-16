using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreHandler : MonoBehaviour
{

    //we will have all itens here.
    //we will put them into different categories.

    [SerializeField] List<StoreData> allStoreItensList = new();
    Dictionary<StoreType, List<StoreData>> allStoreItensDividedByCategoryDictionary = new();

    //divide those fellas between categories.
    
    private void Awake()
    {
        //we put every part into a proper list divided by category.
        //then in the ui we tell 


        GiveIndexToItens();
        CreateStoreCategories();

    }

    void GiveIndexToItens()
    {
        for (int i = 0; i < allStoreItensList.Count; i++)
        {
            allStoreItensList[i].SetStoreIndex(i);
        }
    }

    void CreateStoreCategories()
    {
        allStoreItensDividedByCategoryDictionary.Clear();
        allStoreItensDividedByCategoryDictionary.Add(StoreType.Power, new List<StoreData>());
        allStoreItensDividedByCategoryDictionary.Add(StoreType.Animation, new List<StoreData>());
        allStoreItensDividedByCategoryDictionary.Add(StoreType.Graphic, new List<StoreData>());

        foreach (var item in allStoreItensList)
        {
            if (item.GetAnimation() != null)
            {
                allStoreItensDividedByCategoryDictionary[StoreType.Animation].Add(item);
            }

            if(item.GetGraphic() != null)
            {
                allStoreItensDividedByCategoryDictionary[StoreType.Graphic].Add(item);
            }

            if(item.GetPower() != null)
            {
                allStoreItensDividedByCategoryDictionary[StoreType.Power].Add(item);
            }

        }


    }



}
