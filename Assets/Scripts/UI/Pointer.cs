using UnityEngine;
using Lightbug.GrabIt;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    bool holding = false;
    private Image pointerImage;

    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite pointingSprite;
    [SerializeField] private Sprite holdingSprite;
    [SerializeField] private Sprite grabbableSprite;

    
    public enum pointingAt
    {
        nothing      = 0b_0000_0000,
        interactable = 0b_0000_0001,
        grabbable    = 0b_0000_0010
    }

    // Start is called before the first frame update
    void Start()
    {
        pointerImage = GetComponent<Image>();
        pointerImage.sprite = defaultSprite;
        
        //Subscribing to event calls.
        GrabIt.Grabbed += ShowObjectGrabbed;
        GrabIt.Released += ShowObjectReleased;
        MouseInteraction.PointedAt += ShowPointed;
    }

    void OnDestroy() {
        //It is a good practice to unsubscribe events when the objects are destroyed to avoid errors.
        GrabIt.Grabbed -= ShowObjectGrabbed;
        GrabIt.Released -= ShowObjectReleased;
        MouseInteraction.PointedAt -= ShowPointed;
    }

    void ShowObjectGrabbed()
    {
        holding = true;
        pointerImage.sprite = holdingSprite;
    }

    void ShowObjectReleased()
    {
        holding = false;
    }

    void ShowPointed(MouseInteraction.PointingAt pointingAt)
    {
        if(holding)
        {
            return;
        }
        if(pointingAt == MouseInteraction.PointingAt.interactable)
        {
            pointerImage.sprite = pointingSprite;
        }
        else if(pointingAt == MouseInteraction.PointingAt.grabbable)
        {
            pointerImage.sprite = grabbableSprite;
        }
        else{
            pointerImage.sprite = defaultSprite;
        }
    }
    
}
