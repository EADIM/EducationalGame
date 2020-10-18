using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fase01_OnClickChangeGameState : MonoBehaviour
{

    public fase01_References references;

    private fase01_GameState gameState;

    private void Start() {
        gameState = references.GameState.GetComponent<fase01_GameState>();
    }

    public void changeState(){

        if(gameState.States[gameState.getExplorationName()]){
            gameState.SwitchState(gameState.getSimulationName());
        }
        else if (gameState.States[gameState.getSimulationName()]){
            gameState.SwitchState(gameState.getExplorationName());
        }
    }
}
