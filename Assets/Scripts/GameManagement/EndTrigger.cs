using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{ 
    [Tooltip("Use this for specifying to load any other level than the next one in the build settings. Intended to be used for levels with multiple exits. ")]
    [SerializeField] string customLevelName = "";


    [Tooltip("Example: Informing people")]
    [SerializeField] string choiceName = "";

    [Tooltip("Example: side with blue")]
    [SerializeField] string choiceSolution = "";

    private void OnTriggerEnter(Collider other) {
        if(choiceName != "" && choiceSolution != "")
        {
            ChoiceManager.SaveChoice(choiceName, choiceSolution);
        }
        if (other.tag == "Player")
        {
            if(customLevelName != "")
            {
                LevelManager.Instance.LoadLevel(customLevelName);
            }
            else LevelManager.Instance.LoadNextLevel();
        }
    }
}
