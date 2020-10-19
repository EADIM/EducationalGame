using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fase01_MapScaler : MonoBehaviour {

    [SerializeField] Fase01_References references;
    [SerializeField] GameObject Boundaries;
    [SerializeField] GameObject Floor;
    [SerializeField] GameObject Ceil;
    [SerializeField] GameObject Walls;
    [SerializeField] GameObject StartPlatform;
    [SerializeField] GameObject MidPlatform;
    [SerializeField] GameObject FinalPlatform;

    public void Scale(){
        float scale = GameManager.MapScale;

        Debug.LogFormat("Scale: {0:F2}", scale);

        Boundaries.transform.localScale = new Vector3(
            Boundaries.transform.localScale.x,
            Boundaries.transform.localScale.y,
            Boundaries.transform.localScale.z * scale
        );

        Floor.transform.localScale = new Vector3(   
            Floor.transform.localScale.x,        
            Floor.transform.localScale.y,
            scale
        );

        Ceil.transform.localScale = new Vector3(
            Ceil.transform.localScale.x,
            Ceil.transform.localScale.y,
            scale
        );

        Walls.transform.localScale = new Vector3(   
            Walls.transform.localScale.x,
            Walls.transform.localScale.y,
            scale
        );

        StartPlatform.transform.localPosition = new Vector3( 
            StartPlatform.transform.localPosition.x,
            StartPlatform.transform.localPosition.y,
            30 * (scale - 1)
        );

        MidPlatform.transform.position = new Vector3(  
            MidPlatform.transform.position.x,
            MidPlatform.transform.position.y,
            MidPlatform.transform.position.z * scale
        );

        FinalPlatform.transform.position = new Vector3( 
            FinalPlatform.transform.position.x,
            FinalPlatform.transform.position.y,
            FinalPlatform.transform.position.z * scale
        );

        Vector3 RaycastPosition = references.RaycastCenter.transform.position;
        Vector3 P_Jump = references.PlataformaPulo.gameObject.transform.position;
        references.RaycastCenter.transform.position = new Vector3(
            RaycastPosition.x,
            RaycastPosition.y,
            P_Jump.z - 4.0f
        );
        Fase01_RaycastFromAtoB rayA2B = references.GameState.GetComponent<Fase01_RaycastFromAtoB>();
        rayA2B.setStartPoint(references.RaycastCenter);
    
        Debug.LogFormat("Start Platform: ({0:F2}, {1:F2}, {2:F2})", StartPlatform.transform.position.x, StartPlatform.transform.position.y, StartPlatform.transform.position.z);

        Debug.LogFormat("Mid Platform: ({0:F2}, {1:F2}, {2:F2})", MidPlatform.transform.position.x, MidPlatform.transform.position.y, MidPlatform.transform.position.z);

        Debug.LogFormat("Final Platform: ({0:F2}, {1:F2}, {2:F2})", FinalPlatform.transform.position.x, FinalPlatform.transform.position.y, FinalPlatform.transform.position.z);
    }
}