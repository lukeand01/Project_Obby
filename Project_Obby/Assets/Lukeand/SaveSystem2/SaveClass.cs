using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SaveClass
{
    //this will refere to all data to be saved.

    public int playerStageProgress;
    public int playerHealthTotal;
    public int playerGold;
    public int playerCurrentGraphicIndex;
    public int playerCurrentAnimationIndex;
    public List<int> playerItemsList; //everyitem that can be bough is put into a list. (Skin, permapower, pets) 

    public DateTime dailyRewardLastTime;
    public int dailyRewardIndex;

}
