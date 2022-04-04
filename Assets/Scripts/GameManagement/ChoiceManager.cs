using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    static public void SaveChoice(Situation situation, Choice choice)
    {
        Debug.Log("Saving choice: " + situation.ToString() + " as " + choice.ToString());
        PlayerPrefs.SetString(situation.ToString(), choice.ToString());
        PlayerPrefs.Save();
    }

    static public string GetChoice(Situation situation)
    {
        return PlayerPrefs.GetString(situation.ToString());
    }
}
