using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiniMapIcon : MonoBehaviour
{
    public GameObject MapIcon { get; set; }

    public Texture2D Icon;

    public Color Color = Color.white;

    public List<Transform> MapObjects { get; private set; } = new List<Transform>();

    public bool RotateWithObject;
    public bool RaycastTarget = true;

    /// <summary>
    /// Raised when the minimap icon is clicked
    /// </summary>
    public UnityEngine.UI.Button.ButtonClickedEvent OnClicked;

    public void Start()
    {
        MapObjects.Add(FindObjectOfType<MinimapIconManager>().AddIcon(this));
    }

    private void OnDestroy()
    {
        if (MinimapIconManager.instance == null)
            return;

        MinimapIconManager.instance.RemoveIcon(this);
    }
}
