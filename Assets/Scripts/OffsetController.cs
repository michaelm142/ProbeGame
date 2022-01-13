using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OffsetController : MonoBehaviour
{
    public Vector2 Offset;

    private UnityEngine.UI.Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.material.mainTextureOffset = Offset;
    }
}
