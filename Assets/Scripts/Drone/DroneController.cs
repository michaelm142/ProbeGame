using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DroneController : MonoBehaviour
{
    private Drone activeDrone;
    public Drone ActiveDrone
    {
        get { return activeDrone; }

        set
        {
            if (!Application.isPlaying)
            {
                activeDrone = value;
                return;
            }

            if (activeDrone != null)
                activeDrone.SetInactive();
            activeDrone = value;
            if (activeDrone != null)
                activeDrone.SetActive();
        }
    }

    public static DroneController Instance;

    private List<Drone> drones = new List<Drone>();
    public IEnumerable<Drone> Drones
    {
        get
        {
            foreach (var d in drones)
                yield return d;
        }
    }

    private float moveAxisPrev;

    public string MoveInputAxis = "Fire1";

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        foreach (var d in FindObjectsOfType<Drone>())
        {
            Drone drone = d;
            d.GetComponent<MiniMapIcon>().OnClicked.AddListener(new UnityEngine.Events.UnityAction(delegate () { ActiveDrone = drone; }));
            d.Camera.enabled = false;
            drones.Add(d);
        }

        CameraController.instance.OnCameraChanged.AddListener(OnCameraChanged);
    }

    private void OnCameraChanged()
    {
        if (CameraController.instance.ActiveCamera == null)
            return;
        if (CameraController.instance.ActiveCamera.transform.parent != null && CameraController.instance.ActiveCamera.transform.parent.tag == "Player")
            transform.Find("ProbeStatus/HackingOverlay").gameObject.SetActive(activeDrone.hacking);
    }

    private bool first = true;

    // Update is called once per frame
    void Update()
    {
        if (first)
        {
            if (drones.Count == 0)
            {
                foreach (var d in FindObjectsOfType<Drone>())
                {
                    Drone drone = d;
                    d.GetComponent<MiniMapIcon>().OnClicked.AddListener(new UnityEngine.Events.UnityAction(delegate () { ActiveDrone = drone; }));
                    d.Camera.enabled = false;
                    drones.Add(d);
                }
            }

            ActiveDrone = drones[0];
            first = false;
        }

        float moveAxis = Input.GetAxis(MoveInputAxis);
        if (ActiveDrone != null && moveAxis > 0.1f && moveAxisPrev < 0.1f)
        {
            var raycaster = GetComponent<GraphicRaycaster>();
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            PointerEventData data = new PointerEventData(FindObjectOfType<EventSystem>());
            data.position = Input.mousePosition;

            raycaster.Raycast(data, raycastResults);

            if (raycastResults.Count == 0)
            {
                var cam = CameraController.instance.ActiveCamera;
                Ray r = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(r, out hit, 10000.0f))
                {
                    var agent = ActiveDrone.GetComponent<NavMeshAgent>();
                    NavMeshHit navHit;
                    NavMesh.SamplePosition(hit.point, out navHit, 1000.0f, NavMesh.AllAreas);

                    agent.SetDestination(navHit.position);
                }
            }
        }
        moveAxisPrev = moveAxis;
    }

    public void DroneDestroyed(Drone drone)
    {
        drones.Remove(drone);
        var destoryedIndicator = Instantiate(Resources.Load<GameObject>("UI/PlayerDestroyedIndicator"));
        destoryedIndicator.transform.position = drone.transform.position;
    }

    public void DroneDamaged(Drone drone, Vector3 position)
    {
        if (activeDrone == null || activeDrone != drone)
        {
            var damageIndicator = Instantiate(Resources.Load<GameObject>("UI/PlayerDamageIndicator"));
            damageIndicator.transform.position = drone.transform.position;
        }
        else
        {
            Vector3 direction = position - drone.transform.position;

            GetComponentInChildren<DroneUIController>().SendMessage("OnDroneDamaged", Vector3.Angle(direction, drone.transform.forward) * (Vector3.Dot(drone.transform.right, direction) < 0 ? 1.0f : -1.0f)) ;
        }
    }

    public void SetActiveDrone(int index)
    {
        ActiveDrone = drones[index];
    }

    public void ConnectDrone(GameObject subsystem)
    {
        ActiveDrone.connectedSubsystem = subsystem;
        transform.Find("ProbeStatus/HackingOverlay").gameObject.SetActive(true);
    }

    public void DisconnectDrone()
    {
        ActiveDrone.connectedSubsystem.BroadcastMessage("Disconnect", null, SendMessageOptions.DontRequireReceiver);
        ActiveDrone.connectedSubsystem = null;
    }
}
