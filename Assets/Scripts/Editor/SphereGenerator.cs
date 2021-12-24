using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SphereGenerator : EditorWindow
{
    int resolutionX = 0;
    float size = 1.0f;

    private void OnEnable()
    {
        GetWindow(typeof(SphereGenerator));
    }

    private void OnGUI()
    {
        resolutionX = EditorGUILayout.IntField("Resolution:", resolutionX);
        size = EditorGUILayout.FloatField("Size", size);
        if (GUILayout.Button("Generate Sphere"))
            GenerateSphere(resolutionX);
    }

    private void GenerateSphere(int res)
    {
        GameObject geodesic = new GameObject("Sphere");
        Vector3[] normals = new Vector3[]
        {
            Vector3.forward,
            Vector3.back,
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
        };

        List<Vector3> points = new List<Vector3>();
        foreach (var v in normals)
        {
            points.AddRange(CreateFace(v, res));
        }

        foreach (var point in points)
        {
            GameObject p = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            p.transform.position = point.normalized * size;
            p.transform.SetParent(geodesic.transform);
        }
    }

    private List<Vector3> CreateFace(Vector3 normal, int resolution)
    {
        Vector3 axisA = new Vector3(normal.y, normal.z, normal.x);
        Vector3 axisB = Vector3.Cross(normal, axisA);

        List<Vector3> outval = new List<Vector3>();

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                Vector2 t = new Vector2(x, y) / (resolution - 1f);
                Vector3 point = normal + axisA * (2 * t.x - 1) + axisB * (2 * t.y - 1f);

                outval.Add(point);
            }
        }

        return outval;
    }

    [MenuItem("Generate/Sphere")]
    public static void OnMenuItem()
    {
        var sg = CreateInstance<SphereGenerator>();
        sg.position = new Rect(Screen.width / 2, Screen.height / 2, 500, 100);
        sg.ShowPopup();
    }
}
