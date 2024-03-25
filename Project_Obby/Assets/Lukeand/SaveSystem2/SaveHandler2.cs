
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
        PlayerHandler player = PlayerHandler.instance;


        //ECONOMY
        newSaveClass.playerCoin = player.coins;
        newSaveClass.playerGem = player.gems;

        //STAGE PROGRESS
        newSaveClass.playerStageProgress = player.stageProgress;

        //ANIMATION AND GRAPHICS
        newSaveClass.playerCurrentAnimationIndex = player.graphic.animationIndex;
        newSaveClass.playerCurrentGraphicIndex = player.graphic.graphicIndex;

        //STORE 
        newSaveClass.playerItemsList = PlayerHandler.instance.storeItensOwnedList;

        //INDIVIDUAL STAGES
        newSaveClass.stageList = GameHandler.instance.stageHandler.GetSaveClassStageList();


        bool success = SaveData("0", newSaveClass, true);


    }

    public static void OrderToLoadData()
    {
        //i think should give all information right away with this one here.
        PlayerHandler player = PlayerHandler.instance;
        StageHandler stage = GameHandler.instance.stageHandler;



        if (OrderHasFile())
        {          

            //if you have file but you have nothing then we delete this data and sign it as corrupted and we give a new one.

            SaveClass saveClass = LoadData<SaveClass>("0", true);


            if(saveClass.playerItemsList.Count <= 0)
            {
                //this should never happen so what we do is delete the file and call it again.

                player.UseEmptyData();
                stage.ResetAllStages();
                return;
            }


            MainMenuUI.instance.DebugConsoleText("save: " + saveClass.playerItemsList.Count.ToString());

            //CURRENCY
            player.SetCoin(saveClass.playerCoin);
            player.SetGem(saveClass.playerGem);

            //STAGE PROGRESSIOn
            player.SetStageProgress(saveClass.playerStageProgress);

            //STORE ITEMS
            player.SetNewItemOwnedList(saveClass.playerItemsList);


            //GRAPHIC AND ANIMATION
            player.graphic.SetGraphicIndex(saveClass.playerCurrentGraphicIndex);
            player.graphic.SetAnimationIndex(saveClass.playerCurrentAnimationIndex);

            //INDIVIDUAL STAGES



            stage.ReceiveStageDataList(saveClass.stageList);

        }
        else
        {
            //then we give new data. start something new;
            player.UseEmptyData();
            stage.ResetAllStages();
        }


        //the problem must be here;
        

    }

    public static bool OrderHasFile()
    {
        return HasFile("0");
    }

    public static void OrderDeleteFile()
    {
        DeleteData("0");
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

        string path = Application.persistentDataPath + saveName;
        File.Delete(path);
    }
}
