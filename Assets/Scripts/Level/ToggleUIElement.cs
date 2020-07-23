using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUIElement : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public bool isVisible = true;
    private bool shouldShow = true;
    
    private void Start() {
        if (canvasGroup.alpha > 0){
            isVisible = true;
            shouldShow = true;
        }
        else{
            isVisible = false;
            shouldShow = false;
        }
    }

    public void Hide() {
        Utils.Hide(canvasGroup);
        isVisible = false;
    }

    public void Show() {
        Utils.Show(canvasGroup);
        isVisible = true;
    }

    public void Toggle(){
        if(shouldShow){
            Show();
            shouldShow = false;
        }
        else{
            Hide();
            shouldShow = true;
        }
    }
}
