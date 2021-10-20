using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredLight : MonoBehaviour, Activatable
{
    [SerializeField] private bool enabledAtStart = false;
    [SerializeField] private ColorCode color = ColorCode.White;
    [SerializeField] private GameObject[] signifiers;

    LightBulb lightBulb;

    // Start is called before the first frame update
    void Start()
    {
        lightBulb = transform.parent.GetComponentInChildren<LightBulb>();
        SetColor(color);
        Invoke(nameof(SetInitialEnabled), 0.1f);
    }

    void SetInitialEnabled()
    {
        gameObject.SetActive(enabledAtStart);
        lightBulb.SetColor(color);
        lightBulb.SetActive(enabledAtStart);
    }

    public void SetColor(ColorCode newColor)
    {
        color = newColor;
        Light light = gameObject.GetComponent<Light>();
        light.color = ColorHelper.GetColor(newColor);

        foreach (var signifier in signifiers) {
            signifier.GetComponent<Renderer>().material.SetColor("_Color", light.color);
        }
    }

    public ColorCode GetColorCode()
    {
        return color;
    }

    public void Activate()
    {
        lightBulb.SetActive(true);
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        lightBulb.SetActive(false);
        gameObject.SetActive(false);
    }
}
