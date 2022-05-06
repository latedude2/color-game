using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour, Activatable
{
    public float delay = 2.5f;
    bool activated = false;

    private void OnTriggerStay(Collider other) {
        if (activated) {
            StartCoroutine(EndTheLevel());
            activated = false;
        }
    }

    IEnumerator EndTheLevel() {
        yield return new WaitForSeconds(delay);
        LevelManager.Instance.LoadNextLevel();
    }

    public void Activate()
    {
        activated = true;
    }

    public void Deactivate(){}
}
