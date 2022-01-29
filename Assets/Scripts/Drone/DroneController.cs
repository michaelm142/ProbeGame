using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DroneController : MonoBehaviour
{
    public const int MaxDroneCount = 5;

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

    public float correctiveStrength = 100.0f;

    public static DroneController Instance { get; private set; }

    private List<Drone> drones = new List<Drone>();
    public IEnumerable<Drone> Drones
    {
        get
        {
            foreach (var d in drones)
                yield return d;
        }
    }

    public int DroneCount
    {
        get { return drones.Count; }
    }
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

        if (activeDrone == null)
            return;

        if (Pause.Paused)
        {
            //activeDrone.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            int droneIndex = drones.IndexOf(activeDrone);

            float dot_forward = Vector3.Dot(activeDrone.transform.forward, Vector3.up);
            float dot_right = Vector3.Dot(activeDrone.transform.right, Vector3.up);

            var body = activeDrone.GetComponent<Rigidbody>();
            float strength = Mathf.Abs(1.0f - Vector3.Dot(activeDrone.transform.up, Vector3.up));
            body.AddForce(Vector3.up * strength * correctiveStrength);
            body.AddRelativeForce(Vector3.forward * dot_right * correctiveStrength * strength + Vector3.right * dot_forward * correctiveStrength * strength);

            if (!activeDrone.hacking)
            {
                activeDrone.transform.position += vertical * activeDrone.transform.forward * activeDrone.MoveSpeed * Time.deltaTime;
                activeDrone.transform.rotation *= Quaternion.AngleAxis(horizontal * activeDrone.MoveSpeed, Vector3.up);
            }
        }
#if FALSE
        float moveAxis = Input.GetAxis("Fire1");
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
#endif
    }
    private float moveAxisPrev;

    public void DroneDestroyed(Drone drone)
    {
        FindObjectOfType<DroneUIController>().SendMessage("DroneDestroyed", drone);
        drones.Remove(drone);
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

            GetComponentInChildren<DroneUIController>().SendMessage("OnDroneDamaged", Vector3.Angle(direction, drone.transform.forward) * (Vector3.Dot(drone.transform.right, direction) < 0 ? 1.0f : -1.0f));
        }
    }

    public void SetActiveDrone(int index)
    {
        ActiveDrone = drones[index];
    }

    public void BeginHacking(GameObject subsystem)
    {
        var game = GetComponentInChildren<DroneUIController>().hackingMinigame;
        game.gameObject.SetActive(true);
        game.LoadConfiguration("Config_Level7_2.lvlconfig");
        game.ConnectedSubsystem = subsystem;
    }

    public void StopHacking()
    {
        var game = GetComponentInChildren<DroneUIController>().hackingMinigame;
        game.ConnectedSubsystem = null;
        game.gameObject.SetActive(false);
    }

    public void ConnectDrone()
    {
        var game = GetComponentInChildren<HackingMinigame>();
        game.RestartGame();
        game.gameObject.SetActive(false);
        var subsystem = game.ConnectedSubsystem;
        subsystem.SendMessage("ActivateSubsystem");
        ActiveDrone.connectedSubsystem = subsystem;
        transform.Find("ProbeStatus/HackingOverlay").gameObject.SetActive(true);
    }

    public void DisconnectDrone()
    {
        ActiveDrone.connectedSubsystem.BroadcastMessage("Disconnect", null, SendMessageOptions.DontRequireReceiver);
        ActiveDrone.connectedSubsystem = null;
    }
}
