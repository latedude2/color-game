using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseCollisionAudio : MonoBehaviour
{
    public FMODUnity.EventReference shatterEvent;
    public FMODUnity.EventReference collideEvent;

    public void PlayShatter(float force) {
        PlayOneShot(FMODUnity.RuntimeManager.CreateInstance(shatterEvent), force);
    }

    public void PlayCollision(float force) {
        PlayOneShot(FMODUnity.RuntimeManager.CreateInstance(collideEvent), force);
    }

    private void PlayOneShot(FMOD.Studio.EventInstance eventInstance, float force) {
        eventInstance.setParameterByName("Velocity Y", force);
        eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        eventInstance.start();
        eventInstance.release();
    }
}
