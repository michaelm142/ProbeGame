using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackingMinigame : MonoBehaviour
{
    public int Difficulty = 1;

    private List<HackingTile> tiles = new List<HackingTile>();
    public HackingTile previouslyPressed { get; set; }

    public Sprite Pipe;
    public Sprite Node;
    public Sprite ConnectedNode;
    public Sprite PipeTurnLeft;
    public Sprite PipeTurnRight;
    public Sprite EndPipe;

    public Dictionary<Color, List<HackingTile>> ColorLines = new Dictionary<Color, List<HackingTile>>();

    public List<List<HackingTile>> DebugColorLines => ColorLines.Values.ToList();

    public UnityEngine.Events.UnityEvent Win;

    public Color CurrentColor;// { get; set; } = Color.white;
    public List<Color> CompletedColors = new List<Color>();

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

    private void Update()
    {
        if (CompletedColors.Count == Difficulty)
            Win.Invoke();
    }

    public void UpdateTiles()
    {
        // Clear board
        foreach (var tile in tiles)
        {
            if (tile.IsNode)
                continue;
            tile.sprite = null;
            tile.Color = Color.white;
        }

        // Update each color line on the board
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

                UpdateTileInSequence(color, tilePrev, tile, tileNext);
            }

            if (line.Count < 2)
                continue;
            // update last tile in sequence
            var lastTile = line[line.Count - 1];
            var secondToLast = line[line.Count - 2];
            if (!lastTile.IsNode)
            {
                if (secondToLast == lastTile.tile_left || secondToLast == lastTile.tile_right)
                {
                    lastTile.Color = color;
                    lastTile.transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
                    lastTile.sprite = Pipe;
                }
                else if (secondToLast == lastTile.tile_up || secondToLast == lastTile.tile_down)
                {
                    lastTile.Color = color;
                    lastTile.transform.rotation = Quaternion.AngleAxis(0.0f, Vector3.forward);
                    lastTile.sprite = Pipe;
                }
            }
            else
            {
                // O O O
                // O X >
                // O O O
                if (lastTile == secondToLast.tile_left)
                {
                    lastTile.sprite = ConnectedNode;
                    lastTile.transform.rotation = Quaternion.AngleAxis(-90.0f, Vector3.forward);
                }
                // O ^ O
                // O X O
                // O O O
                if (lastTile == secondToLast.tile_down)
                {
                    lastTile.sprite = ConnectedNode;
                    lastTile.transform.rotation = Quaternion.AngleAxis(0.0f, Vector3.forward);
                }
                // O O O
                // < X O
                // O O O
                if (lastTile == secondToLast.tile_right)
                {
                    lastTile.sprite = ConnectedNode;
                    lastTile.transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
                }
                // O O O
                // O X O
                // O v O
                if (lastTile == secondToLast.tile_up)
                {
                    lastTile.sprite = ConnectedNode;
                    lastTile.transform.rotation = Quaternion.AngleAxis(180.0f, Vector3.forward);
                }
            }
        }
    }

    private void UpdateTileInSequence(Color sequenceColor, HackingTile tilePrev, HackingTile tile, HackingTile tileNext)
    {
        if (tilePrev.IsNode)
        {
            // O O O
            // O X >
            // O O O
            if (tilePrev == tile.tile_left)
            {
                tilePrev.sprite = ConnectedNode;
                tilePrev.transform.rotation = Quaternion.AngleAxis(-90.0f, Vector3.forward);
            }
            // O ^ O
            // O X O
            // O O O
            if (tilePrev == tile.tile_down)
            {
                tilePrev.sprite = ConnectedNode;
                tilePrev.transform.rotation = Quaternion.AngleAxis(0.0f, Vector3.forward);
            }
            // O O O
            // < X O
            // O O O
            if (tilePrev == tile.tile_right)
            {
                tilePrev.sprite = ConnectedNode;
                tilePrev.transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
            }
            // O O O
            // O X O
            // O v O
            if (tilePrev == tile.tile_up)
            {
                tilePrev.sprite = ConnectedNode;
                tilePrev.transform.rotation = Quaternion.AngleAxis(180.0f, Vector3.forward);
            }
        }
        // O v O
        // O X >
        // O O O
        if (tilePrev == tile.tile_up && tileNext == tile.tile_right)
        {
            tile.sprite = PipeTurnRight;
            tile.transform.rotation = Quaternion.identity;
        }
        // O v O
        // < X o
        // O O O
        else if (tilePrev == tile.tile_up && tileNext == tile.tile_left)
        {
            tile.sprite = PipeTurnLeft;
            tile.transform.rotation = Quaternion.identity;
        }
        // O O O
        // > X O
        // O V O
        else if (tilePrev == tile.tile_left && tileNext == tile.tile_down)
        {
            tile.sprite = PipeTurnRight;
            tile.transform.rotation = Quaternion.AngleAxis(180.0f, Vector3.forward);
        }
        // O ^ O
        // > X O
        // O O O
        else if (tilePrev == tile.tile_left && tileNext == tile.tile_up)
        {
            tile.sprite = PipeTurnLeft;
            tile.transform.rotation = Quaternion.identity;
        }
        // O ^ O
        // 0 X <
        // O O O
        else if (tilePrev == tile.tile_right && tileNext == tile.tile_up)
        {
            tile.sprite = PipeTurnRight;
            tile.transform.rotation = Quaternion.identity;
        }
        // O O O
        // O X <
        // O v O
        else if (tilePrev == tile.tile_right && tileNext == tile.tile_down)
        {
            tile.sprite = PipeTurnLeft;
            tile.transform.rotation = Quaternion.AngleAxis(180.0f, Vector3.forward);
        }
        // O O O
        // O X >
        // O ^ O
        else if (tilePrev == tile.tile_down && tileNext == tile.tile_right)
        {
            tile.sprite = PipeTurnRight;
            tile.transform.rotation = Quaternion.AngleAxis(-90.0f, Vector3.forward);
        }
        // O O O
        // < X O
        // O ^ O
        else if (tilePrev == tile.tile_down && tileNext == tile.tile_left)
        {
            tile.sprite = PipeTurnLeft;
            tile.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
        }
        // O O O
        // X X X
        // O O O
        else if ((tilePrev == tile.tile_left && tileNext == tile.tile_right || (tilePrev == tile.tile_right && tileNext == tile.tile_left)))
        {
            tile.sprite = Pipe;
            tile.transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
        }
        // O X O
        // 0 X O
        // O X O
        else
        {
            tile.transform.rotation = Quaternion.identity;
            tile.sprite = Pipe;
        }

        tile.Color = sequenceColor;
    }

    public void RestartGame()
    {
        CompletedColors.Clear();
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

            while (tiles[rand_x + rand_y * difficulty].sprite == Node)
            {
                rand_x = Random.Range(0, difficulty);
                rand_y = Random.Range(0, difficulty);
            }
            tiles[rand_x + rand_y * difficulty].GetComponent<HackingTile>().sprite = Node;
            tiles[rand_x + rand_y * difficulty].GetComponent<Image>().color = ColorPalette[i];
        }
    }
}

