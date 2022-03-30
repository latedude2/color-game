using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManagerTest : MonoBehaviour
{

    public string dialogueEvent;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // DialogueManager.Instance.BeginDialogue(dialogueEvent);
        }
    }
}
