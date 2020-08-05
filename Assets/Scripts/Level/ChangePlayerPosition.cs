using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerPosition : MonoBehaviour
{
    public References references;
    private GameObject player;
    PlayerController sm;
    GameState gms;
    GetProblemInfo gpi;

    public string nameInitialPlatform01 = "Inferior Meio";
    public string nameInitialPlatform02 = "Meio Meio";
    public static bool canChangePosition = true;

    private void Start() {
        player = references.Player;
        sm = player.GetComponent<PlayerController>();
        gms = references.GameState.GetComponent<GameState>();
        gpi = references.GameState.GetComponent<GetProblemInfo>();
    }

    private void OnMouseDown() {
        Debug.Log("Clicou no collider do " + transform.parent.name);
        Debug.Log("Posição do Coliider: " + transform.position);

        if(gms.States[gms.getExplorationName()])
        {
            if(canChangePosition)
            {
                if(transform.parent.name == nameInitialPlatform01)
                {
                    sm.StartPlatformPosition = 0;
                }
                else if(transform.parent.name == nameInitialPlatform02)
                {
                    sm.StartPlatformPosition = 1;
                }

                if(sm.Checkpoints.Count > 0)
                {
                    Vector3 newPosition = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
                    player.GetComponent<PlayerController>().Checkpoints[0].setPosition(newPosition);
                    gpi.ColisorPlataformaInicial = transform.GetComponent<BoxCollider>();
                    gpi.OnIntialPlatformChange();
                }
                sm.ResetPosition(sm.Checkpoints[0]);
            }
        }
    }

    public void changeToFalse(){
        canChangePosition = false;
    }

    public void changeToTrue(){
        canChangePosition = true;
    }
}
