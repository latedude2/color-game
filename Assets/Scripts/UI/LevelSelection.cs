using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class LevelSelection : MonoBehaviour
{
    Image backgroundImage;

    public void SetBackgroundImage(Sprite sprite)
    {
        backgroundImage = GetComponent<Image>();
        backgroundImage.sprite = sprite;   
    }

}
