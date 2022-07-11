using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Memory : MonoBehaviour
{
    public float memoryDuration = 5f;
    private float currentMemoryDuration = 0f;
    public string memoryText = "";
    Transform player;

    TextMeshProUGUI text;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = memoryText;
        currentMemoryDuration = memoryDuration;
    }

    void Update()
    {
        transform.LookAt(player);
        currentMemoryDuration -= Time.deltaTime;
        text.color = new Color(text.color.r, text.color.g, text.color.b, currentMemoryDuration / memoryDuration);
        if(currentMemoryDuration < 0)
        {
            Destroy(gameObject);
        }
    }
}
