using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGate : MonoBehaviour
{
    [SerializeField] int permissionQuantityRequire = 1;
    int currentPermission;


    public void ReceivePermission()
    {
        currentPermission++;

    }

    void OpenGate()
    {

    }
    void CloseGate()
    {

    }


}
