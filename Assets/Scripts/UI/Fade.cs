using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fade : MonoBehaviour
{
    public Sprite topFade;
    public Sprite middleFade;
    public Sprite bottomFade;
    public GameObject Content;
    public ScrollRect myScrollRect;
    Image mask;
    float scrollPosition;

    // Use this for initialization
    public void SetImage(Sprite maskSprite)
    {
        mask = GetComponent<Image>();
        mask.sprite = maskSprite;
    }
    private void Update()
    {
        scrollPosition=myScrollRect.verticalNormalizedPosition;
        if (scrollPosition < 1 && scrollPosition > 0 )
        {
           SetImage(middleFade);
        }
        else if(scrollPosition==1)
        {
            SetImage(bottomFade);
        }
        else if(scrollPosition == 0)
        {
            SetImage(topFade);
        }
    }
}
