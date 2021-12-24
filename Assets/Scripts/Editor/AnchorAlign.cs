using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnchorAlign : EditorWindow
{
    [MenuItem("Edit/Align Anchors")]
    public static void OnMenuItem()
    {
        var obj = Selection.activeObject as GameObject;
        if (obj == null || obj.GetComponent<RectTransform>() != null)
        {
            var rt = obj.GetComponent<RectTransform>();
            if (rt.parent == null)
                return;
            var parentRect = (rt.parent as RectTransform).rect;
            var rect = rt.rect;

            float minX = rect.x / parentRect.width;
            float minY = rect.y / parentRect.height;
            float maxX = (rect.x + rect.width) / parentRect.width;
            float maxY = (rect.y + rect.height) / parentRect.height;

            rt.anchorMin = new Vector2(minX, minY);
            rt.anchorMax = new Vector2(maxX, maxY);
        }

    }
}
