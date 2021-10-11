using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField]
    float turnSpeed = 80.0f;

    private void Start() {
        DisableCollisionWithPlayer();
    }

    private void DisableCollisionWithPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
             Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

    public float GetTurnSpeed()
    {
        return turnSpeed;
    }
}
