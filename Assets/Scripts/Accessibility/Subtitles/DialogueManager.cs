using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using TMPro;
using FMOD.Studio;

// Thanks to Random Seed Games for sharing their implementation
// https://www.youtube.com/watch?v=1NW0BYn5KfE&ab_channel=RandomSeedGames

[RequireComponent(typeof(TMP_Text))]
public class DialogueManager : MonoBehaviour
{
    
    private TMP_Text subtitleUI;
    private float lineStartTime;
    private bool isDisplaying = false;
    private EventInstance fmodEvent;

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

    private void Start()
    {
        subtitleUI = GetComponent<TMP_Text>();
        subtitleUI.SetText("");
    }

    private void Update()
    {
        if (!isDisplaying)
            return;

        // Check if dialogue has finished
        FMOD.Studio.PLAYBACK_STATE playbackState;    
        fmodEvent.getPlaybackState(out playbackState);
        if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED) {
            EndDialogue();
            return;
        }
        
        // Check for <break/> tag or negative nextSubtitle
        if (nextSubtitle > 0 && !subtitleText[nextSubtitle - 1].Contains("<break/>"))
        {
            subtitleUI.SetText(displaySubtitle);
        }

        // Increment nextSubtitle when passing time point
        if (nextSubtitle < subtitleText.Count)
        {
            if (Time.time - lineStartTime > subtitleTimings[nextSubtitle])
            {
                displaySubtitle = subtitleText[nextSubtitle];
                nextSubtitle++;
            }
        }

        // Fire triggers when passing time point
        if (nextTrigger < triggers.Count)
        {
            if (Time.time - lineStartTime > triggerTimings[nextSubtitle])
            {
                GameObject obj = GameObject.Find(triggerObjectNames[nextTrigger]);
                if (obj != null) {
                    obj.SendMessage(triggerMethodNames[nextTrigger]);
                }
                nextTrigger++;
            }
        }
    }

    public void BeginDialogue(FMOD.Studio.EventInstance instance)
    {
        fmodEvent = instance;
        isDisplaying = true;

        // Reset variables
        lineStartTime = Time.time;
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
        TextAsset temp = Resources.Load<TextAsset>("Subtitles/" + GetEventPath());
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

    public void EndDialogue() {
        isDisplaying = false;
        subtitleUI.SetText("");
    }

    string GetEventPath() {
        string result;
        FMOD.Studio.EventDescription description;

        // Get path in the form event:/folder/sub-folder/eventName
        fmodEvent.getDescription(out description);
        description.getPath(out result);

        // remove event:/Dialogue/
        if (result.Contains("event:/Dialogue/"))
            return result.Remove(0,16);
        else {
            Debug.LogWarning("The FMOD Event you are trying to reference is not in Dialogue folder.");
            return null;
        }
    }

    // Remove all non-float characters from timing
    private string CleanTimeString(string timeString)
    {
        Regex digitsOnly = new Regex(@"[^\d+(\.\d+)*S]");
        return digitsOnly.Replace(timeString, "");
    }
}
