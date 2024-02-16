
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] int currentScene;
    [SerializeField] Image blackScreen;


    int currentForBigAd;
    bool first; 

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

    public void ResetScene(StageData data, StageTimeClass currentTimer)
    {
        //then we reload the currentscene.       
        StopAllCoroutines();
        StartCoroutine(ResetSceneProcess(data, currentTimer));      
    }




    #endregion

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
        UIHandler.instance.uiPlayer.ResetTimer();
        currentScene = data.stageId;
        //so here before we raise the curtains we always get the player position fixed.
        //we now tell teh player what it should do.
        PlayerHandler.instance.ResetScenePlayer();
        LocalHandler.instance.StartLocalHandler(data);

        yield return StartCoroutine(PlayerHandler.instance.FixPlayerPositionProcess());

        PlayerHandler.instance.ChangeProgress(); //this makes that the currentscene
        PlayerHandler.instance.graphic.StopAnimation();

        yield return StartCoroutine(RaiseCurtainsProcess());

        PlayerHandler.instance.controller.blockClass.RemoveBlock("ChangeScene");
        PlayerHandler.instance.controller.blockClass.RemoveBlock("MainMenu");
    }

    IEnumerator ResetSceneProcess(StageData data, StageTimeClass timeClass )
    {
        PlayerHandler.instance.controller.blockClass.AddBlock("ChangeScene", BlockClass.BlockType.Complete);

        yield return StartCoroutine(LowerCurtainsProcess());
        //Debug.Log("0 " + PlayerHandler.instance.lastSpawnPointIndex);

        yield return StartCoroutine(LoadSameSceneProcess(data));


        
        UIHandler.instance.ControlHolder(true);
        UIHandler.instance.uiPlayer.ResetTimer();
        
        currentScene = data.stageId;

    
        yield return StartCoroutine(PlayerHandler.instance.FixPlayerPositionProcess());

        LocalHandler.instance.StartLocalHandler(data, timeClass);
        PlayerHandler.instance.graphic.StopAnimation();
        //PlayerHandler.instance.cam.ResetCam();

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


        AsyncOperation unloadAsync = SceneManager.UnloadSceneAsync(currentScene, UnloadSceneOptions.None);


        yield return new WaitUntil(() => unloadAsync.isDone);


        SaveClass save = new SaveClass();

        SaveHandler2.SaveData("0", save, true);
     
        yield return new WaitUntil(() => GameHandler.instance != null && UIHandler.instance != null);

        yield return StartCoroutine(ShowAdProcess());

        PlayerHandler.instance.controller.blockClass.ClearBlock();


        //we play an ad.
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

        yield return StartCoroutine(ShowAdProcess());


        PlayerHandler.instance.controller.blockClass.ClearBlock();

    }


    IEnumerator ShowAdProcess()
    {



        AdHandler adHandler = GameHandler.instance.adHandler;

        if(adHandler.debugShouldNotShowAd) 
        {
            Debug.Log("do not show ads");
            yield break;
        }


        currentForBigAd += 3;

        

        if (currentForBigAd >= 3)
        {
            currentForBigAd = 0;

            Debug.Log("this was called");
            GameHandler.instance.adHandler.RequestRewardAd(RewardType.Nothing);
        }
        else
        {
            GameHandler.instance.adHandler.RequestInterstitial();
        }



        yield return new WaitUntil(() => !adHandler.isShowingAd);


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
