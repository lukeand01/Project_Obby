using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Store / Dance")]
public class StoreDanceData : StoreData
{
    [SerializeField] DanceData danceData;


    public override void Buy()
    {
        throw new System.NotImplementedException();
    }
}
