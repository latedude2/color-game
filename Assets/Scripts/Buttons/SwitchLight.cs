using UnityEngine;

public class SwitchLight : MonoBehaviour, Interactable
{
    private GameObject lightGameObject;
    private AudioClip onClip;
    private AudioClip offClip;


    private void Start() {
        lightGameObject = transform.parent.GetComponentInChildren<Light>().gameObject;
        onClip = (AudioClip) Resources.Load("Audio/SFX/SwitchOn");
        offClip = (AudioClip) Resources.Load("Audio/SFX/SwitchOff");
    }

    public void interact()
    {
        bool isActive = lightGameObject.activeInHierarchy;
        lightGameObject.SetActive(!isActive);
        transform.parent.GetComponentInChildren<LightBulb>().SetActive(!isActive);
        if(isActive) {
            AudioSource.PlayClipAtPoint(offClip, GetComponent<Transform>().position);
        } else {
            AudioSource.PlayClipAtPoint(onClip, GetComponent<Transform>().position);
        }
    }

    private void initializeAudio() {

    }
}
