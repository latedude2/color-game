using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class ActivatableSound : MonoBehaviour, Activatable
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
            if (_audio.Params.Length > 0) {
                _audio.Params[0].Value = 1;
            }
            _audio.Play();
        }
    }

    public void Deactivate(){
        if (soundsEnabled) {
            if (_audio.Params.Length > 0) {
                _audio.Params[0].Value = 0;
            }
            _audio.Play();
        }
    }

    void EnableSounds() {
        soundsEnabled = true;
    }
}
