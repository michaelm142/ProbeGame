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
        if (obj != null && obj.GetComponent<RectTransform>() != null)
        {
            var rt = obj.GetComponent<RectTransform>();
            if (rt.parent == null)
                return;
            var parentRect = (rt.parent as RectTransform).rect;
            var rect = rt.rect;

            float minX = (rt.offsetMin.x + rect.min.x + rt.anchoredPosition.x) / parentRect.width;
            float maxX = (rt.offsetMax.x + rect.max.x + rt.anchoredPosition.x) / parentRect.width;
            float minY = (rt.offsetMin.y + rect.min.y + rt.anchoredPosition.y) / parentRect.height;
            float maxY = (rt.offsetMax.y + rect.max.y + rt.anchoredPosition.y) / parentRect.height;
            float offsetX = rt.anchoredPosition.x / parentRect.width;
            float offsetY = rt.anchoredPosition.y / parentRect.height;
            Undo.RegisterCompleteObjectUndo(rt, "Undo Align Anchors");

            //rt.sizeDelta = new Vector2(-maxX, -maxY);
            //rt.anchoredPosition = Vector2.zero;
            rt.anchorMin = new Vector2(minX, minY);
            rt.anchorMax = new Vector2(maxX, maxY);

        }

    }
}
