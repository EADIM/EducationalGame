using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickChangeGameState : MonoBehaviour
{

    public References references;

    private GameState gameState;

    private void Start() {
        gameState = references.GameState.GetComponent<GameState>();
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
