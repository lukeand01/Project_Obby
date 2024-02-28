using MyBox;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    //GUIDE - this is just a place i will hold ui reference for easier use.

    public static UIHandler instance;

    GameObject holder;




    //GUIDE - this is for putting refernces. then its for getting references. they are separated to prevent changing refereneces from script.
    #region REFERENCES
    [Separator("Input")]
    [SerializeField] GameObject inputHolder;
    [SerializeField] Joystick moveJoystickRef;
    [SerializeField] Joystick cameraJoystickRef;
    [SerializeField] InputButton inputButtonJumpRef;
    [SerializeField] InputButton inputButtonPauseRef;


    [Separator("Button Power")]
    [SerializeField] ButtonPower buttonPowerTemplate;
    [SerializeField] Transform buttonPowerContainer;
    List<ButtonPower> buttonPowerList = new();


    [Separator("CANVAS REFERENCES")]
    [SerializeField] PauseUI uiPauseRef;
    [SerializeField] StageUI uiStageRef;
    [SerializeField] PlayerUI uiPlayerRef;
    [SerializeField] EndUI uiEndRef;
    [SerializeField] ConfirmationWindowUI uiConfirmatioWindowRef;
    //[SerializeField] RewardUI uiRewardRef;

    [Separator("TEMPLATE REFERENCES")]
    [SerializeField] FadeUI uiFadeRef;


    #endregion


    #region GETTERS INPUT
    public Joystick moveJoystick { get => moveJoystickRef; }

    public Joystick cameraJoystick { get => cameraJoystickRef; }

    public InputButton inputButtonJump { get => inputButtonJumpRef; }

    #endregion

    #region GETTERS UI
    public PauseUI uiPause { get => uiPauseRef; }
    public StageUI uiStage { get => uiStageRef; }

    public PlayerUI uiPlayer { get => uiPlayerRef; }

    public EndUI uiEnd { get => uiEndRef; }

    public ConfirmationWindowUI uiConfirmationWindow { get => uiConfirmatioWindowRef; }

    //public RewardUI uiReward { get => uiRewardRef; }    

    #endregion

    #region GETTERS TEMPLATE

    public FadeUI uiFade{ get => uiFadeRef; }

    #endregion


    #region POWER BUTTONS
    public void CreatePowerButtons(List<PowerData> powerDataList)
    {


        foreach (var item in powerDataList)
        {
            ButtonPower newObject = Instantiate(buttonPowerTemplate, Vector3.zero, Quaternion.identity);
            newObject.transform.parent = buttonPowerContainer;
            newObject.SetUpPower(item);
        }
    }

    public void UnselectPowerButton(PowerData data)
    {
        foreach (var item in buttonPowerList)
        {
            if (item.power == data)
            {

            }
        }
    }

    public void UnselectAllPowerButtons()
    {

    }
    #endregion




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        holder = transform.GetChild(0).gameObject;

        DontDestroyOnLoad(gameObject);
    }

    public void ControlHolder(bool choice)
    {
        holder.SetActive(choice);
        ControlInputButtons(choice);

        uiPlayerRef.ControlHolder(choice);
        uiEnd.Close();


        //reward ui only exists if the holder is closed because its only closed in the mainmenu
    }

    public void StopEverything()
    {
        uiEnd.Close();
        uiPause.StopPause();
    }


    public void ControlInputButtons(bool isVisible)
    {
        inputHolder.SetActive(isVisible);
    }

    public void ControlPlayerUI(bool isVisible)
    {

    }

}
