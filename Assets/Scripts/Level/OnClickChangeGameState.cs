using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickChangeGameState : MonoBehaviour
{

    public GameObject gameStateObject;
    private GameState gameState;

    private void Start() {
        gameState = gameStateObject.GetComponent<GameState>();
    }

    public void changeState(){

        if(gameState.States["Exploration"]){
            gameState.SwitchState("Simulation");
        }
        else if (gameState.States["Simulation"]){
            gameState.SwitchState("Exploration");
        }
        
    }
}
