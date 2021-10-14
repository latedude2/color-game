using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{ 

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
            LevelManager.Instance.LoadNextLevel();
    }
}
