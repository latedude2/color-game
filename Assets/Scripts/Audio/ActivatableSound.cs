using UnityEngine;
using FMODUnity;

public class ActivatableSound : MonoBehaviour, Activatable
{
    private StudioEventEmitter _audio;
    bool initiated = false;

    void Awake() {
        _audio = GetComponent<StudioEventEmitter>();
    }

    public void Activate() {
        if(!initiated)
            Initiate();
        else {
            if (_audio.Params.Length > 0) {
                _audio.Params[0].Value = 1;
                _audio.Play();
            }
        }
    }

    private void Initiate() {
        initiated = true;
    }

    public void Deactivate(){
        if (_audio.Params.Length > 0) {
            _audio.Params[0].Value = 0;
            _audio.Play();
        }
    }
}
