using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState : MonoBehaviour
{

    public References references;

    private GameObject Player;
    private Canvas Canvas;
    public Dictionary<string, bool> States = new Dictionary<string, bool>();

    public string currentState = "Exploration";
    public string previousState = "Exploration";

    private string SimulationName = "Simulation";
    private string ExplorationName = "Exploration";
    private string WinName = "Win";
    private string LostName = "Lost";
    private string PauseName = "Pause";
    private ToggleUIElement inputfieldUI;
    public float UnitScale = 1.0f;

    public bool pausedFromUI = false;

    private void Awake()
    {
        InitializeStates();    
        Application.targetFrameRate = 60;
    }
    
    private void Start() {
        Player = references.Player;
        Canvas = references.Canvas.GetComponent<Canvas>();
        transform.GetComponent<GetProblemInfo>().OnIntialPlatformChange();
        references.QuestionInfo.GetComponent<SetProblemInfo>().OnInfoChanged(transform.GetComponent<GetProblemInfo>());
        GameObject inputField = Utils.GetChildWithName(Canvas.gameObject, "Input Container");
        inputfieldUI = inputField.GetComponent<ToggleUIElement>();
    }

    private void Update(){
        if(!inputfieldUI.isVisible){
            if(Input.GetKey(KeyCode.Escape) && !States[getPauseName()]){
                SwitchState(getPauseName());
            }
        }
    }

    public void SwitchState(string newState){
        Debug.Log("State switched to " + newState, this);
        Dictionary<string, bool>.KeyCollection kc = States.Keys;

        foreach(string key in kc.ToList()){
            States[key] = false;
        }

        States[newState] = true;

        if(currentState != previousState)
        {
            previousState = currentState;
        }
        currentState = newState;

        if (newState == getExplorationName()){
            changeExploration();
        }
        else if (newState == getSimulationName()){
            changeSimulation();
        }
        else if (newState == getLostName()){
            changeLost();
        }
        else if (newState == getWinName()){
            changeWin();
        }
        else if (newState == getPauseName()){
            changePause();
        }
    }

    public void ResetValues(){
        references.ExplorerCamera.transform.position = references.ExplorerCamera.GetComponent<CameraController>().InitialPosition;
        references.ExplorerCamera.transform.rotation = references.ExplorerCamera.GetComponent<CameraController>().InitialRotation;
        Player.GetComponent<PlayerController>().ResetEverythingFromScratch();
        GameObject Timer =  Utils.GetChildWithName(Canvas.gameObject, "Timer");
        Timer.GetComponent<Timer>().Reset();
        currentState = getExplorationName();
        previousState = getExplorationName();
        SwitchState(getExplorationName());
    }

    private void changeExploration(){
        List<Checkpoint> checkpoints = Player.GetComponent<PlayerController>().Checkpoints;
        if(checkpoints.Count > 0)
        {
            if(!pausedFromUI){
                Player.GetComponent<PlayerController>().Reset(checkpoints[checkpoints.Count - 1]);
            }
        }
        transform.gameObject.GetComponent<ToggleCamera>().switchToCamera01();
        references.PlayerProfileCamera.GetComponent<Camera>().enabled = false;
        ResumeUIElements();
        GameObject buttons = Utils.GetChildWithName(Canvas.gameObject, "Buttons");
        GameObject buttons_helpButton = Utils.GetChildWithName(buttons, "Help");
        buttons_helpButton.GetComponent<ToggleUIElement>().Show();
        GameObject buttons_switchCameras = Utils.GetChildWithName(buttons, "Switch Cameras");
        buttons_switchCameras.GetComponent<ToggleUIElement>().Hide();
        GameObject buttons_playButton = Utils.GetChildWithName(buttons, "Play Button");
        GameObject buttons_playButton_text = Utils.GetChildWithName(buttons_playButton, "Text");
        buttons_playButton_text.GetComponent<TMPro.TMP_Text>().text = "PLAY";
        GameObject joysticks_container = Utils.GetChildWithName(Canvas.gameObject, "Joysticks Container");
        joysticks_container.GetComponent<ToggleUIElement>().Show();
        pausedFromUI = false;
    }

    private void changeSimulation(){
        transform.gameObject.GetComponent<ToggleCamera>().switchToCamera02();
        references.PlayerProfileCamera.GetComponent<Camera>().enabled = false;
        ResumeUIElements();
        GameObject joysticks_container = Utils.GetChildWithName(Canvas.gameObject, "Joysticks Container");
        joysticks_container.GetComponent<ToggleUIElement>().Hide();
        GameObject levelStats = Utils.GetChildWithName(Canvas.gameObject, "Level Stats");
        levelStats.GetComponent<ToggleUIElement>().Show();
        inputfieldUI.Hide();
        GameObject buttons = Utils.GetChildWithName(Canvas.gameObject, "Buttons");
        GameObject buttons_helpButton = Utils.GetChildWithName(buttons, "Help");
        buttons_helpButton.GetComponent<ToggleUIElement>().Hide();
        GameObject buttons_switchCameras = Utils.GetChildWithName(buttons, "Switch Cameras");
        buttons_switchCameras.GetComponent<ToggleUIElement>().Show();
        GameObject buttons_playButton = Utils.GetChildWithName(buttons, "Play Button");
        GameObject buttons_playButton_text = Utils.GetChildWithName(buttons_playButton.gameObject, "Text");
        buttons_playButton_text.GetComponent<TMPro.TMP_Text>().text = "STOP";
    }

    private void changePause(){
        pausedFromUI = true;
        transform.gameObject.GetComponent<GamePause>().PauseGame();
        PauseUIElements();
    }

    private void changeLost(){
        Player.GetComponent<PlayerController>().Reset(Player.GetComponent<PlayerController>().Checkpoints[Player.GetComponent<PlayerController>().Checkpoints.Count - 1]);
        //playMessage(0); //Failure
        SwitchState(getExplorationName());
    }

    private void changeWin(){
        GameObject buttons = Utils.GetChildWithName(Canvas.gameObject, "Buttons");
        buttons.GetComponent<ToggleUIElement>().Hide();
        inputfieldUI.Hide();
        GameObject levelStats = Utils.GetChildWithName(Canvas.gameObject, "Level Stats");
        levelStats.GetComponent<ToggleUIElement>().Hide();
        GameObject joysticks_container = Utils.GetChildWithName(Canvas.gameObject, "Joysticks Container");
        joysticks_container.GetComponent<ToggleUIElement>().Hide();
        GameObject winnerWindow = Utils.GetChildWithName(Canvas.gameObject, "Winner Window");
        winnerWindow.GetComponent<ToggleUIElement>().Show();
        GameObject TimeText = Utils.GetChildWithName(winnerWindow.gameObject, "Time Taken");
        GameObject Timer =  Utils.GetChildWithName(Canvas.gameObject, "Timer");
        string text = TimeText.GetComponent<TMPro.TMP_Text>().text;
        text = text + Timer.GetComponent<TMPro.TMP_Text>().text;
        TimeText.GetComponent<TMPro.TMP_Text>().text = text;
        transform.gameObject.GetComponent<GamePause>().PauseGame();
    }

    private void PauseUIElements(){
        Player.GetComponent<ToggleInputField>().canShow = false;
        ChangePlayerPosition.canChangePosition = false;
        GameObject buttons = Utils.GetChildWithName(Canvas.gameObject, "Buttons");
        buttons.GetComponent<ToggleUIElement>().Hide();
        inputfieldUI.Hide();
        GameObject levelStats = Utils.GetChildWithName(Canvas.gameObject, "Level Stats");
        levelStats.GetComponent<ToggleUIElement>().Hide();
        GameObject joysticks_container = Utils.GetChildWithName(Canvas.gameObject, "Joysticks Container");
        joysticks_container.GetComponent<ToggleUIElement>().Hide();
        GameObject pauseMenu = Utils.GetChildWithName(Canvas.gameObject, "Pause Menu");
        pauseMenu.GetComponent<ToggleUIElement>().Show();
    }

    private void ResumeUIElements(){
        Player.GetComponent<ToggleInputField>().canShow = true;
        ChangePlayerPosition.canChangePosition = true;
        transform.gameObject.GetComponent<GamePause>().ResumeGame();
        GameObject buttons = Utils.GetChildWithName(Canvas.gameObject, "Buttons");
        buttons.GetComponent<ToggleUIElement>().Show();
        GameObject levelStats = Utils.GetChildWithName(Canvas.gameObject, "Level Stats");
        levelStats.GetComponent<ToggleUIElement>().Show();
        GameObject pauseMenu = Utils.GetChildWithName(Canvas.gameObject, "Pause Menu");
        pauseMenu.GetComponent<ToggleUIElement>().Hide();
    }

    public void FreezeUI(){
        GameObject buttons = Utils.GetChildWithName(Canvas.gameObject, "Buttons");
        FreezeElement(buttons);
        GameObject levelStats = Utils.GetChildWithName(Canvas.gameObject, "Level Stats");
        FreezeElement(levelStats);
        GameObject joysticks_container = Utils.GetChildWithName(Canvas.gameObject, "Joysticks Container");
        FreezeElement(joysticks_container);
    }

    private void FreezeElement(GameObject elem){
        Utils.Freeze(elem.GetComponent<CanvasGroup>());
    }

    public void UnfreezeUI(){
        GameObject buttons = Utils.GetChildWithName(Canvas.gameObject, "Buttons");
        UnfreezeElement(buttons);
        GameObject levelStats = Utils.GetChildWithName(Canvas.gameObject, "Level Stats");
        UnfreezeElement(levelStats);
        GameObject joysticks_container = Utils.GetChildWithName(Canvas.gameObject, "Joysticks Container");
        UnfreezeElement(joysticks_container);
    }

    private void UnfreezeElement(GameObject elem){
        Utils.Unfreeze(elem.GetComponent<CanvasGroup>());
    }

    public void SwitchToPreviousState(){
        SwitchState(previousState);
    }

    private void InitializeStates(){
        States.Add(getExplorationName(), true);
        States.Add(getSimulationName(), false);
        States.Add(getLostName(), false);
        States.Add(getWinName(), false);
        States.Add(getPauseName(), false);
    }

    public void playMessage(int success){
        GameObject message = Utils.GetChildWithName(Canvas.gameObject, "Action Message");

        if (success == 0){    
            message.GetComponent<ActionMessage>().playFailureMessage();
        }
        else{
            message.GetComponent<ActionMessage>().playSuccessMessage();
        }
    }

    public string getSimulationName(){
        return SimulationName;
    }

    public string getExplorationName(){
        return ExplorationName;
    }

    public string getLostName(){
        return LostName;
    }

    public string getWinName(){
        return WinName;
    }

    public string getPauseName(){
        return PauseName;
    }

}
