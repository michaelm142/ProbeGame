using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drone : MonoBehaviour
{
    public Camera Camera { get; private set; }

    private GameObject selectRing;
    public GameObject connectedSubsystem;

    public float MaxHealth = 30.0f;
    public float Health;

    public bool hacking { get { return connectedSubsystem != null; } }

    // Start is called before the first frame update
    void Awake()
    {
        Camera = transform.GetComponentInChildren<Camera>();

        var ring = Resources.Load<Texture2D>("SelectRing");
        selectRing = MinimapIconManager.instance.AddIcon(ring, Color.white).gameObject;
        selectRing.SetActive(false);
        GetComponent<MiniMapIcon>().MapObjects.Add(selectRing.transform);
        Health = MaxHealth;
    }

    private void Start()
    {
        var di = gameObject.AddComponent<DroneInventory>();
    }

    private void OnDestroy()
    {
        var di = GetComponent<DroneInventory>();
        var obj = new GameObject("Drone Inventory");
        DontDestroyOnLoad(obj);
        var obj_di = obj.AddComponent<DroneInventory>();
        obj_di.Energy = di.Energy;
        obj_di.Metal = di.Metal;
    }

    // Update is called once per frame
    void Update()
    {
        var clickNavigation = FindObjectOfType<MiniMapClickNavigation>();
        if (clickNavigation.ActiveDrone == this)
            selectRing.SetActive(true);
        else
            selectRing.SetActive(false);

        if (Health <= 0.0f)
            Destroy(gameObject);
    }

    public void SetActive()
    {
        selectRing.SetActive(true);
        CameraController.instance.ActiveCamera = Camera;
    }

    public void SetInactive()
    {
        CameraController.instance.ActiveCamera = null;
        selectRing.SetActive(false);
    }

    public void Damage(object[] info)
    {
        float ammount = (float)info[0];
        Vector3 position = (Vector3)info[1];
        Health -= ammount;
        DroneController.Instance.DroneDamaged(this, position);
    }
}
