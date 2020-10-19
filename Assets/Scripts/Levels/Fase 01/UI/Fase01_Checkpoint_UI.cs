using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TMPro.TMP_Text))]
public class Fase01_Checkpoint_UI : MonoBehaviour
{
    public Fase01_References references;
    
    private Fase01_PlayerController player_sm;

    private void Start() {
        player_sm = references.Player.GetComponent<Fase01_PlayerController>();
    }

    void Update()
    {
        GetComponent<TMPro.TMP_Text>().text = ( ( (player_sm.Checkpoints.Count - 1) < 0 ) ? 0 : player_sm.Checkpoints.Count - 1 ).ToString() + "/2";
    }
}
