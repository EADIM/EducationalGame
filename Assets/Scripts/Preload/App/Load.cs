using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour
{
    [SerializeField] private bool debug = true;
    [SerializeField] private int scene = 1;
    [SerializeField] private GameObject canvas;

    private void Awake() {
        DontDestroyOnLoad(this);

        if(debug)
        {
            if(canvas.activeSelf)
            {
                Utils.GetChildWithName(canvas, "FPS").GetComponent<ToggleUIElement>().Show();
            }
            SceneManager.LoadScene(scene);
            return;
        }
        
        SceneManager.LoadScene(1);
    }
}
