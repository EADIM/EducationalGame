using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fase01_ChangePlayerPosition : MonoBehaviour
{
    public fase01_References references;
    
    GameObject player;
    GetProblemInfo gpi;
    fase01_PlayerController pc;
    fase01_GameState gms;

    public readonly string nameInitialPlatform01 = "Inferior Meio";
    public readonly string nameInitialPlatform02 = "Meio Meio";
    public static bool canChangePosition = true;

    private void Awake() {
        player = references.Player;
    }

    private void Start() {
        pc = player.GetComponent<fase01_PlayerController>();
        gms = references.GameState.GetComponent<fase01_GameState>();
        gpi = references.GameState.GetComponent<GetProblemInfo>();
    }

    private void OnMouseDown() {
        //Debug.Log("Clicou no collider do " + transform.parent.name);
        //Debug.Log("Posição do Coliider: " + transform.position);

        if(gms.States[gms.getExplorationName()])
        {
            if(canChangePosition)
            {
                if(transform.parent.name == nameInitialPlatform01)
                {
                    pc.StartPlatformPosition = 0;
                }
                else if(transform.parent.name == nameInitialPlatform02)
                {
                    pc.StartPlatformPosition = 1;
                }

                if(pc.Checkpoints.Count > 0)
                {
                    SetNewPosition();
                }
                pc.ResetPosition(pc.Checkpoints[0]);
            }
        }
    }

    public void SetNewPosition()
    {
        //Debug.LogFormat("Player position: {0} Collider: {1}", player.transform, transform);

        Vector3 newPosition = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        pc.Checkpoints[0].setPosition(newPosition);
        gpi.ColisorPlataformaInicial = transform.GetComponent<BoxCollider>();
        gpi.OnIntialPlatformChange();
    }

    public void changeToFalse(){
        canChangePosition = false;
    }

    public void changeToTrue(){
        canChangePosition = true;
    }
}
