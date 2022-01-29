using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SonarPingControl : MonoBehaviour, IPointerClickHandler
{
    public Camera minimapCamera;

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
        var sampleBuffer = GetComponent<RawImage>().texture;
        var cameraPosition = minimapCamera.transform.position;
        cameraPosition.y = 0.0f;

        RectTransform r = GetComponent<RectTransform>();
        Vector3 mapPos = r.worldToLocalMatrix.MultiplyPoint(eventData.position) / r.rect.max;
        Vector3 vertical = minimapCamera.transform.up * (mapPos.y * minimapCamera.orthographicSize);
        Vector3 horizontal = minimapCamera.transform.right * (mapPos.x * minimapCamera.orthographicSize);
        Vector3 samplePos = ((mapPos + Vector3.one) / 2.0f);
        samplePos.y = 1.0f - samplePos.y;
        samplePos.x *= sampleBuffer.width;
        samplePos.y *= sampleBuffer.height;

        ping.transform.position = cameraPosition + vertical + horizontal;
        coolDownTimer = 0.0f;
        useSonarPing = false;
    }
}
