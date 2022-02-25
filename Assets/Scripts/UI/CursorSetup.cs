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
        Vector2 hotSpot = new Vector2(xspot, yspot);
        if(!Application.isEditor)
        {
            Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.ForceSoftware);
        }
    }
}
