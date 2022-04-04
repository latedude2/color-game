using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceResult : MonoBehaviour
{
    [SerializeField] Situation situation;

    // TODO: Find better naming
    public ChoiceGameObjects[] choiceGameObjects;

    [System.Serializable]
    public class ChoiceGameObjects
    {
        // TODO: Find a way to make these private while still showing them in inspector
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
    }
}
