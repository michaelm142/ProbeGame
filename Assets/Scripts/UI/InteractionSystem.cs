using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    private GameObject interactableButtonPrefab;

    private List<Transform> icons { get; set; } = new List<Transform>();

    private List<Interactable> trackedObjects { get; set; } = new List<Interactable>();

    // Start is called before the first frame update
    void Start()
    {
        interactableButtonPrefab = Resources.Load<GameObject>("UI/InteractionButton");
    }

    // Update is called once per frame
    void Update()
    {
        icons.RemoveAll(i => i == null);
        trackedObjects.RemoveAll(t => t == null);

        foreach (var trackedObj in trackedObjects)
        {
            trackedObj.transform.localPosition = GetScreenPosition(trackedObj.transform.position);
        }
    }

    Vector3 GetScreenPosition(Vector3 worldPosition)
    {
        return Camera.current.WorldToScreenPoint(worldPosition);
    }

    public Transform AddIcon(Button.ButtonClickedEvent OnClicked = null, string name = null)
    {
        GameObject icon = Instantiate(interactableButtonPrefab, transform);
        if (OnClicked == null)
            Destroy(icon.GetComponent<Button>());
        else
            icon.GetComponent<Button>().onClick = OnClicked;

        icon.name = name != null ? name : string.Format("Icon {0}", icons.Count);
        icons.Add(icon.transform);

        return icon.transform;
    }
}
