using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(RawImage))]
public class MinimapFogOfWar : MonoBehaviour
{
    public ComputeShader shader;
    private ComputeBuffer points;

    public int TextureDim = 512;
    private const int POINT_COUNT = 5;
    private int kernalIndex;

    private RenderTexture texture;

    public List<Vector3> pointBuffer;

    // Start is called before the first frame update
    void Start()
    {
        texture = new RenderTexture(TextureDim, TextureDim, 16);
        Color[] textureData = new Color[TextureDim * TextureDim];
        for (int i = 0; i < textureData.Length; i++)
            textureData[i] = Color.black;
        texture.enableRandomWrite = true;
        texture.Create();

        kernalIndex = shader.FindKernel("CSMain");
        int startKernal = shader.FindKernel("CSStart");
        shader.SetTexture(startKernal, "Result", texture);
        shader.Dispatch(startKernal, TextureDim / 8, TextureDim / 8, TextureDim / 8);

        points = new ComputeBuffer(5, 12);
        //pointBuffer = new List<Vector3>();
        //for (int i = 0; i < POINT_COUNT; i++)
        //    pointBuffer.Add(Vector3.zero);
        shader.SetBuffer(kernalIndex, "points", points);

        var image = GetComponent<RawImage>();
        image.texture = texture;

        shader.SetFloat("TextureDim", TextureDim);
        var rectTransform = GetComponent<RectTransform>();
        Rect rect = rectTransform.rect;
        shader.SetFloat("RectWidth", rect.width);
        shader.SetFloat("RectHeight", rect.height);
    }

    private void OnDestroy()
    {
        points.Release();
    }

    // Update is called once per frame
    void Update()
    {
        var drones = FindObjectsOfType<Drone>();
        for (int i = 0; i < drones.Length && i < pointBuffer.Count; i++)
        {
            var rectTransform = GetComponent<RectTransform>();
            Rect rect = rectTransform.rect;

            Vector3 position = MinimapIconManager.instance.GetMapPosition(drones[i].transform.position) + Vector3.right * (rect.width * 0.5f) + Vector3.up * (rect.height * 0.5f);
            Debug.Log(position);
            // TODO: calculate drone LOS
            float range = 25.0f;

            pointBuffer[i] = new Vector3(position.x, position.y, range);
        }
        shader.SetTexture(kernalIndex, "Result", texture);

        points.SetData(pointBuffer.ToList());

        shader.Dispatch(kernalIndex, TextureDim / 8, TextureDim / 8, TextureDim / 8);
    }
}
