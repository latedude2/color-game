using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PeacockAnimation : MonoBehaviour, Activatable {
    public List<LightSwitch> switches;
    public float loadedTailValue = 10;
    public float loadedBodyValue = 20;
    public float loadSpeed = .01f;
    private Renderer _renderer;
    private bool loadTail = false;
    private bool loadBody = false;
    private float tailProgress;
    private float bodyProgress;
    public ParticleSystem particles;

    void Start() {
        _renderer = GetComponent<Renderer>();
    }

    private void Update() {
        
    }

    public void Activate() {
        print(switches.All(_switch => _switch.active));
        if (switches.All(_switch => _switch.active)) {
            StopAllCoroutines();
            StartCoroutine(Animate());
        }
    }

    public void Deactivate() {
        StopAllCoroutines();
        _renderer.material.SetFloat("TailProgress", 0);
        _renderer.material.SetFloat("BodyProgress", 0);
        loadTail = false;
        loadBody = false;
        particles.Stop();
    }

    private void FixedUpdate() {
        if (loadTail) {
            tailProgress = Mathf.Clamp(_renderer.material.GetFloat("TailProgress") + loadSpeed * Time.deltaTime, 0, loadedTailValue);
            _renderer.material.SetFloat("TailProgress", tailProgress);
        }
        if (loadBody) {
            bodyProgress = Mathf.Clamp(_renderer.material.GetFloat("BodyProgress") + loadSpeed * Time.deltaTime, 0, loadedBodyValue);
            _renderer.material.SetFloat("BodyProgress", bodyProgress);
        }
    }

    private IEnumerator Animate() {
        particles.Play();
        loadTail = true;
        tailProgress = 0;
        while (loadTail) {
            if (tailProgress == loadedTailValue) {
                loadTail = false;
            }
            yield return new WaitForSeconds(0.01f);
        }
        _renderer.material.SetFloat("BodyProgress", 5);
        loadBody = true;
        bodyProgress = 5;
        while (loadBody) {
            if (bodyProgress == loadedBodyValue) {
                loadBody = false;
            }
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1f);
        LevelManager.Instance.LoadNextLevel(2.5f);
    }
}
