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

    void Start() {
        _renderer = GetComponent<Renderer>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            Activate();
        }
    }

    public void Activate() {
        if (switches.All(_switch => _switch.active)) {
            StopAllCoroutines();
            StartCoroutine(Animate());
        }
    }

    public void Deactivate() {
        StopAllCoroutines();
        _renderer.material.SetFloat("TailProgress", 0);
        _renderer.material.SetFloat("BodyProgress", 0);
    }

    private IEnumerator Animate() {
        bool loadTail = true;
        float tailProgress = 0;
        while (loadTail) {
            tailProgress = Mathf.Clamp(_renderer.material.GetFloat("TailProgress") + loadSpeed, 0, loadedTailValue);
            _renderer.material.SetFloat("TailProgress", tailProgress);
            if (tailProgress == loadedTailValue) {
                loadTail = false;
            }
            yield return new WaitForSeconds(0.01f);
        }
        bool loadBody = true;
        float bodyProgress = 0;
        while (loadBody) {
            bodyProgress = Mathf.Clamp(_renderer.material.GetFloat("BodyProgress") + loadSpeed, 0, loadedBodyValue);
            _renderer.material.SetFloat("BodyProgress", bodyProgress);
            if (bodyProgress == loadedBodyValue) {
                loadBody = false;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
