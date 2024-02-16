using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicalHandler : MonoBehaviour
{
    [SerializeField] List<GameObject> graphicList = new();
    [SerializeField] List<RuntimeAnimatorController> animationList = new();


    public GameObject GetNewGraphic(int index)
    {
        return graphicList[index];
    }
    public RuntimeAnimatorController GetNewAnimation(int index)
    {
        return animationList[index];
    }

}


public enum GraphicType
{
    Assassin = 0,
    Bandit = 1,
    Boy = 2,
    Bussiness_Woman = 3,
    Girl = 4,
    Grandfater = 5,
    Hero = 6,
    Knight = 7,
    Mage = 8,
    Ninja = 9,
    Pirate = 10,
    Punk = 11,
    Samurai = 12,
    Santa = 13,
    Skeleton = 14,
    Sniper = 15,
    Soldier = 16,
    Supergirl = 17,
    Viking = 18,
    Zombie = 19
}

public enum AnimationType
{
    Chicken_Dance = 0,
    Gangnam_Style = 1,
    House_Dancing = 2,
    Northern_Soul_Spin_Combo = 3,
    Robot_Hip_Hop = 4,
    Slide_Hip_Hop = 5,
    Tut_Hip_Hop = 6,
    Wave_Hip_Hop_Dance = 7
}