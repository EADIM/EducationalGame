using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public GameObject character;
    public Canvas canvas;
    public Dictionary<string, bool> States = new Dictionary<string, bool>();

    public string currentState = "Exploration";
    public string previousState = "Exploration";

    private string SimulationName = "Simulation";
    private string ExplorationName = "Exploration";
    private string WinName = "Win";
    private string LostName = "Lost";
    private string PauseName = "Pause";

    private void Awake()
    {
        InitializeStates();    
    }
    
    public void SwitchState(string newState){
        Debug.Log("State switched to " + newState, this);
        Dictionary<string, bool>.KeyCollection kc = States.Keys;

        foreach(string key in kc.ToList()){
            States[key] = false;
        }

        States[newState] = true;
        previousState = currentState;
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
        character.GetComponent<SimulationMovement>().ResetEverythingFromScratch();
        GameObject Timer =  Utils.GetChildWithName(canvas.gameObject, "Timer");
        Timer.GetComponent<Timer>().Reset();
        currentState = getExplorationName();
        previousState = getExplorationName();
        SwitchState(getExplorationName());
    }

    private void changeExploration(){
        transform.gameObject.GetComponent<ToggleCamera>().switchToExplore();
        ResumeUIElements();
        GameObject buttons = Utils.GetChildWithName(canvas.gameObject, "Buttons");
        GameObject buttons_playButton = Utils.GetChildWithName(buttons, "Play Button");
        GameObject buttons_playButton_text = Utils.GetChildWithName(buttons_playButton, "Text");
        buttons_playButton_text.GetComponent<TMPro.TMP_Text>().text = "PLAY";
        GameObject joysticks_container = Utils.GetChildWithName(canvas.gameObject, "Joysticks Container");
        joysticks_container.GetComponent<ToggleUIElement>().Show();
    }

    private void changeSimulation(){
        transform.gameObject.GetComponent<ToggleCamera>().switchToPlayer();
        ResumeUIElements();
        GameObject joysticks_container = Utils.GetChildWithName(canvas.gameObject, "Joysticks Container");
        joysticks_container.GetComponent<ToggleUIElement>().Hide();
        GameObject levelStats = Utils.GetChildWithName(canvas.gameObject, "Level Stats");
        levelStats.GetComponent<ToggleUIElement>().Show();
        GameObject inputField = Utils.GetChildWithName(canvas.gameObject, "Input Container");
        inputField.GetComponent<ToggleUIElement>().Hide();
        GameObject buttons = Utils.GetChildWithName(canvas.gameObject, "Buttons");
        GameObject buttons_playButton = Utils.GetChildWithName(buttons, "Play Button");
        GameObject buttons_playButton_text = Utils.GetChildWithName(buttons_playButton.gameObject, "Text");
        buttons_playButton_text.GetComponent<TMPro.TMP_Text>().text = "STOP";
    }

    private void changePause(){
        transform.gameObject.GetComponent<GamePause>().PauseGame();
        PauseUIElements();
    }

    private void changeLost(){
        character.GetComponent<SimulationMovement>().Reset();
        //playMessage(0); //Failure
        SwitchState(getExplorationName());
    }

    private void changeWin(){
        GameObject buttons = Utils.GetChildWithName(canvas.gameObject, "Buttons");
        buttons.GetComponent<ToggleUIElement>().Hide();
        GameObject inputField = Utils.GetChildWithName(canvas.gameObject, "Input Container");
        inputField.GetComponent<ToggleUIElement>().Hide();
        GameObject levelStats = Utils.GetChildWithName(canvas.gameObject, "Level Stats");
        levelStats.GetComponent<ToggleUIElement>().Hide();
        GameObject joysticks_container = Utils.GetChildWithName(canvas.gameObject, "Joysticks Container");
        joysticks_container.GetComponent<ToggleUIElement>().Hide();
        GameObject winnerWindow = Utils.GetChildWithName(canvas.gameObject, "Winner Window");
        winnerWindow.GetComponent<ToggleUIElement>().Show();
        GameObject TimeText = Utils.GetChildWithName(winnerWindow.gameObject, "Time Taken");
        GameObject Timer =  Utils.GetChildWithName(canvas.gameObject, "Timer");
        string text = TimeText.GetComponent<TMPro.TMP_Text>().text;
        text += Timer.GetComponent<TMPro.TMP_Text>().text;
        TimeText.GetComponent<TMPro.TMP_Text>().text = text;
        transform.gameObject.GetComponent<GamePause>().PauseGame();
    }

    private void PauseUIElements(){
        GameObject buttons = Utils.GetChildWithName(canvas.gameObject, "Buttons");
        buttons.GetComponent<ToggleUIElement>().Hide();
        GameObject inputField = Utils.GetChildWithName(canvas.gameObject, "Input Container");
        inputField.GetComponent<ToggleUIElement>().Hide();
        GameObject levelStats = Utils.GetChildWithName(canvas.gameObject, "Level Stats");
        levelStats.GetComponent<ToggleUIElement>().Hide();
        GameObject joysticks_container = Utils.GetChildWithName(canvas.gameObject, "Joysticks Container");
        joysticks_container.GetComponent<ToggleUIElement>().Hide();
        GameObject pauseMenu = Utils.GetChildWithName(canvas.gameObject, "Pause Menu");
        pauseMenu.GetComponent<ToggleUIElement>().Show();
    }

    private void ResumeUIElements(){
        transform.gameObject.GetComponent<GamePause>().ResumeGame();
        GameObject buttons = Utils.GetChildWithName(canvas.gameObject, "Buttons");
        buttons.GetComponent<ToggleUIElement>().Show();
        GameObject levelStats = Utils.GetChildWithName(canvas.gameObject, "Level Stats");
        levelStats.GetComponent<ToggleUIElement>().Show();
        GameObject pauseMenu = Utils.GetChildWithName(canvas.gameObject, "Pause Menu");
        pauseMenu.GetComponent<ToggleUIElement>().Hide();
    }

    public void FreezeUI(){
        GameObject buttons = Utils.GetChildWithName(canvas.gameObject, "Buttons");
        FreezeElement(buttons);
        GameObject levelStats = Utils.GetChildWithName(canvas.gameObject, "Level Stats");
        FreezeElement(levelStats);
        GameObject joysticks_container = Utils.GetChildWithName(canvas.gameObject, "Joysticks Container");
        FreezeElement(joysticks_container);
    }

    private void FreezeElement(GameObject elem){
        Utils.Freeze(elem.GetComponent<CanvasGroup>());
    }

    public void UnfreezeUI(){
        GameObject buttons = Utils.GetChildWithName(canvas.gameObject, "Buttons");
        UnfreezeElement(buttons);
        GameObject levelStats = Utils.GetChildWithName(canvas.gameObject, "Level Stats");
        UnfreezeElement(levelStats);
        GameObject joysticks_container = Utils.GetChildWithName(canvas.gameObject, "Joysticks Container");
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
        GameObject message = Utils.GetChildWithName(canvas.gameObject, "Message");

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
