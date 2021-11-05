using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSurface : MonoBehaviour, Activatable
{
    [Min(.001f)] public float loadingSpeed = .01f;
    [SerializeField] GameObject[] gameObjectsToActivate;
    private Renderer _renderer;
    public float progress;
    public ColorCode _color;

    private void Start() {
        _renderer = GetComponent<Renderer>();
        loadingSpeed /= transform.localScale.x;
        _renderer.material.SetColor("LoadedColor", ColorHelper.GetColor(_color));
        _renderer.material.SetFloat("Progress", progress);
        if(gameObjectsToActivate.Length == 0)
        {
            Debug.LogError("Objects that should be activated was not set.");
        }
    }

    public void Activate()
    {
        StopAllCoroutines();
        StartCoroutine(SetLoad(true, loadingSpeed));
    }

    public void Deactivate()
    {
        if (_renderer.material.GetFloat("Progress") == 1) {
            SetActivatables(false);
        }
        StopAllCoroutines();
        StartCoroutine(SetLoad(false, loadingSpeed));
    }

    private IEnumerator SetLoad(bool load, float speed) {
        bool waiting = true;
        if (!load) {
            while (waiting) {
                float sum = 0;
                foreach (GameObject gameObjectToActivate in gameObjectsToActivate) {
                    if (gameObjectToActivate.TryGetComponent<WireSurface>(out WireSurface wire)) {
                        sum += wire.progress;
                    }
                }
                if (sum == 0)
                    waiting = false;
                else
                    yield return new WaitForSeconds(0.01f);
            }
        }
        bool loading = true;
        int loadDir = load ? 1 : -1;
        int loadInt = Convert.ToInt16(load);

        while (loading) {
            progress = Mathf.Clamp(_renderer.material.GetFloat("Progress") + speed * loadDir, 0, 1);
            _renderer.material.SetFloat("Progress", progress);
            if (progress == loadInt) {
                loading = false;
                if (load) {
                    SetActivatables(true);
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void SetActivatables(bool activate) {
        foreach(GameObject gameObjectToActivate in gameObjectsToActivate) {
            Activatable activatable = gameObjectToActivate.GetComponent<Activatable>();
            if(activatable != null)
                if(activate)
                    activatable.Activate();
                else
                    activatable.Deactivate();
            else
                Debug.LogError("Object is missing activatable component.");
        }
    }
}