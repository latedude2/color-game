using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceResult : MonoBehaviour
{
    [SerializeField] Situation situation;
    // [SerializeField] Choice choice;
    // [SerializeField] GameObject[] gameObjectsToEnable;
    // [SerializeField] GameObject[] gameObjectsToDisable;

    public ChoiceGameObjects[] choiceGameObjects;

    [System.Serializable]
    public class ChoiceGameObjects
    {
        [SerializeField] public Choice choice;
        [SerializeField] public GameObject[] gameObjects;
    }

    void Start()
    {
        foreach (ChoiceGameObjects objects in choiceGameObjects)
        {
            if (objects.choice == ChoiceManager.GetChoice(situation)) {
                foreach(GameObject gameObject in objects.gameObjects)
                {
                    gameObject.SetActive(true);
                }
            } else {
                foreach(GameObject gameObject in objects.gameObjects)
                {
                    gameObject.SetActive(false);
                }
            }
        }
        // if(ChoiceManager.GetChoice(situation) == choice.ToString())
        // {
        //     foreach(GameObject gameObject in gameObjectsToEnable)
        //         gameObject.SetActive(true);
        //     foreach(GameObject gameObject in gameObjectsToDisable)
        //         gameObject.SetActive(false);
        // }
    }
}
