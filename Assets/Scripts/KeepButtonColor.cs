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
        checkIfJoystickIsActive();    
        button = gameObjectButton.GetComponent<Button>();
        colorBlock = button.colors;
        onButtonClick();
    }

    private void Update() {
        if(joystick != null){
            checkIfJoystickIsActive();
            onButtonClick();
        }
    }

    public void onButtonClick(){
        SetColors(checkIfButtonIsPressed());
    }

    private Color checkIfButtonIsPressed(){
        if (isPressed){
            return colorOnButtonPressed;
        }
        else{
            return colorOnButtonReleased;
        }
    }
    private void SetColors(Color color){
        colorBlock.normalColor = color;
        isPressed = !isPressed;

        if(button != null){
            button.colors = colorBlock;
        }
    }

    private void checkIfJoystickIsActive(){
        try{
            isPressed = joystick.activeSelf;
        }catch(UnassignedReferenceException){
            //Debug.Log("Joystick Null on " + this.ToString());
            isPressed = false;
        }
    }
}
