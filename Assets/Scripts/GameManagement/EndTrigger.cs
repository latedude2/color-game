using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{ 
    [SerializeField] string customLevelName = "";

    private void OnTriggerEnter(Collider other) {
        if(customLevelName != "")
        {
            LevelManager.Instance.LoadLevel(customLevelName);
        }
        else if (other.tag == "Player")
            LevelManager.Instance.LoadNextLevel();
    }
}
