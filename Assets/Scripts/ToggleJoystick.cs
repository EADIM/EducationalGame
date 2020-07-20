using UnityEngine;

public class ToggleJoystick : MonoBehaviour
{
    public bool isActive = false;
    public GameObject joystick;

    private void Start() {
        isActive = joystick.activeSelf;
    }

    public void HideShowJoystick(){
        if (isActive){
            joystick.SetActive(false);
            isActive = false;
        }
        else{
            joystick.SetActive(true);
            isActive = true;
        }
    }
}
