using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ActivatableSound : MonoBehaviour, Activatable
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _onSFX;
    [SerializeField] private AudioClip _offSFX;
    [SerializeField] private GameObject LoopPrefab;
    [SerializeField] private AudioSource _audioLoop;

    bool soundsEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        // Audio source that plays on/off sounds
        _audioSource = GetComponent<AudioSource>();

        // Loop that plays while object is active
        if (LoopPrefab != null) {
            Instantiate(LoopPrefab, transform).TryGetComponent<AudioSource>(out _audioLoop);
        }

        // Disable sounds when loading levels
        Invoke(nameof(EnableSounds), .2f);
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
        }
        Loop();
    }

    public void Deactivate()
    {
        if(soundsEnabled)
        {
            _audioSource.PlayOneShot(_offSFX);
        }
        _audioLoop.Stop();
    }

    void Loop() {
        if (_audioLoop != null) {
            _audioLoop.time = UnityEngine.Random.value;
            _audioLoop.Play();
        }
    }
}
