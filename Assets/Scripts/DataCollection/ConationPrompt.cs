using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class ConationPrompt : MonoBehaviour
{
    private string filename = "conation.txt";
    private float reportedConation = 4f; 

    private void Start() {
        Settings.UnlockCursor();    
    }
    public void ChangeValue(float newValue)
    {
        reportedConation = newValue;
    }

    public void SaveValue()
    {
        string directory = System.IO.Path.Combine(Application.persistentDataPath, GameManager.Instance.session.ToString().Replace("/", "-").Replace(":", "-"));
        if (!Directory.Exists(directory)) {
            //if it doesn't, create it
            Directory.CreateDirectory(directory);
        }
        string filepath = System.IO.Path.Combine(Application.persistentDataPath, directory, filename);
        StreamWriter writer = new StreamWriter(filepath, true);
        writer.WriteLine(reportedConation.ToString());
        writer.Close();
    }
}
