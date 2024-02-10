using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNode : MonoBehaviour
{
    public Vector2 pos;
    public Vector2Int gridLocation;

    public bool isBlocked;

    //what is the g cost
    public int g;
    public int h;
    public int f { get { return g + h; } }

    public List<MyNode> neighborsList = new List<MyNode>();
    public MyNode cameFrom;

    public void AddNeighbor(MyNode node) => neighborsList.Add(node);

    public void SetUpPos(Vector2 pos) => this.pos = pos;

}

//