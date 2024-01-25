
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public static class SaveHandler2 
{
    //writting in json seem to be better.
    //howevr cant save in persistntpath still.

    #region GAME ESPECIFIC ORDERS
    public static void OrderToSaveData()
    {
        //we just put all information here
        SaveClass newSaveClass = new SaveClass();
        newSaveClass.playerGold = PlayerHandler.instance.gold;
        newSaveClass.playerHealth = PlayerHandler.instance.currentHealth;
        newSaveClass.playerStageProgress = PlayerHandler.instance.stageProgress;

        bool success = SaveData("0", newSaveClass, true);

        Debug.Log("save was a sucess: " + success);

    }

    public static SaveClass OrderToLoadData()
    {
        return LoadData<SaveClass>("0", true);

    }

    #endregion


    public static bool SaveData<T>(string saveName, T data, bool isEncrypted)
    {
        string path = Application.persistentDataPath + saveName;


        if (File.Exists(path))
        {
            File.Delete(path);
        }
            try
            {
                FileStream stream = File.Create(path);
                stream.Close();
                File.WriteAllText(path, JsonConvert.SerializeObject(data));
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        

    }

    public static bool HasFile(string saveName)
    {
        string path = Application.persistentDataPath + saveName;
        return File.Exists(path);
    }
    
    public static t LoadData<t>(string saveName, bool isEncrypted)
    {
        string path = Application.persistentDataPath + saveName;

        if(!File.Exists(path))
        {
            throw new FileNotFoundException("this path does not exist " + path);
        }

        try
        {
            t data = JsonConvert.DeserializeObject<t>(File.ReadAllText(path));
            return data;
        }
        catch(Exception e)
        {
            Debug.LogError("Failed to load data because of " + e.Message);
            throw e;
        }

    }

    public static void DeleteData(string saveName)
    {
        Debug.Log("delete save fil");
        string path = Application.persistentDataPath + saveName;
        File.Delete(path);
    }
}
