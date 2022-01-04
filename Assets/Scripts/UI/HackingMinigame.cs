using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackingMinigame : MonoBehaviour
{
    public int Difficulty = 1;

    private List<HackingTile> tiles = new List<HackingTile>();

    public Sprite Pipe;
    public Sprite Node;
    public Sprite ConnectedNode;
    public Sprite PipeTurnLeft;
    public Sprite PipeTurnRight;
    public Sprite EndPipe;

    public Dictionary<Color, List<HackingTile>> ColorLines = new Dictionary<Color, List<HackingTile>>();

    public HackingTile previouslyPressed;

    public Color CurrentColor;// { get; set; } = Color.white;

    private Color[] ColorPalette = new Color[]
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.cyan,
        new Color(0.4f, 0.09f, 0.8f),
        Color.magenta,
        Color.yellow,
    };

    // Start is called before the first frame update
    void Start()
    {
        foreach (Color c in ColorPalette)
            ColorLines.Add(c, new List<HackingTile>());

        GenerateTiles(Difficulty);
    }

    public void UpdateTiles()
    {
        foreach (var tile in tiles)
        {
            if (tile.IsNode)
                continue;
            tile.sprite = null;
            tile.Color = Color.white;
        }
        foreach (var kvp in ColorLines)
        {
            var color = kvp.Key;
            var line = kvp.Value;
            if (line.Count == 0)
                continue;

            for (int i = 1; i < line.Count - 1; i++)
            {
                var tile = line[i];
                var tileNext = line[i + 1];
                var tilePrev = line[i - 1];
                if (tileNext == tile.tile_left && tilePrev == tile.tile_right || (tileNext == tile.tile_right && tilePrev == tile.tile_right))
                    tile.sprite = PipeTurnRight;
                if (tile.IsNode)
                    continue;

                tile.sprite = Pipe;
                tile.Color = color;
            }
        }
    }

    public void RestartGame()
    {
        foreach (var line in ColorLines)
        {
            line.Value.Clear();
        }
        UpdateTiles();
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
                GameObject tile = new GameObject(string.Format("Tile {0}x{1}", x, y), typeof(RectTransform), typeof(HackingTile), typeof(Image));
                tile.transform.SetParent(transform);
                tile.transform.localPosition = Vector3.right * x * tileDim + Vector3.up * y * tileDim;
                tile.transform.localPosition -= Vector3.up * (rect.height * 0.5f) + Vector3.right * (rect.width * 0.5f);
                var tileRT = tile.GetComponent<RectTransform>();
                tileRT.pivot = Vector2.one * 0.5f;
                tileRT.sizeDelta = new Vector2(tileDim, tileDim);

                var ht = tile.GetComponent<HackingTile>();
                ht.game = this;
                ht.ConnectedNode = ConnectedNode;
                ht.Node = Node;
                ht.Pipe = Pipe;
                ht.EndPipe = EndPipe;
                ht.PipeTurnLeft = PipeTurnLeft;
                ht.PipeTurnRight = PipeTurnRight;

                tiles.Add(ht);
            }
        }

        for (int y = 0; y < Difficulty; y++)
        {
            for (int x = 0; x < Difficulty; x++)
            {
                var tile_up = (y == Difficulty - 1) ? null : tiles[x + (y + 1) * Difficulty].GetComponent<HackingTile>();
                var tile_down = (y == 0) ? null : tiles[x + (y - 1) * Difficulty].GetComponent<HackingTile>();
                var tile_right = (x == Difficulty - 1) ? null : tiles[(x + 1) + y * Difficulty].GetComponent<HackingTile>();
                var tile_left = (x == 0) ? null : tiles[(x - 1) + y * Difficulty].GetComponent<HackingTile>();
                var tile = tiles[x + y * Difficulty].GetComponent<HackingTile>();

                tile.GetComponent<HackingTile>().tile_up = tile_up;
                tile.GetComponent<HackingTile>().tile_down = tile_down;
                tile.GetComponent<HackingTile>().tile_right = tile_right;
                tile.GetComponent<HackingTile>().tile_left = tile_left;
            }
        }

        for (int i = 0; i < difficulty; i++)
        {
            int rand_x = Random.Range(0, difficulty);
            int rand_y = Random.Range(0, difficulty);
            while (tiles[rand_x + rand_y * difficulty].sprite == Node)
            {
                rand_x = Random.Range(0, difficulty);
                rand_y = Random.Range(0, difficulty);
            }

            tiles[rand_x + rand_y * difficulty].GetComponent<HackingTile>().sprite = Node;
            tiles[rand_x + rand_y * difficulty].GetComponent<Image>().color = ColorPalette[i];

            rand_x = Random.Range(0, difficulty);
            rand_y = Random.Range(0, difficulty);
            tiles[rand_x + rand_y * difficulty].GetComponent<HackingTile>().sprite = Node;
            tiles[rand_x + rand_y * difficulty].GetComponent<Image>().color = ColorPalette[i];
        }
    }
}
