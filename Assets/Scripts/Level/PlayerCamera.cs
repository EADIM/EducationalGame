using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObject))]
public class PlayerCamera : MonoBehaviour
{
    public Vector3 distance;
    public GameObject player;
    void Update()
    {
        transform.SetPositionAndRotation(
            new Vector3(
                player.transform.position.x + distance.x,
                player.transform.position.y + distance.y,
                player.transform.position.z + distance.z
            ),
            transform.rotation
        );
    }
}
