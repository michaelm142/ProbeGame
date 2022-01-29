using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HackingTile : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler
{
    public Sprite Pipe;
    public Sprite Node;
    public Sprite ConnectedNode;
    public Sprite PipeTurnLeft;
    public Sprite PipeTurnRight;
    public Sprite EndPipe;

    public HackingTile tile_up;
    public HackingTile tile_left;
    public HackingTile tile_right;
    public HackingTile tile_down;

    public List<HackingTile> neighbours { get; private set; }

    public Color Color
    {
        get { return GetComponent<Image>().color; }
        set { GetComponent<Image>().color = value; }
    }

    public HackingMinigame game;
    public Sprite sprite
    {
        get { return GetComponent<Image>().sprite; }
        set { GetComponent<Image>().sprite = value; }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        game.previouslyPressed = this;

        if (IsNode)
        {
            if (game.CurrentColor == Color.white)
                game.CurrentColor = Color;
            game.CompletedColors.Remove(Color);
            game.ColorLines[Color].Clear();
            Debug.Log("Line Started");
            game.ColorLines[Color].Add(this);
            return;
        }

        Color = game.CurrentColor;
        if (!game.ColorLines.ContainsKey(Color))
            return;

        sprite = Pipe;
        //Debug.Log(game.previouslyPressed.sprite.name);

        if (game.ColorLines.Values.ToList().Find(l => l.Contains(this)) == null)
            game.ColorLines[Color].Add(this);
        else
        {
            int index = game.ColorLines[Color].IndexOf(this);
            game.ColorLines[Color].RemoveRange(index + 1, (game.ColorLines[Color].Count - 1) - index);
        }

        game.UpdateTiles();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsNode && game.CurrentColor == Color && !game.ColorLines[Color].Contains(this))
        {
            if (game.CurrentColor == Color)
            {
                game.CompletedColors.Add(Color);
                game.ColorLines[Color].Add(this);
                return;
            }
            else if (eventData.pointerPress != null && game.CurrentColor != Color.white)
            {
                game.ColorLines[game.CurrentColor].Clear();
                game.CurrentColor = Color.white;
            }

        }
        else if (eventData.pointerPress != null && neighbours.Contains(game.previouslyPressed))
            OnPointerDown(eventData);

    }

    // Start is called before the first frame update
    void Start()
    {
        neighbours = new List<HackingTile>()
        {
            tile_up,
            tile_down,
            tile_left,
            tile_right,
        };
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (sprite == Node)
            return;

        //if (sprite == Pipe)
        //{
        //    if (IsPipeAndColor(tile_left) || IsPipeAndColor(tile_right))
        //        transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
        //    if (IsPipeAndColor(tile_down) && IsPipeAndColor(tile_right))
        //    {
        //        transform.rotation = Quaternion.AngleAxis(-90.0f, Vector3.forward);
        //        sprite = PipeTurnRight;
        //    }
        //    if (IsPipeAndColor(tile_down) && IsPipeAndColor(tile_left))
        //    {
        //        sprite = PipeTurnRight;
        //        transform.rotation = Quaternion.AngleAxis(180.0f, Vector3.forward);
        //    }
        //    if (IsPipeAndColor(tile_up) && IsPipeAndColor(tile_right))
        //    {
        //        sprite = PipeTurnRight;
        //        transform.rotation = Quaternion.identity;
        //    }
        //    if (IsPipeAndColor(tile_up) && IsPipeAndColor(tile_left))
        //    {
        //        sprite = PipeTurnRight;
        //        transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
        //    }
        //}
    }

    public bool IsNode
    {
        get { return sprite == Node || sprite == ConnectedNode; }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        game.previouslyPressed = null;
        if (!IsNode || !game.CompletedColors.Contains(Color))
            game.ColorLines[Color].Clear();
        game.CurrentColor = Color.white;
        game.UpdateTiles();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
