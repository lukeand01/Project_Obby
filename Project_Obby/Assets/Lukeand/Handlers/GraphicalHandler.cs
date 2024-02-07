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
