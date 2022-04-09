using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class AudioSettings : Loadable
{
    private Bus music;
    private Bus sfx;

    public AudioSettings() {
        music = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        sfx = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        music.setVolume(value);
    }

    public void SetSfxVolume(float value)
    {
        PlayerPrefs.SetFloat("SfxVolume", value);
        sfx.setVolume(value);
    }

    public void Load()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
        SetSfxVolume(PlayerPrefs.GetFloat("SfxVolume"));
    }
}
