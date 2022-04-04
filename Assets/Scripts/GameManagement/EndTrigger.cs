using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{ 
    [Tooltip("Use this for specifying to load any other level than the next one in the build settings. Intended to be used for levels with multiple exits. ")]
    [SerializeField] string customLevelName = "";


    [Tooltip("Example: Situation1")]
    [SerializeField] Situation situation = Situation.Unset;

    [Tooltip("Example: side with blue")]
    [SerializeField] Choice choice = Choice.Unset;

    private void OnTriggerEnter(Collider other) {
        if(situation != Situation.Unset && choice != Choice.Unset)
        {
            ChoiceManager.SaveChoice(situation, choice);
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
