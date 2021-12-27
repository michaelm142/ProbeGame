using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingMinigame : MonoBehaviour
{
    public int Difficulty = 1;

    private List<GameObject> tiles = new List<GameObject>();

    public Sprite Pipe;
    public Sprite Node;
    public Sprite ConnectedNode;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTiles(Difficulty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateTiles(int difficulty)
    {
        var rt = GetComponent<RectTransform>();
        var rect = rt.rect;
        float tileDim = rect.width / (float)difficulty;
        for (int y = 0; y < difficulty; y++)
        {
            for (int x = 0; x < difficulty; x++)
            {
                GameObject tile = new GameObject(string.Format("Tile {0}x{1}", x, y));
                tile.transform.SetParent(transform);
                Instantiate(tile);
                tile.transform.localPosition = Vector3.right * x * tileDim + Vector3.up * y * tileDim;
                tile.transform.localPosition -= Vector3.up * (rect.height * 0.5f) + Vector3.right * (rect.width * 0.5f);
                var tileRT = tile.AddComponent<RectTransform>();
                tileRT.pivot = Vector2.zero;
                tileRT.sizeDelta = new Vector2(tileDim, tileDim);

                var image = tile.AddComponent<UnityEngine.UI.Image>();
                Color randomColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                image.color = randomColor;

                var ht = tile.AddComponent<HackingTile>();
                ht.ConnectedNode = ConnectedNode;
                ht.Node = Node;
                ht.Pipe = Pipe;

                tiles.Add(tile);
            }
        }
    }
}
