using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDraw : MonoBehaviour
{
    [SerializeField] private Texture2D CursorImage;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void OnGUI()
    {
        float xMin = Screen.width - (Screen.width - Input.mousePosition.x) - (CursorImage.width / 2);
        float yMin = (Screen.height - Input.mousePosition.y) - (CursorImage.height / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, CursorImage.width, CursorImage.height), CursorImage);
    }

    public void SetCursor(Texture2D cursorImage)
    {
        this.CursorImage = cursorImage;
    }
}
