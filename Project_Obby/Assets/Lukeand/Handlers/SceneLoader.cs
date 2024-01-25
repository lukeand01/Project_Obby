using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] int currentScene;
    [SerializeField] Image blackScreen;



    #region FUNCTIONS
    public void ChangeScene(StageData data)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeSceneProcess(data));
    }

    public void ChangeToMainMenu()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeToMenuProcess());
    }

    public void ResetScene(StageData data)
    {
        //then we reload the currentscene.       
        StopAllCoroutines();
        StartCoroutine(ResetSceneProcess(data));      
    }




    #endregion


    //we save the information when we load stuff. always in the end irhgt before allowing movement.

    #region MAIN PROCESSES
    IEnumerator ChangeToMenuProcess()
    {

        PlayerHandler.instance.controller.blockClass.AddBlock("MainMenu", BlockClass.BlockType.Complete);

        yield return StartCoroutine(LowerCurtainsProcess());

        //reload the main menu.
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);

        yield return new WaitUntil(() => loadAsync.isDone);

        AsyncOperation unloadAsync = SceneManager.UnloadSceneAsync(currentScene, UnloadSceneOptions.None);

        yield return new WaitUntil(() => unloadAsync.isDone);

        yield return new WaitUntil(() => GameHandler.instance != null && UIHandler.instance != null && PlayerHandler.instance != null);

        UIHandler.instance.ControlHolder(false);
        currentScene = 0;


        yield return StartCoroutine(RaiseCurtainsProcess());

        
    }

    
    IEnumerator ChangeSceneProcess(StageData data)
    {

        PlayerHandler.instance.controller.blockClass.AddBlock("ChangeScene", BlockClass.BlockType.Complete);
        yield return StartCoroutine(LowerCurtainsProcess());
       
        yield return StartCoroutine(LoadAnotherSceneProcess(data));

        UIHandler.instance.ControlHolder(true);
        currentScene = data.stageId;
        //so here before we raise the curtains we always get the player position fixed.
        //we now tell teh player what it should do.
        LocalHandler.instance.StartLocalHandler(data);
        PlayerHandler.instance.ResetScenePlayer();

        yield return StartCoroutine(PlayerHandler.instance.FixPlayerPositionProcess());

        PlayerHandler.instance.ChangeProgress(); //this makes that the currentscene

        yield return StartCoroutine(RaiseCurtainsProcess());

        PlayerHandler.instance.controller.blockClass.RemoveBlock("ChangeScene");
        PlayerHandler.instance.controller.blockClass.RemoveBlock("MainMenu");
    }

    IEnumerator ResetSceneProcess(StageData data)
    {
        PlayerHandler.instance.controller.blockClass.AddBlock("ChangeScene", BlockClass.BlockType.Complete);

        yield return StartCoroutine(LowerCurtainsProcess());
        //Debug.Log("0 " + PlayerHandler.instance.lastSpawnPointIndex);

        yield return StartCoroutine(LoadSameSceneProcess(data));


        LocalHandler.instance.StartLocalHandler(data);
        UIHandler.instance.ControlHolder(true);
        currentScene = data.stageId;

    
        yield return StartCoroutine(PlayerHandler.instance.FixPlayerPositionProcess());

        PlayerHandler.instance.RemoveIsDead();

        yield return StartCoroutine(RaiseCurtainsProcess());


        PlayerHandler.instance.controller.blockClass.RemoveBlock("ChangeScene");
        PlayerHandler.instance.controller.blockClass.RemoveBlock("MainMenu");
    }

    #endregion

    #region SECONDARY PROCESSES

    IEnumerator LoadAnotherSceneProcess(StageData data)
    {
        //we check which sccene it is and we activate

        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(data.stageId, LoadSceneMode.Additive);

        yield return new WaitUntil(() => loadAsync.isDone);


        Debug.Log("this is the currentscene " + currentScene);

        AsyncOperation unloadAsync = SceneManager.UnloadSceneAsync(currentScene, UnloadSceneOptions.None);


        yield return new WaitUntil(() => unloadAsync.isDone);


        SaveClass save = new SaveClass();

        SaveHandler2.SaveData("0", save, true);
     
        yield return new WaitUntil(() => GameHandler.instance != null && UIHandler.instance != null);

   
        
    }

    IEnumerator LoadSameSceneProcess(StageData data)
    {
        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        yield return new WaitUntil(() => loadSceneAsync.isDone);

        AsyncOperation mainUnloadAsync = SceneManager.UnloadSceneAsync(data.stageId);

        yield return new WaitUntil(() => mainUnloadAsync.isDone);

        AsyncOperation mainloadAsync = SceneManager.LoadSceneAsync(data.stageId, LoadSceneMode.Additive);

        yield return new WaitUntil(() => mainloadAsync.isDone);

        AsyncOperation unloadAsync = SceneManager.UnloadSceneAsync(1, UnloadSceneOptions.None);

        yield return new WaitUntil(() => unloadAsync.isDone);

        yield return new WaitUntil(() => GameHandler.instance != null && UIHandler.instance != null && PlayerHandler.instance != null);


    }

    #endregion

    #region CURTAIN
    IEnumerator LowerCurtainsProcess()
    {
        PlayerHandler.instance.controller.blockClass.AddBlock("SceneLoad", BlockClass.BlockType.Complete);
        blackScreen.gameObject.SetActive(true);
        while (blackScreen.color.a < 1)
        {
            blackScreen.color += new Color(0, 0, 0, 0.035f);
            yield return new WaitForSeconds(0.001f);
        }
    }

    IEnumerator RaiseCurtainsProcess()
    {
        while (blackScreen.color.a > 0)
        {
            blackScreen.color -= new Color(0, 0, 0, 0.035f);
            yield return new WaitForSeconds(0.001f);
        }
        blackScreen.gameObject.SetActive(false);
        PlayerHandler.instance.controller.blockClass.RemoveBlock("SceneLoad");

    }
    #endregion

}



//LETS FIX THIS WHOLE MESS.
//now i need to deal with the thing about putting the player in the right position.
