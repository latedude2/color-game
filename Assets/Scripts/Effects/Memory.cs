using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Memory : MonoBehaviour
{
    public float memoryDuration = 5f;
    public string memoryText = "";

    GameObject player;
    RectTransform rectTransform;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<Canvas>().worldCamera = player.GetComponentInChildren<Camera>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        //point towards player
        Vector3 direction = player.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
        
    }
}
