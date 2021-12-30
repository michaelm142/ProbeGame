using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// defines behavior for an interactable object
public class Interactable : MonoBehaviour
{
    private GameObject interactableButtonPrefab;
    private GameObject icon;

    public float radius = 10.0f;

    public Button.ButtonClickedEvent OnInteract;

    private void Start()
    {
        interactableButtonPrefab = Resources.Load<GameObject>("UI/Interaction");

        icon = Instantiate(interactableButtonPrefab, GameObject.Find("HUD/InteractionSystem").transform);
        if (OnInteract == null)
            Destroy(icon.GetComponent<Button>());
        else
            icon.GetComponent<Button>().onClick = OnInteract;
        icon.GetComponent<Button>().onClick.AddListener(OnUIInteractionClick);

        icon.name = name != null ? name : string.Format("Icon {0}", FindObjectsOfType<Interactable>().Length);
        icon.GetComponentInChildren<Text>().text = icon.name;
    }

    void OnUIInteractionClick()
    {
        gameObject.BroadcastMessage("Interact", SendMessageOptions.DontRequireReceiver);
    }

    private void Update()
    {
        icon.transform.localPosition = GetScreenPosition(transform.position);
        var activeDrone = DroneController.Instance.ActiveDrone;
        if (activeDrone == null)
            return;
        Vector3 L = transform.position - activeDrone.transform.position;
        if (L.magnitude < radius && Vector3.Dot(L, activeDrone.transform.forward) > 0.0f && !activeDrone.hacking)
        {
            if (!icon.activeSelf)
                icon.SetActive(true);
        }
        else if (icon.activeSelf)
            icon.SetActive(false);
    }
    Vector3 GetScreenPosition(Vector3 worldPosition)
    {
        if (CameraController.instance == null || CameraController.instance.ActiveCamera == null) return Vector3.zero;
        Vector3 point = CameraController.instance.ActiveCamera.WorldToScreenPoint(worldPosition) - Vector3.up * (Screen.height / 2.0f) - Vector3.right * (Screen.width / 2.0f);

        point.y = Mathf.Clamp(point.y, -Screen.height * 0.25f, Screen.height * 0.5f);

        return point;
    }

    private void OnDestroy()
    {
        Destroy(icon);
    }
}
