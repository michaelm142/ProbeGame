using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisconnectButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private float value;

    private bool buttonDown;

    private Animator anim;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonDown = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonDown)
            value += Time.deltaTime;
        else
            value -= Time.deltaTime;

        value = Mathf.Clamp(value, 0.0f, 1.0f);

        anim.SetFloat("Blend", value);
    }

    public void End()
    {
        buttonDown = false;
        value = 0.0f;
    }
}
