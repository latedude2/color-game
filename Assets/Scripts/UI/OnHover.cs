using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnHover : MonoBehaviour, IPointerEnterHandler
{
    public GameObject targetImage;
    public Sprite backgroundImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetImage.GetComponent<LevelSelection>().SetBackgroundImage(backgroundImage);
    }
}
