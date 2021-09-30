using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathfield : MonoBehaviour
{
    GameObject player;
    Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = GameObject.Find("SpawnPoint").transform;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject == player)
        {
            player.transform.position = spawnPoint.transform.position;
            player.transform.rotation = spawnPoint.transform.rotation;
        }
    }
}
