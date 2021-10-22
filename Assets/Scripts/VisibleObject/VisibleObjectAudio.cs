using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleObjectAudio : MonoBehaviour, Activatable
{
    private AudioSource _audioSource;
    private AudioClip _onSFX;
    private AudioClip _offSFX;
    [SerializeField] private AudioSource _audioLoop;

    bool soundsEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        _onSFX = (AudioClip)Resources.Load("Audio/SFX/VO ON");
        _offSFX = (AudioClip)Resources.Load("Audio/SFX/VO OFF");
        _audioSource = GetComponent<AudioSource>();
        Invoke(nameof(EnableSounds), 0.5f);
    }

    void EnableSounds()
    {
        soundsEnabled = true;
    }

    public void Activate()
    {
        if(soundsEnabled)
        {
            _audioSource.PlayOneShot(_onSFX);
            _audioLoop.time = UnityEngine.Random.value;
            _audioLoop.Play();
        }
    }

    public void Deactivate()
    {
        if(soundsEnabled)
        {
            _audioSource.PlayOneShot(_offSFX);
            _audioLoop.Stop();
        }
    }
}
