using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyClass 
{


    public KeyClass()
    {
        SetUpKeys2();
    }

    

    Dictionary<KeyType, KeyCode> keyDictionary = new Dictionary<KeyType, KeyCode>();



    public KeyCode GetKey(KeyType inputType)
    {
        return keyDictionary[inputType];
    }

    void ChangeKey(KeyType keyType, KeyCode key)
    {

    }

    

    void SetUpKeys2()
    {
       
    }


}

public enum KeyType
{   
    

}