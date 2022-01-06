using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MiniMapClickNavigation : MonoBehaviour, IPointerDownHandler
{
    public Texture2D sampleBuffer;

    Color[] pixelBuffer;

    public Camera minimapCamera;

    public RenderTexture rt
    {
        get { return minimapCamera.targetTexture; }
    }

    private DroneController droneController
    {
        get { return FindObjectOfType<DroneController>(); }
    }

    public Drone ActiveDrone
    {
        get { return droneController.ActiveDrone; }
        set { droneController.ActiveDrone = value; }
    }

    public PointerEventData.InputButton InputButton;

    public event System.EventHandler OnClick;

    public bool IgnoreNextClick;

    public NavMeshAgent agent
    {
        get
        {
            if (ActiveDrone == null)
                return null;

            return ActiveDrone.GetComponent<NavMeshAgent>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != InputButton)
            return;
        RectTransform r = GetComponent<RectTransform>();
        // Debug.Log(r.worldToLocalMatrix.MultiplyPoint(eventData.position) / r.rect.max);
        Vector3 mapPos = r.worldToLocalMatrix.MultiplyPoint(eventData.position) / r.rect.max;
        Vector3 vertical = minimapCamera.transform.up * (mapPos.y * minimapCamera.orthographicSize);
        Vector3 horizontal = minimapCamera.transform.right * (mapPos.x * minimapCamera.orthographicSize);
        Vector3 samplePos = ((mapPos + Vector3.one) / 2.0f);
        samplePos.y = 1.0f - samplePos.y;
        samplePos.x *= sampleBuffer.width;
        samplePos.y *= sampleBuffer.height;
        if (pixelBuffer[(int)samplePos.x + (int)samplePos.y * sampleBuffer.width].a == 0 && !ActiveDrone.hacking)
        {
             if (!IgnoreNextClick && agent != null)
                agent.SetDestination(minimapCamera.transform.position + vertical + horizontal);
            else
                IgnoreNextClick = false;
            OnClick?.Invoke(minimapCamera.transform.position + vertical + horizontal, new System.EventArgs());
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        sampleBuffer = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        RenderTexture.active = rt;
        sampleBuffer.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        sampleBuffer.Apply();

        pixelBuffer = sampleBuffer.GetPixels();

        OnClick += onClick;
    }

    void onClick(object sender, System.EventArgs e) { }

    // Update is called once per frame
    void Update()
    {
    }
}
