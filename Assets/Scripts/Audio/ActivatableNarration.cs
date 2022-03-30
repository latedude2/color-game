using UnityEngine;
using FMODUnity;

public class ActivatableNarration : MonoBehaviour, Activatable
{
    private StudioEventEmitter _audio;
    bool soundsEnabled = false;

    void Awake() {
        _audio = GetComponent<StudioEventEmitter>();
    }

    private void Start() {
        // Avoid sounds from playing on new scene load
        Invoke(nameof(EnableSounds), .2f);
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
}
