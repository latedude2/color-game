using UnityEngine;
using FMODUnity;
using UnityEngine.Events;

public class ActivatableNarration : MonoBehaviour, Activatable
{
    private StudioEventEmitter _audio;
    bool soundsEnabled = false;
    public UnityEvent OnDialogueEnd;

    void Awake() {
        _audio = GetComponent<StudioEventEmitter>();
    }

    private void Start() {
        // Avoid sounds from playing on new scene load
        Invoke(nameof(EnableSounds), .2f);
        DialogueManager.Instance.OnDialogueEnd.AddListener(DialogueEnd);
    }

    public void Activate() {
        if(soundsEnabled) {
            _audio.Play();
            DialogueManager.Instance.BeginDialogue(_audio.EventInstance);
            soundsEnabled = false;
        }
    }

    public void Deactivate() {
        return;
    }

    void EnableSounds() {
        soundsEnabled = true;
    }

    void DialogueEnd() {
        OnDialogueEnd.Invoke();
    }

    private void OnDestroy() {
        DialogueManager.Instance.OnDialogueEnd.RemoveListener(DialogueEnd);
    }
}
