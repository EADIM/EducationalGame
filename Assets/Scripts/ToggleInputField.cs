using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInputField : MonoBehaviour
{
    
    public bool shouldActivateInputContainer;
    public GameObject inputContainer;

    public Camera explorerCamera;
    public bool isMenuActive;

    public ToggleUIElement uIElement;

    void Start()
    {
        uIElement = inputContainer.GetComponent<ToggleUIElement>();
        shouldActivateInputContainer = inputContainer.activeSelf;
        isMenuActive = false;
    }

    void Update() 
    {    
        uIElement.canShow = !transform.GetComponentInParent<GameState>().State;

        if(Input.GetKey(KeyCode.Escape)){
            shouldActivateInputContainer = false;
            toggleInputContainer();
        }
    }

    private void OnMouseDown() 
    {
        toggleInputContainer();
    }

    public void toggleInputContainer(){
        if (shouldActivateInputContainer && !isMenuActive && uIElement.canShow){
            uIElement.Show();
        }
        else{
            uIElement.Hide();
        }
        //inputContainer.SetActive(shouldActivateInputContainer && !isMenuActive);
        shouldActivateInputContainer = !shouldActivateInputContainer;
    }

    public void activateMenu(){
        isMenuActive = true;
    }

    public void deactivateMenu(){
        isMenuActive = false;
    }
}
