using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Fase01_ToggleInputField : MonoBehaviour
{
    
    public bool shouldActivateInputContainer;
    public Fase01_References references;
    public GameObject inputContainer;
    private GameObject gameState;
    public bool canShow = true;
    private ToggleUIElement UIElement;
    private Camera explorerCamera;

    private void Awake() {
        explorerCamera = references.ExplorerCamera.GetComponent<Camera>();
    }

    void Start()
    {
        UIElement = inputContainer.GetComponent<ToggleUIElement>();
        gameState = references.GameState;
        shouldActivateInputContainer = gameState.GetComponent<Fase01_GameState>().States["Exploration"];
    }

    void Update() 
    {    
        if (Input.GetKey(KeyCode.Escape) && UIElement.isVisible)
        {
            Debug.LogFormat("Escape key pressed.");
            shouldActivateInputContainer = false;
            toggleInputContainer();
        }
    
        //bool blockedByInterface;
        
        if(Application.isEditor)
        {
            if(Input.GetMouseButton(0))
            {
                checkPointer( Input.mousePosition );
            }
        }
        // Check if there is a touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            int fingerID = Input.GetTouch(0).fingerId;
            Vector3 touchPos = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0);
            checkPointer( touchPos );
        }
    }

    private void checkPointer(Vector3 pos)
    {
        shouldActivateInputContainer = gameState.GetComponent<Fase01_GameState>().States["Exploration"];
        RaycastHit hit; 
        Ray ray = explorerCamera.ScreenPointToRay(pos); 
        if ( Physics.Raycast (ray,out hit,100.0f)) 
        {
            string name = hit.transform.name;
            string tag = hit.transform.tag;

            //Debug.LogFormat("Hit.name: {0} Hit.tag: {1}", name, tag);
            
            if ((tag == "Player" && !UIElement.isVisible && canShow && !Fase01_CameraController.isLMoving && !Fase01_CameraController.isRMoving))
            {
                toggleInputContainer();
            }
        }
    }

    public void toggleInputContainer(){
        if (!UIElement.isVisible && shouldActivateInputContainer && canShow){
            UIElement.Show();
            Fase01_ChangePlayerPosition.canChangePosition = false;
            GameObject jc = Utils.GetChildWithName(references.Canvas, "Joysticks Container");
            jc.GetComponent<ToggleUIElement>().Hide();

        }
        else{
            UIElement.Hide();
            Fase01_ChangePlayerPosition.canChangePosition = true;
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
