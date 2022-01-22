using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickerText : MonoBehaviour
{
    private Text text;

    public string textBuffer { get; set; }

    public float time;
    public float duration = 1.0f;

    public bool animateAutomatically;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        if (textBuffer == null)
            textBuffer = text.text;
        text.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if ((animateAutomatically && Application.isPlaying) && time < duration)
            time += Time.deltaTime;
        time = Mathf.Clamp(time, 0.0f, duration);


        float targetLength = textBuffer.Length * (time / duration);
        text.text = textBuffer.Substring(0, (int)targetLength);
    }
}
