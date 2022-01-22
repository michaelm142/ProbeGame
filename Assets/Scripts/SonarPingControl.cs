using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SonarPingControl : MonoBehaviour, IPointerClickHandler
{
    public float CoolDownLength = 30.0f;
    private float coolDownTimer;

    public bool useSonarPing;

    public Image Image;
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        coolDownTimer = CoolDownLength;
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDownTimer < CoolDownLength)
        {
            coolDownTimer += Time.deltaTime;
            button.interactable = false;
        }
        else
            button.interactable = true;

        Image.fillAmount = coolDownTimer / CoolDownLength;
        Image.SetAllDirty();
    }

    public void ButtonClick()
    {
        useSonarPing = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!useSonarPing || coolDownTimer < CoolDownLength) return;

        GameObject prefab = Resources.Load<GameObject>("UI/SonarPing");
        var ping = Instantiate(prefab);
        ping.transform.position = eventData.position;
        coolDownTimer = 0.0f;
        useSonarPing = false;
    }
}
