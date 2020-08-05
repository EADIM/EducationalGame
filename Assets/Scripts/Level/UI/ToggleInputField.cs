using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleInputField : MonoBehaviour
{
    
    public bool shouldActivateInputContainer;
    public References references;
    public GameObject inputContainer;
    private GameObject gameState;
    public bool canShow = true;
    private ToggleUIElement UIElement;


    void Start()
    {
        UIElement = inputContainer.GetComponent<ToggleUIElement>();
        gameState = references.GameState;
        shouldActivateInputContainer = gameState.GetComponent<GameState>().States["Exploration"];
    }

    void Update() 
    {    
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.LogFormat("Escape key pressed.");
            shouldActivateInputContainer = false;
            toggleInputContainer();
        }
    }

    private void OnMouseDown() 
    {
        // Check if there is a touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            int fingerID = Input.GetTouch(0).fingerId;
        
            // Check if finger is over a UI element
            if (EventSystem.current.IsPointerOverGameObject(fingerID))
            {
                Debug.Log("finger is over panel.");
                return;
            }
            else{
                Debug.Log("finger is not over panel.");
                shouldActivateInputContainer = gameState.GetComponent<GameState>().States["Exploration"];
                toggleInputContainer();
            }
        }
    }

    public void toggleInputContainer(){
        if (!UIElement.isVisible && shouldActivateInputContainer && canShow){
            UIElement.Show();
            ChangePlayerPosition.canChangePosition = false;
            GameObject jc = Utils.GetChildWithName(references.Canvas, "Joysticks Container");
            jc.GetComponent<ToggleUIElement>().Hide();

        }
        else{
            UIElement.Hide();
            ChangePlayerPosition.canChangePosition = true;
            GameObject jc = Utils.GetChildWithName(references.Canvas, "Joysticks Container");
            jc.GetComponent<ToggleUIElement>().Show();
        }
    }

    public void changeToFalse(){
        canShow = false;
    }

    public void changeToTrue(){
        canShow = true;
    }
}
