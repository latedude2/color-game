using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceResult : MonoBehaviour
{
    [SerializeField] Situation situation;
    [SerializeField] Choice choice;
    [SerializeField] GameObject[] gameObjectsToEnable;
    [SerializeField] GameObject[] gameObjectsToDisable;
    void Start()
    {
        if(ChoiceManager.GetChoice(situation) == choice.ToString())
        {
            foreach(GameObject gameObject in gameObjectsToEnable)
                gameObject.SetActive(true);
            foreach(GameObject gameObject in gameObjectsToDisable)
                gameObject.SetActive(false);
        }
    }
}
