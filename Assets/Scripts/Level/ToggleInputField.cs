using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInputField : MonoBehaviour
{
    
    public bool shouldActivateInputContainer;
    public GameObject inputContainer;
    public GameObject gameState;
    private ToggleUIElement uIElement;

    void Start()
    {
        uIElement = inputContainer.GetComponent<ToggleUIElement>();
        shouldActivateInputContainer = gameState.GetComponent<GameState>().States["Exploration"];
    }

    void Update() 
    {    
        if(Input.GetKey(KeyCode.Escape)){
            shouldActivateInputContainer = false;
            toggleInputContainer();
        }
    }

    private void OnMouseDown() 
    {
        shouldActivateInputContainer = gameState.GetComponent<GameState>().States["Exploration"];
        toggleInputContainer();
    }

    public void toggleInputContainer(){
        if (shouldActivateInputContainer){
            uIElement.Show();
        }
        else{
            uIElement.Hide();
        }
    }
}
