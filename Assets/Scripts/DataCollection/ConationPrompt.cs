using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class ConationPrompt : MonoBehaviour
{
    private string filename = "conation.txt";
    private string whyFilename = "whyConation.txt";

    [SerializeField] private GameObject whyConationCanvas;
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

    public void SaveWhyValue()
    {
        string directory = System.IO.Path.Combine(Application.persistentDataPath, GameManager.Instance.session.ToString().Replace("/", "-").Replace(":", "-"));
        if (!Directory.Exists(directory)) {
            //if it doesn't, create it
            Directory.CreateDirectory(directory);
        }
        string filepath = System.IO.Path.Combine(Application.persistentDataPath, directory, whyFilename);
        StreamWriter writer = new StreamWriter(filepath, true);
        
        string comment = GameObject.Find("WhyConationText").GetComponent<Text>().text;

        writer.WriteLine(comment);
        writer.WriteLine("----------------");
        writer.Close();
    }

    public void ShowWhyQuestion()
    {
        GameObject.Find("ConationCanvas").SetActive(false);
        whyConationCanvas.SetActive(true);
    }
}
