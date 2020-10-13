using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState : MonoBehaviour
{

    public References references;

    private GameObject Player;
    private Canvas Canvas;
    private ToggleCamera ToggleCamera;
    private GetProblemInfo getProblemInfo;
    public Dictionary<string, bool> States = new Dictionary<string, bool>();

    public string currentState = "Start";
    public string previousState = "Exploration";

    private readonly string SimulationName = "Simulation";
    private readonly string ExplorationName = "Exploration";
    private readonly string WinName = "Win";
    private readonly string LostName = "Lost";
    private readonly string PauseName = "Pause";
    private readonly string StartName = "Start";
    private ToggleUIElement inputfieldUI;
    public float UnitScale = 0.91177873f; //the amount of units to relative to a meter. (prev = 0.5492f, 1.111436f)

    public bool pausedFromUI = false;

    public bool FirstExplanationWindow = false;


    private void Awake()
    {
        InitializeStates();    
        Application.targetFrameRate = 60;
    }
    
    private void Start() 
    {
        UnitScale = 0.54f;
        Player = references.Player;
        Canvas = references.Canvas.GetComponent<Canvas>();
        transform.GetComponent<GetProblemInfo>().OnIntialPlatformChange();
        GameObject inputField = Utils.GetChildWithName(Canvas.gameObject, "Input Container");
        getProblemInfo = transform.GetComponent<GetProblemInfo>();
        inputfieldUI = inputField.GetComponent<ToggleUIElement>();
        ToggleCamera = transform.gameObject.GetComponent<ToggleCamera>();
        SwitchState(getStartName());
    }

    private void Update()
    {
        
    }


    public void SwitchState(string newState)
    {
        //Debug.Log("State switched to " + newState, this);
        Dictionary<string, bool>.KeyCollection kc = States.Keys;

        foreach(string key in kc.ToList())
        {
            States[key] = false;
        }

        States[newState] = true;

        if(newState != currentState)
        {
            previousState = currentState;
        }
        currentState = newState;

        if (newState == getExplorationName())
        {
            changeExploration();
        }
        else if (newState == getSimulationName())
        {
            changeSimulation();
        }
        else if (newState == getLostName())
        {
            changeLost();
        }
        else if (newState == getWinName())
        {
            changeWin();
        }
        else if (newState == getPauseName())
        {
            changePause();
        }
        else if (newState == getStartName())
        {
            changeStart();
        }
    }

    public void ResetValues()
    {
        references.ExplorerCamera.transform.position = references.ExplorerCamera.GetComponent<CameraController>().InitialPosition;
        references.ExplorerCamera.transform.rotation = references.ExplorerCamera.GetComponent<CameraController>().InitialRotation;
        Player.GetComponent<PlayerController>().ResetEverythingFromScratch();
        GameObject Timer =  Utils.GetChildWithName(Canvas.gameObject, "Timer");
        Timer.GetComponent<Timer>().Reset();
        currentState = getStartName();
        previousState = getExplorationName();
        SwitchState(getStartName());
    }



    private void changeExploration()
    {
        GameObject explanationContainer = Utils.GetChildWithName(Canvas.gameObject, "Explanation Container");
        explanationContainer.GetComponent<ToggleUIElement>().Hide();
        List<Checkpoint> checkpoints = Player.GetComponent<PlayerController>().Checkpoints;
        if(checkpoints.Count > 0)
        {
            if(!pausedFromUI){
                Player.GetComponent<PlayerController>().Reset(checkpoints[checkpoints.Count - 1]);
            }
        }
        
        /*
        ToggleCamera.switchToCamera(ToggleCamera.currentCamera, ToggleCamera.cameras[1]);
        string text = ToggleCamera.getActiveCameraName() + " selecionada.";
        ShowMessage(text, Canvas.gameObject, "Message Container", "Action Message", new Color(154.0f/255.0f,0,0,1));
        references.PlayerProfileCamera.GetComponent<Camera>().enabled = false;
        */

        ResumeUIElements();
        GameObject buttons = Utils.GetChildWithName(Canvas.gameObject, "Buttons");
        GameObject buttons_helpButton = Utils.GetChildWithName(buttons, "Help");
        buttons_helpButton.GetComponent<ToggleUIElement>().Show();
        GameObject buttons_playButton = Utils.GetChildWithName(buttons, "Play Button");
        GameObject buttons_playButton_text = Utils.GetChildWithName(buttons_playButton, "Text");
        buttons_playButton_text.GetComponent<TMPro.TMP_Text>().text = "PLAY";
        GameObject joysticks_container = Utils.GetChildWithName(Canvas.gameObject, "Joysticks Container");
        joysticks_container.GetComponent<ToggleUIElement>().Show();
        UnfreezeUI();
        pausedFromUI = false;
    }

    private void changeSimulation()
    {
        if((Player.GetComponent<PlayerController>().Acceleration <= 0.0f || Player.GetComponent<PlayerController>().Acceleration > 1000.0f)
         || (Player.GetComponent<PlayerController>().JumpAngle <= 0.0f || Player.GetComponent<PlayerController>().JumpAngle > 90.0f))
         {
            Debug.Log("WARNING: inavlid player properties!");
            //Show warning message;
            string text = "Aceleração ou ângulo inválidos. Para a aceleração, escolha valores entre 1 e 1000. Para o ângulo, escolha valores entre 1 e 90.";
            ShowMessage(text, Canvas.gameObject, "Message Container", "Action Message", new Color(154.0f/255.0f,0,0,1));
            //Switch state to lost
            SwitchState(getLostName());
            return;
        }
       
        ResumeUIElements();
        GameObject joysticks_container = Utils.GetChildWithName(Canvas.gameObject, "Joysticks Container");
        joysticks_container.GetComponent<ToggleUIElement>().Hide();
        GameObject levelStats = Utils.GetChildWithName(Canvas.gameObject, "Level Stats");
        levelStats.GetComponent<ToggleUIElement>().Show();
        inputfieldUI.Hide();
        GameObject buttons = Utils.GetChildWithName(Canvas.gameObject, "Buttons");
        GameObject buttons_helpButton = Utils.GetChildWithName(buttons, "Help");
        buttons_helpButton.GetComponent<ToggleUIElement>().Hide();
        GameObject buttons_playButton = Utils.GetChildWithName(buttons, "Play Button");
        GameObject buttons_playButton_text = Utils.GetChildWithName(buttons_playButton.gameObject, "Text");
        buttons_playButton_text.GetComponent<TMPro.TMP_Text>().text = "STOP";
    }

    private void changePause()
    {
        pausedFromUI = true;
        GamePause.PauseGame();
        PauseUIElements();
    }

    private void changeLost()
    {
        Player.GetComponent<PlayerController>().Reset(Player.GetComponent<PlayerController>().Checkpoints[Player.GetComponent<PlayerController>().Checkpoints.Count - 1]);
        //playMessage(0); //Failure
        SwitchState(getExplorationName());
    }

    private void changeWin()
    {
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
        string text = "Tempo tomado: ";
        text = text + Timer.GetComponent<TMPro.TMP_Text>().text;
        TimeText.GetComponent<TMPro.TMP_Text>().text = text;
        GamePause.PauseGame();
    }

    private void changeStart()
    {
        GameManager.RandScale();
        
        MapScaler scaler = this.transform.GetComponent<MapScaler>();
        scaler.Scale();
        Vector3 newPos = references.PlataformaInicial01.transform.position;
        newPos.x += 4.65f;
        newPos.z += -4.0f;
        Player.transform.position = newPos;

        getProblemInfo.OnVariablesChange();
        references.QuestionInfo.GetComponent<SetProblemInfo>().OnInfoChanged(getProblemInfo);
        references.ExplanationInfo.GetComponent<SetProblemInfo>().OnInfoChanged(getProblemInfo);
        //references.HelpInfo.GetComponent<SetProblemInfo>().OnInfoChanged(getProblemInfo);

        Vector3 playerPos = Player.transform.position;
        playerPos.z += 20f;
        playerPos.y += 15f;
        ToggleCamera.cameras[1].transform.position = playerPos;

        FreezeUI();
        pausedFromUI = true;
        GamePause.PauseGame();
        ResumeUIElements();
        GameObject buttons = Utils.GetChildWithName(Canvas.gameObject, "Buttons");
        FreezeElement(buttons);
        GameObject buttons_helpButton = Utils.GetChildWithName(buttons, "Help");
        buttons_helpButton.GetComponent<ToggleUIElement>().Hide();
        GetComponent<ToggleInputField>().canShow = false;
        ChangePlayerPosition.canChangePosition = false;
        GameObject explanationContainer = Utils.GetChildWithName(Canvas.gameObject, "Explanation Container");
        explanationContainer.GetComponent<ToggleUIElement>().Show();
    }



    private void PauseUIElements()
    {
        GetComponent<ToggleInputField>().canShow = false;
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

    private void ResumeUIElements()
    {
        GetComponent<ToggleInputField>().canShow = true;
        ChangePlayerPosition.canChangePosition = true;
        GamePause.ResumeGame();
        GameObject buttons = Utils.GetChildWithName(Canvas.gameObject, "Buttons");
        buttons.GetComponent<ToggleUIElement>().Show();
        GameObject levelStats = Utils.GetChildWithName(Canvas.gameObject, "Level Stats");
        levelStats.GetComponent<ToggleUIElement>().Show();
        GameObject pauseMenu = Utils.GetChildWithName(Canvas.gameObject, "Pause Menu");
        pauseMenu.GetComponent<ToggleUIElement>().Hide();
    }



    public void FreezeUI()
    {
        GameObject buttons = Utils.GetChildWithName(Canvas.gameObject, "Buttons");
        FreezeElement(buttons);
        GameObject levelStats = Utils.GetChildWithName(Canvas.gameObject, "Level Stats");
        FreezeElement(levelStats);
        GameObject joysticks_container = Utils.GetChildWithName(Canvas.gameObject, "Joysticks Container");
        FreezeElement(joysticks_container);
    }

    private void FreezeElement(GameObject elem)
    {
        Utils.Freeze(elem.GetComponent<CanvasGroup>());
    }

    public void UnfreezeUI()
    {
        GameObject buttons = Utils.GetChildWithName(Canvas.gameObject, "Buttons");
        UnfreezeElement(buttons);
        GameObject levelStats = Utils.GetChildWithName(Canvas.gameObject, "Level Stats");
        UnfreezeElement(levelStats);
        GameObject joysticks_container = Utils.GetChildWithName(Canvas.gameObject, "Joysticks Container");
        UnfreezeElement(joysticks_container);
    }

    private void UnfreezeElement(GameObject elem)
    {
        Utils.Unfreeze(elem.GetComponent<CanvasGroup>());
    }



    public void SwitchToPreviousState()
    {
        SwitchState(previousState);
    }

    private void InitializeStates()
    {
        States.Add(getStartName(), true);
        States.Add(getExplorationName(), false);
        States.Add(getSimulationName(), false);
        States.Add(getLostName(), false);
        States.Add(getWinName(), false);
        States.Add(getPauseName(), false);
    }


    public void GS_SwitchCameras(){
        ToggleCamera.switchCameras();
        string text = ToggleCamera.getActiveCameraName() + " selecionada.";
        ShowMessage(text, Canvas.gameObject, "Message Container", "Action Message", new Color(154.0f/255.0f,0,0,1));
    }

    public void ShowMessage(string msg, GameObject canvas, string message_container_name, string action_message_name, Color color){
        GameObject MessageContainer = Utils.GetChildWithName(canvas, message_container_name);
        GameObject message = Utils.GetChildWithName(MessageContainer, action_message_name);
        TMPro.TMP_Text messageTMP = message.GetComponent<TMPro.TMP_Text>();
        messageTMP.text = msg;
        messageTMP.color = color;
        ToggleUIElement MessageContainerUI = MessageContainer.GetComponent<ToggleUIElement>();
        MessageContainerUI.Show();
        StartCoroutine(HideMessage(4.0f, MessageContainerUI));
    }

    private IEnumerator HideMessage(float waitTime, ToggleUIElement uiElem)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        uiElem.Hide();
    }


    public string getSimulationName()
    {
        return SimulationName;
    }

    public string getExplorationName()
    {
        return ExplorationName;
    }

    public string getLostName()
    {
        return LostName;
    }

    public string getWinName()
    {
        return WinName;
    }

    public string getPauseName()
    {
        return PauseName;
    }

    public string getStartName()
    {
        return StartName;
    }



    public void setFirstExplanationWindow(bool value)
    {
        FirstExplanationWindow = value;
    }
}
