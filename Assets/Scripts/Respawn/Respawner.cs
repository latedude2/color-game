using UnityEngine;
using Lightbug.GrabIt;

public class Respawner : MonoBehaviour
{
    private Vector3 spawnPoint;

    private void Start() {
        GrabIt.Released += SaveSpawnPoint;
        PressurePlate.Pressed += SaveSpawnPoint;
        InvisibleTrigger.Triggered += SaveSpawnPoint;
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

    private void FixedUpdate() {
        CheckOutOfBounds();
    }

    private void CheckOutOfBounds()
    {
        if (gameObject.transform.position.y < -5.0f)
        {
            this.gameObject.transform.position = spawnPoint;
        }
    }

    private void OnDestroy() {
        GrabIt.Released -= SaveSpawnPoint;
        PressurePlate.Pressed -= SaveSpawnPoint;
        InvisibleTrigger.Triggered -= SaveSpawnPoint;
    }
}
