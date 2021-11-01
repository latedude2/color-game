using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireSurface : MonoBehaviour, Activatable
{
    public float loadingSpeed = .01f;
    private Renderer _renderer;

    private void Start() {
        _renderer = GetComponent<Renderer>();
        loadingSpeed *= transform.localScale.x;
    }

    public void Activate()
    {
        StopAllCoroutines();
        StartCoroutine(SetLoad(true, loadingSpeed));
    }

    public void Deactivate()
    {
        StopAllCoroutines();
        StartCoroutine(SetLoad(false, loadingSpeed));
    }

    private IEnumerator SetLoad(bool load, float speed) {
        
        bool loading = true;
        int loadDir = load ? 1 : -1;
        while (loading) {
            Debug.Log(load);
            float progress = Mathf.Clamp(_renderer.material.GetFloat("Progress") + speed * loadDir, 0, 1);
            _renderer.material.SetFloat("Progress", progress);
            if (progress == Convert.ToInt16(load)) {
                loading = false;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
