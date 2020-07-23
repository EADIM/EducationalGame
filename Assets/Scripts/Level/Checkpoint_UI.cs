using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TMPro.TMP_Text))]
public class Checkpoint_UI : MonoBehaviour
{
    public SimulationMovement player_sm;

    void Update()
    {
        GetComponent<TMPro.TMP_Text>().text = (player_sm.checkpoints.Count - 1).ToString() + "/2";
    }
}
