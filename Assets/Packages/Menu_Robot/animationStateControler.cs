using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateControler : MonoBehaviour
{
    Animator animator;
    int touchedHash;

    [SerializeField]
    private bool debug;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        touchedHash = Animator.StringToHash("touchScreen");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;
            bool isActing = animator.GetBool(touchedHash);

            if(Physics.Raycast(raycast, out hit))
            {
                if(hit.collider.name == "Jammo_Player (1)" && !isActing)
                {
                    animator.SetBool(touchedHash, true);
                }
            }
        }
        else if (animator.GetBool(touchedHash))
        {
            animator.SetBool(touchedHash, false);
        }

        /*#if UNITY_EDITOR
                if(Input.GetMouseButtonDown(0))
                {
                    Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    bool isActing = animator.GetBool(touchedHash);

                    if(Physics.Raycast(raycast, out hit))
                    {
                        if(hit.collider.name == "Jammo_Player (1)" && !isActing)
                        {
                            animator.SetBool(touchedHash, true);
                            Debug.Log("Clicou no robo litle!!!!!");
                        }
                    }  
                }
                else if (animator.GetBool(touchedHash))
                {
                    animator.SetBool(touchedHash, false);
                    Debug.Log("Desclicou no robo litle!!!!!"); 
                }
                
        #endif*/
    }
}
