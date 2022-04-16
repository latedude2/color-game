using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public class AudioSettings : MonoBehaviour, Loadable
{
    private Bus music;
    private Bus sfx;

    private float defaultSfxVolume = 1;
    private float defaultMusicVolume = 1;

    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;

    void Start() {
        music = FMODUnity.RuntimeManager.GetBus("bus:/Gameplay/Music");
        sfx = FMODUnity.RuntimeManager.GetBus("bus:/Gameplay/SFX");
    }

    public void SetMusicVolume(float value)
    {
        musicSlider.value = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        music.setVolume(value);
        PlayerPrefs.Save();
    }

    public void SetSfxVolume(float value)
    {
        sfxSlider.value = value;
        PlayerPrefs.SetFloat("SfxVolume", value);
        sfx.setVolume(value);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolume));
        SetSfxVolume(PlayerPrefs.GetFloat("SfxVolume", defaultSfxVolume));
    }
}
