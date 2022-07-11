using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Memory : MonoBehaviour
{
    public float memoryDuration = 5f;
    public string memoryText = "";
    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponentInChildren<TextMeshProUGUI>().text = memoryText;
    }

    void Update()
    {
        transform.LookAt(player);
        memoryDuration -= Time.deltaTime;
        if(memoryDuration < 0)
        {
            Destroy(gameObject);
        }
    }
}
