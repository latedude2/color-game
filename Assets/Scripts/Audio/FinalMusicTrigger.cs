using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMusicTrigger : MonoBehaviour {

    private void Start() {
        GetComponent<SetLightColor>().OnInteract += Interact;
    }

    public void Interact() {
        FindObjectOfType<MusicManager>().SwitchSoundtrack(4);
        GetComponent<SetLightColor>().enabled = false;
    }

    private void OnDestroy() {
        GetComponent<SetLightColor>().OnInteract -= Interact;
    }
}
