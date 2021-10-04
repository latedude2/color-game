using UnityEngine;
using Lightbug.GrabIt;

public class Respawner : MonoBehaviour
{
    private Vector3 spawnPoint;

    private void Start() {
        GrabIt.Released += SaveSpawnPoint;
        SaveSpawnPoint();
    }

    public void SaveSpawnPoint()
    {
        spawnPoint = this.gameObject.transform.position;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Respawn")
        {
            this.gameObject.transform.position = spawnPoint;
        }
    }

    private void OnDestroy() {
        GrabIt.Released -= SaveSpawnPoint;
    }
}
