using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    private static Scene scene;

    private void Awake() {
        setCurrentScene();
    }

    public void Select(string levelName){
        SceneManager.LoadScene(levelName);

    }

    public void LoadGameLevel(string levelName){
        SceneName.NameOfTheScene = levelName;
        SceneManager.LoadScene("Loading Screen");
    }

    public static void setCurrentScene(){
        scene = SceneManager.GetActiveScene();
    }

    private void Update() {
        if(Input.GetKey(KeyCode.Escape) && scene.name == "LevelSelect"){
            Select("Menu");
        }
    }

    public static Scene GetCurrentScene(){
        return scene;
    }
}
