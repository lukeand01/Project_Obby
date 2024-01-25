using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    public string stageName;
    public int stageId;
    public StageTimeClass stageLimitTimer;
    public StageTimeClass stageCompletedTimer {  get; private set; }

}

[System.Serializable]
public class StageTimeClass
{
    public int minutes;
    public int seconds;

    public StageTimeClass(int minutes, int seconds)
    {
        this.minutes = minutes;
        this.seconds = seconds;
    }


    public bool HasSomething()
    {
        return minutes != 0 && seconds != 0;
    }
}