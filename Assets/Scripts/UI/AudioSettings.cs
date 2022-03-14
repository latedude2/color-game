using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class AudioSettings
{
    private Bus music;
    private Bus sfx;

    public AudioSettings() {
        music = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        sfx = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
    }

    public void SetMusicVolume(float value)
    {
        music.setVolume(value);
    }

    public void SetSfxVolume(float value)
    {
        sfx.setVolume(value);
    }
}
