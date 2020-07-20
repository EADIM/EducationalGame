using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUIElement : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public bool canShow = true;
    public void Hide() {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public void Show() {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
}
