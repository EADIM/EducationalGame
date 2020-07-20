using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeepButtonColor : MonoBehaviour
{
    public GameObject gameObjectButton;
    public GameObject joystick = null;
    private Button button = null;
    private ColorBlock colorBlock;
    public Color colorOnButtonPressed;
    public Color colorOnButtonReleased;
    public bool isPressed = false;

    void Start(){
        checkIfIsPressed();    
        button = gameObjectButton.GetComponent<Button>();
        colorBlock = button.colors;
        onButtonClick();
    }

    public void onButtonClick(){
        if (isPressed){
            isPressed = false;
            colorBlock.normalColor = colorOnButtonPressed;
        }
        else{
            isPressed = true;
            colorBlock.normalColor = colorOnButtonReleased;
        }

        if(button != null){
            button.colors = colorBlock;
        }
    }

    private void checkIfIsPressed(){
        try{
            isPressed = joystick.activeSelf;
        }catch(UnassignedReferenceException){
            Debug.Log("Joystick Null on " + this.ToString());
        }
    }
}
