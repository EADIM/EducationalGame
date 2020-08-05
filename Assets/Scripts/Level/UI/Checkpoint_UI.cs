using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TMPro.TMP_Text))]
public class Checkpoint_UI : MonoBehaviour
{
    public References references;
    
    private PlayerController player_sm;

    private void Start() {
        player_sm = references.Player.GetComponent<PlayerController>();
    }

    void Update()
    {
        GetComponent<TMPro.TMP_Text>().text = ( ( (player_sm.Checkpoints.Count - 1) < 0 ) ? 0 : player_sm.Checkpoints.Count - 1 ).ToString() + "/2";
    }
}
