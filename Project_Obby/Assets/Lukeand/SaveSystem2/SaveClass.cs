using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SaveClass
{
    //this will refere to all data to be saved.

    public int playerStageProgress;
    public int playerCoin;
    public int playerGem;

    public int playerCurrentGraphicIndex;
    public int playerCurrentAnimationIndex;


    public List<int> playerItemsList; //everyitem that can be bough is put into a list. (Skin, permapower, pets) 
    public List<SaveClassStage> stageList;


    public DateTime dailyRewardLastTime;
    public int dailyRewardIndex;
}

public struct SaveClassStage
{
    public SaveClassStage(int stageIndex, int stageStarsQuantity, int bestTimerMinute, int bestTimerSecond)
    {
        this.stageIndex = stageIndex;
        this.stageStarsQuantity = stageStarsQuantity;
        this.bestTimerMinute = bestTimerMinute;
        this.bestTimerSecond = bestTimerSecond; 
    }

    public int stageIndex;
    public int stageStarsQuantity;
    public int bestTimerMinute;
    public int bestTimerSecond;

}