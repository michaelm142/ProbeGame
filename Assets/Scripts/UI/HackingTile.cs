using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HackingTile : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    public Sprite Pipe;
    public Sprite Node;
    public Sprite ConnectedNode;

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = Pipe;
        GetComponent<Image>().color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerPress != null)
            OnPointerDown(eventData);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
