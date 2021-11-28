using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMusicTrigger : MonoBehaviour {

    private void Start() {
        GetComponent<SwitchLightButton>().OnInteract += Interact;
    }

    public void Interact() {
        FindObjectOfType<MusicManager>().NextSoundtrack();
        GetComponent<SwitchLightButton>().enabled = false;
    }
}
