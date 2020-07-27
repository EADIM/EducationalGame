using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static GameObject GetChildWithName(GameObject obj, string name) {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null) {
            return childTrans.gameObject;
        } else {
            Debug.Log("Could not find " + name + " in " + obj.name);
            return null;
        }
    }

    public static void Hide(CanvasGroup canvasGroup) {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public static void Show(CanvasGroup canvasGroup) {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public static void Freeze(CanvasGroup canvasGroup){
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public static void Unfreeze(CanvasGroup canvasGroup){
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public bool checkIfObjectIsStill(GameObject obj){

        bool ObjIsMoving = false;


        StartCoroutine(
            CheckMoving(
                obj, //gameobject
                (i)=> { ObjIsMoving = i; } //callback function
            )
        );

        return ObjIsMoving;
    }

    private IEnumerator CheckMoving(GameObject gmobj, System.Action<bool> callback)
    {
        Vector3 startPos = gmobj.transform.position;

        yield return new WaitForSeconds(1f);

        Vector3 finalPos = gmobj.transform.position;

        if( startPos.x != finalPos.x ||
            startPos.y != finalPos.y ||
            startPos.z != finalPos.z)
            {
                callback(true);
            }
        
        yield return 0;
    }
}
