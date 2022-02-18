using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSurface : MonoBehaviour, Activatable, Activater
{
    [Min(.001f)] public float loadingSpeed = 5f;
    [SerializeField] public List<GameObject> gameObjectsToActivate;
    private Renderer _renderer;
    private ParticleSystem sparks;
    public float progress;
    public ColorCode _color;
    private bool loading = false;
    private int loadDir;

    public Activater activater;

    private void Start() {
        SetLoadedColor();
        sparks = GetComponentInChildren<ParticleSystem>();
        loadingSpeed /= transform.localScale.x;
        _renderer.material.SetFloat("Progress", progress);
    }

    public void RemoveActivatable(GameObject activatableGameObject)
    {
        gameObjectsToActivate.Remove(activatableGameObject);
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
            sparks.Stop();
        }
        StopAllCoroutines();
        StartCoroutine(SetLoad(false, loadingSpeed));
    }

    private void FixedUpdate() {
        if (loading) {
            progress = Mathf.Clamp(_renderer.material.GetFloat("Progress") + loadingSpeed * loadDir * Time.deltaTime, 0, 1);
            _renderer.material.SetFloat("Progress", progress);
        }
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
        loading = true;
        loadDir = load ? 1 : -1;
        int loadInt = Convert.ToInt16(load);
        Vector3 sparksPos = sparks.transform.localPosition;
        sparks.Play();

        while (loading) {
            sparksPos.x = progress - 0.5f;
            sparks.transform.localPosition = sparksPos;
            if (progress == loadInt) {
                loading = false;
                if (load) {
                    SetActivatables(true);
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
        sparks.Stop();
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

    public void SetLoadedColor()
    {
        _renderer = GetComponent<Renderer>();
        var tempMaterial = new Material(_renderer.sharedMaterial);
        tempMaterial.SetColor("LoadedColor", ColorHelper.GetColor(_color));
        _renderer.sharedMaterial = tempMaterial;
    }
}
