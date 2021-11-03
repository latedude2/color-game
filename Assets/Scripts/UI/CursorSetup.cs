using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetup : MonoBehaviour
{
    [SerializeField] Texture2D cursorTexture;
    [SerializeField] float xspot;
    [SerializeField] float yspot;

    // Start is called before the first frame update
    void Update()
    {
        Vector2 hotSpot = new Vector2(xspot, yspot);
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.ForceSoftware);


    }
}
