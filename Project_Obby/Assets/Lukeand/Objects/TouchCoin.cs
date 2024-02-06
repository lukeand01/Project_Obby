using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCoin : MonoBehaviour
{

    //what kind of effect would be nice for getting a coin?s

    public int index {  get; private set; }
    public void SetIndex(int index)
    {
        this.index = index;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;

        //PlayerHandler.instance.ChangeGold(1);
        LocalHandler.instance.AddLocalCoin(1);


        Destroy(gameObject); 
    }




}
