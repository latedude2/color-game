using UnityEngine;
using UnityEngine.UI;

public class ChoiceSubtitle : MonoBehaviour
{
    [SerializeField] string textToShow = "Missing subtitle lol";
    Text textComponent;
    Animation panelAnimation;

    private void Start() {
        textComponent = GameObject.Find("ChoiceSubtitlePanel/ChoiceSubtitle").GetComponent<Text>();
        panelAnimation = GameObject.Find("ChoiceSubtitlePanel").GetComponent<Animation>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            panelAnimation.Play("ChoiceSubtitleAnim");
            textComponent.text = textToShow;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            panelAnimation.Play("ChoiceSubtitleHideAnim");
            textComponent.text = "";
        }
    }
}
