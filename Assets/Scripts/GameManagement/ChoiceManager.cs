using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    static public void SaveChoice(string choiceName, string choiceSolution)
    {
        PlayerPrefs.SetString(choiceName, choiceSolution);
        PlayerPrefs.Save();
    }

    static public string GetChoice(string choiceName)
    {
        return PlayerPrefs.GetString(choiceName);
    }
}
