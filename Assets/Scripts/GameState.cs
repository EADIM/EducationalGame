using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public bool State = false;
    public GameObject character;
    public Canvas ui;

    string[] uiElements = {
        "Level Stats",
        "Input Container",
        "Winner Window",
        "Buttons"
    };

    public bool winLevel = false;

    public bool firstStage = true;
    public bool secondStage = false;

    private void Update() {
        //Debug.Log("State: " + State);
        if (winLevel){
            showWinMenu();
            GameObject buttons = GetChildWithName(ui.gameObject, uiElements[3]);
            Hide(buttons.GetComponent<CanvasGroup>());
        }
    }

    public void SwitchState(){
        ui.GetComponent<ToggleCamera>().switchCamera();
        
        if (State){
            if(secondStage){
                HideUI();
            }
            else{
                ResetValues();
            }
        }
        else{
            ShowInGame();
        }

        State = !State;
    }

    public void ResetValues(){
        winLevel = false;
        firstStage = true;
        secondStage = false;
        character.GetComponent<SimulationMovement>().ResetValues();
        HideUI();
    }

    public void HideUI(){
        GameObject LevelStats = GetChildWithName(ui.gameObject, uiElements[0]);
        if (checkIfNull(LevelStats, uiElements[0], "ResetValues")){
            Hide(LevelStats.GetComponent<CanvasGroup>());
        }
        GameObject buttons = GetChildWithName(ui.gameObject, uiElements[3]);
        Show(buttons.GetComponent<CanvasGroup>());
    }
    private void ShowInGame(){
        GameObject LevelStats = GetChildWithName(ui.gameObject, uiElements[0]);
        if (checkIfNull(LevelStats, uiElements[0], "ShowInGame")){
            Show(LevelStats.GetComponent<CanvasGroup>());
        }
        GameObject InputContainer = GetChildWithName(ui.gameObject, uiElements[1]);
        if (checkIfNull(InputContainer, uiElements[1], "ResetValues")){
            Hide(InputContainer.GetComponent<CanvasGroup>());
        }
    }

    private GameObject GetChildWithName(GameObject obj, string name) {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null) {
            return childTrans.gameObject;
        } else {
            return null;
        }
    }

    private void showWinMenu(){
        GameObject winWindow = GetChildWithName(ui.gameObject, uiElements[2]);
        if(checkIfNull(winWindow, uiElements[2], "showWinMenu")){
            winWindow.SetActive(true);
        }
    }

    void Hide(CanvasGroup canvasGroup) {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    void Show(CanvasGroup canvasGroup) {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    private bool checkIfNull(GameObject obj, string objName, string functionName){
        string msg;
        bool returnValue = false;
        if (obj) {
            msg = objName + " is not null.";
            returnValue = true;
        }
        else{
            msg = objName + " is null.";
            returnValue = false;
        }

        //Debug.Log(functionName + ": " + msg);
        return returnValue;
    }
}
