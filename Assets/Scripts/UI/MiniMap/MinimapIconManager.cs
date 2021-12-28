using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapIconManager : MonoBehaviour
{
    private List<MiniMapIcon> trackedObjects { get; set; } = new List<MiniMapIcon>();

    private static MinimapIconManager _instance;
    public static MinimapIconManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<MinimapIconManager>();

            return _instance;
        }
    }

    private Camera minimapCamera
    {
        get { return GetComponent<MiniMapClickNavigation>().minimapCamera; }
    }

    public GameObject IconPrefab;

    public Vector3 GetMapPosition(Vector3 point)
    {
        Vector3 L = point - minimapCamera.transform.position;
        float dot_r = Vector3.Dot(L, minimapCamera.transform.right) / minimapCamera.orthographicSize;
        float dot_u = Vector3.Dot(L, minimapCamera.transform.up) / minimapCamera.orthographicSize;

        Rect r = GetComponent<RectTransform>().rect;

        return new Vector3(r.width * (dot_r / 2.0f), r.height * (dot_u / 2.0f));
    }

    Quaternion GetMapRotation(Quaternion rotation)
    {
        return Quaternion.AngleAxis(rotation.eulerAngles.y - 90.0f, Vector3.back);
    }

    public Transform AddIcon(Texture2D sprite, Color color, Button.ButtonClickedEvent OnClicked = null, string name = null)
    {
        GameObject icon = Instantiate(IconPrefab, transform);
        icon.GetComponent<Image>().sprite = Sprite.Create(sprite, Rect.MinMaxRect(0, 0, sprite.width, sprite.height), Vector3.one * 0.5f);
        icon.GetComponent<Image>().color = color;
        if (OnClicked == null)
            Destroy(icon.GetComponent<Button>());
        else
            icon.GetComponent<Button>().onClick = OnClicked;

        icon.name = name != null ? name : string.Format("Icon {0}", trackedObjects.Count);

        return icon.transform;
    }


    public Transform AddIcon(MiniMapIcon mapIcon, GameObject prefabOverride = null)
    {
        GameObject Icon = null;
        if (prefabOverride == null)
            Icon = Instantiate(IconPrefab, transform);
        else
            Icon = Instantiate(prefabOverride, transform);
        if (mapIcon.Icon != null)
        {
            Icon.GetComponent<Image>().sprite = Sprite.Create(mapIcon.Icon, Rect.MinMaxRect(0, 0, mapIcon.Icon.width, mapIcon.Icon.height), Vector2.one * 0.5f);
            Icon.GetComponent<Image>().raycastTarget = mapIcon.RaycastTarget;
        }
        if (mapIcon.OnClicked == null)
            Icon.GetComponent<Button>().interactable = false;
        else
            Icon.GetComponent<Button>().onClick = mapIcon.OnClicked;
        if (mapIcon.gameObject != null)
            Icon.name = string.Format("Icon for: {0}", mapIcon.gameObject.name);

        Icon.GetComponent<Image>().color = mapIcon.Color;

        trackedObjects.Add(mapIcon);
        mapIcon.MapIcon = Icon;

        return Icon.GetComponent<Transform>();
    }

    public void RemoveIcon(MiniMapIcon mapIcon)
    {
        Destroy(mapIcon.MapIcon);
        trackedObjects.Remove(mapIcon);
    }

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        trackedObjects.RemoveAll(t => t == null);

        foreach (var trackedObj in trackedObjects)
        {
            foreach (var icon in trackedObj.MapObjects)
            {
                if (!trackedObj.enabled)
                {
                    icon.gameObject.SetActive(false);
                    break;
                }
                else if (!icon.gameObject.activeSelf)
                    icon.gameObject.SetActive(true);

                icon.transform.localPosition = GetMapPosition(trackedObj.transform.position);
                icon.GetComponent<Image>().color = trackedObj.Color;
                if (trackedObj.RotateWithObject)
                    icon.transform.localRotation = GetMapRotation(trackedObj.transform.rotation);
            }
        }
    }

}
