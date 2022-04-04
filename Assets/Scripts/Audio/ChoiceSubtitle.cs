using UnityEngine;
using UnityEngine.UI;

public class ChoiceSubtitle : MonoBehaviour
{
    [SerializeField] string textToShow = "Missing subtitle lol";
    Text textComponent;

    private void Start() {
        textComponent = GameObject.Find("ChoiceSubtitle").GetComponent<Text>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            textComponent.text = textToShow;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            textComponent.text = "";
        }
    }
}
