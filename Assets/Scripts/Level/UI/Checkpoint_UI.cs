using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TMPro.TMP_Text))]
public class Checkpoint_UI : MonoBehaviour
{
    public fase01_References references;
    
    private fase01_PlayerController player_sm;

    private void Start() {
        player_sm = references.Player.GetComponent<fase01_PlayerController>();
    }

    void Update()
    {
        GetComponent<TMPro.TMP_Text>().text = ( ( (player_sm.Checkpoints.Count - 1) < 0 ) ? 0 : player_sm.Checkpoints.Count - 1 ).ToString() + "/2";
    }
}
