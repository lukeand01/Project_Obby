using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPoint : MonoBehaviour
{
    bool isUsed;
    int index;

    [SerializeField] Transform orientation;
    MeshRenderer rend;
    BoxCollider myCollider;

    private void Awake()
    {
        rend = gameObject.GetComponent<MeshRenderer>();
        myCollider = gameObject.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isUsed)
        {
            return;
        }
        if (other.transform.tag != "Player")
        {
            return;
        }

        if (PlayerHandler.instance.isDead)
        {
            Debug.Log("is dead could not interact");
            return;
        }



        Used();
    }

   

    public void Used()
    {
        MakeItUsed();
        PlayerHandler.instance.AssignNewSpawnPoint(this, index);
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public void Unselect()
    {

    }

    public Vector3 GetSpawnPos()
    {
        return transform.position + orientation.forward;
    }

    public void MakeItUsed()
    {
        isUsed = true;

        if(rend != null)
        {
            rend.enabled = false;
        }
       
    }
}
