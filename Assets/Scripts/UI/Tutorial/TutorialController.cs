using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public List<TutorialCondition> conditions;

    private int conditionIndex;

    private TutorialCondition CurrentCondition
    {
        get 
        {
            if (conditionIndex >= conditions.Count)
                return null;

            return conditions[conditionIndex]; 
        }
    }

    private Text text;

    private Image image;

    private TickerText TickerText;

    public float VisibleDuration = 20.0f;
    private float visibleTimer;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponentInChildren<Text>();
        image = GetComponent<Image>();
        TickerText = GetComponentInChildren<TickerText>();

        text.text = CurrentCondition.StartText;

        visibleTimer = VisibleDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (visibleTimer > 0)
        {
            image.enabled = true;
            text.enabled = true;
            visibleTimer -= Time.deltaTime;

            if (visibleTimer < 0)
            {
                image.enabled = false;
                text.enabled = false;
            }
        }
        if (CurrentCondition != null && CurrentCondition.Complete)
        {
            conditionIndex++;
            image.enabled = true;
            TickerText.time = 0.0f;
        }
    }
}

public abstract class TutorialCondition : MonoBehaviour
{
    public abstract bool Complete { get; }

    public abstract string StartText { get; }
}
