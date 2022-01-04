using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickerText : MonoBehaviour
{
    private Text text;

    private string startText;

    public float time;
    public float duration = 1.0f;

    public bool animateAutomatically;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        startText = text.text;
        text.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (animateAutomatically && time < duration)
            time += Time.deltaTime;
        time = Mathf.Clamp(time, 0.0f, duration);


        float targetLength = startText.Length * (time / duration);
        text.text = startText.Substring(0, (int)targetLength);
    }
}
