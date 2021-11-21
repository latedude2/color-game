using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorSetup : MonoBehaviour
{
    [SerializeField] Texture2D cursorTexture;
    [SerializeField] float xspot;
    [SerializeField] float yspot;

    // Start is called before the first frame update
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "ConationQuestion1" || SceneManager.GetActiveScene().name != "ConationQuestion2" || SceneManager.GetActiveScene().name != "ConationQuestion3")
        {
            Vector2 hotSpot = new Vector2(xspot, yspot);
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.ForceSoftware);
        }


    }
}
