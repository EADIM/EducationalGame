using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fase01_OnClickChangeGameState : MonoBehaviour
{

    public Fase01_References references;

    private Fase01_GameState gameState;

    private void Start() {
        gameState = references.GameState.GetComponent<Fase01_GameState>();
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
