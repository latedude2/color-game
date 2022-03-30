using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

// Thanks to Random Seed Games for sharing their implementation
// https://www.youtube.com/watch?v=1NW0BYn5KfE&ab_channel=RandomSeedGames

public class DialogueManager : MonoBehaviour
{

    private string[] fileLines;

    // Subtitle variables
    private List<string> subtitleLines = new List<string>();
    private List<string> subtitleTimingStrings = new List<string>();
    public List<float> subtitleTimings = new List<float>();
    public List<string> subtitleText = new List<string>();
    private int nextSubtitle = 0;
    private string displaySubtitle;

    // Trigger variables
    private List<string> triggerLines = new List<string>();
    private List<string> triggerTimingStrings = new List<string>();
    public List<float> triggerTimings = new List<float>();
    private List<string> triggers = new List<string>();
    public List<string> triggerObjectNames = new List<string>();
    public List<string> triggerMethodNames = new List<string>();
    private int nextTrigger = 0;

    // Singleton property
    public static DialogueManager Instance { get; private set;}

    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        Instance = this;
    }

    // Update is called once per frame
    public void BeginDialogue(string dialogueFileName)
    {
        // Reset variables
        subtitleLines = new List<string>();
        subtitleTimingStrings = new List<string>();
        subtitleTimings = new List<float>();
        subtitleText = new List<string>();
        triggerLines = new List<string>();
        triggerTimingStrings = new List<string>();
        triggerTimings = new List<float>();
        triggers = new List<string>();
        triggerObjectNames = new List<string>();
        triggerMethodNames = new List<string>();
        nextSubtitle = 0;
        nextTrigger = 0;

        // Get data from text file
        TextAsset temp = Resources.Load<TextAsset>("Subtitles/" + dialogueFileName);
        fileLines = temp.text.Split('\n');

        // Split subtitle lines and triggers
        foreach (string line in fileLines)
        {
            if (line.Contains("<trigger/>"))
            {
                triggerLines.Add(line);
            }
            else
            {
                subtitleLines.Add(line);
            }
        }

        // Split subtitle elements
        for (int i = 0; i < subtitleLines.Count; i++)
        {
            string[] splitTemp = subtitleLines[i].Split('|');
            subtitleTimingStrings.Add(splitTemp[0]);
            Debug.Log(CleanTimeString(subtitleTimingStrings[i]));
            subtitleTimings.Add(float.Parse(CleanTimeString(subtitleTimingStrings[i])));
            subtitleText.Add(splitTemp[1]);
        }

        //Split trigger elements
        for (int i = 0; i < triggerLines.Count; i++)
        {
            string[] splitTemp1 = triggerLines[i].Split('|');
            triggerTimingStrings.Add(splitTemp1[0]);
            triggerTimings.Add(float.Parse(CleanTimeString(triggerTimingStrings[i])));
            triggers.Add(splitTemp1[1]);
            string[] splitTemp2 = triggers[i].Split('-');
            splitTemp2[0] = splitTemp2[0].Replace("<trigger/>", "");
            triggerObjectNames.Add(splitTemp2[0]);
            triggerMethodNames.Add(splitTemp2[1]);
        }

        // Set initial text
        if (subtitleText[0] != null)
        {
            displaySubtitle = subtitleText[0];
        }
    }

    // Remove all non-float characters from timing
    private string CleanTimeString(string timeString)
    {
        Regex digitsOnly = new Regex(@"[^\d+(\.\d+)*S]");
        return digitsOnly.Replace(timeString, "");
    }
}
