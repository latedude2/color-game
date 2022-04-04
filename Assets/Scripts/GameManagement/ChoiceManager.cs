using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    static public void SaveChoice(Situation situation, Choice choice)
    {
        Debug.Log("Saving choice: " + situation.ToString() + " as " + choice.ToString());
        PlayerPrefs.SetInt(situation.ToString(), (int)choice);
        PlayerPrefs.Save();
    }

    static public Choice GetChoice(Situation situation)
    {
        return (Choice)PlayerPrefs.GetInt(situation.ToString());
    }
}
