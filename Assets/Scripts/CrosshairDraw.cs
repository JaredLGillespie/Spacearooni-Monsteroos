using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CrosshairDraw : MonoBehaviour
{
    [Serializable]
    private class CursorInfo
    {
        public string Name;
        public Sprite Image;
        public Sprite AltImage;
    }

    [SerializeField] private List<CursorInfo> CursorInfos;
    [SerializeField] private Sprite DefaultImage;
    [SerializeField] private Sprite DefaultImageAlt;

    private CursorInfo currentCursorInfo;
    private bool useAlt = false;

    private void Start()
    {
        // Hide cursor
        Cursor.visible = false;

        // Use default image
        var def = new CursorInfo
        {
            Name = "default",
            Image = DefaultImage,
            AltImage = DefaultImageAlt
        };

        CursorInfos.Add(def);

        if (currentCursorInfo == null)
            currentCursorInfo = def;
    }

    private void OnGUI()
    {
        if (useAlt)
        {
            float xMin = Screen.width - (Screen.width - Input.mousePosition.x) - (currentCursorInfo.AltImage.rect.width / 2);
            float yMin = (Screen.height - Input.mousePosition.y) - (currentCursorInfo.AltImage.rect.height / 2);
            GUI.DrawTextureWithTexCoords(new Rect(xMin, yMin, currentCursorInfo.AltImage.rect.width, currentCursorInfo.AltImage.rect.height), currentCursorInfo.AltImage.texture, new Rect(0.0f, 0.0f, 0.5f, 1.0f));
        }
        else
        {
            float xMin = Screen.width - (Screen.width - Input.mousePosition.x) - (currentCursorInfo.Image.rect.width / 2);
            float yMin = (Screen.height - Input.mousePosition.y) - (currentCursorInfo.Image.rect.height / 2);
            GUI.DrawTextureWithTexCoords(new Rect(xMin, yMin, currentCursorInfo.AltImage.rect.width, currentCursorInfo.AltImage.rect.height), currentCursorInfo.AltImage.texture, new Rect(0.0f, 0.0f, 0.5f, 1.0f));
        }
    }

    public void SetCursor(string name)
    {
        var ci = CursorInfos.Where(w => w.Name.ToLower().Equals(name.ToLower())).FirstOrDefault();

        if (ci == null)
            throw new UnityException("Unknown cursor name given: " + name);

        currentCursorInfo = ci;
    }

    public void UseAltImage()
    {
        useAlt = true;
    }

    public void UseNormalImage()
    {
        useAlt = false;
    }
}
