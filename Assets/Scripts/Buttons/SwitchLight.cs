using UnityEngine;

public class SwitchLight : MonoBehaviour, Interactable
{
    private GameObject lightGameObject;
    private void Start() {
        lightGameObject = transform.parent.GetComponentInChildren<Light>().gameObject;
    }

    public void interact()
    {
        lightGameObject.SetActive(!lightGameObject.activeInHierarchy);
    }
}
