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

    public GameObject connectedSubsystem;
    private static GameObject ExplosionPrefab;

    public float MaxHealth = 30.0f;
    public float Health;
    public float MoveSpeed = 1.5f;

    public bool hacking { get { return connectedSubsystem != null; } }

    // Start is called before the first frame update
    void Awake()
    {
        Camera = transform.GetComponentInChildren<Camera>();

        var ring = Resources.Load<Texture2D>("SelectRing");
        Health = MaxHealth;

        if (ExplosionPrefab == null)
            ExplosionPrefab = Resources.Load<GameObject>("Effects/DroneExplode");
    }

    private void Start()
    {
        var di = gameObject.AddComponent<DroneInventory>();
    }

    private void OnDestroy()
    {
        if (Health > 0.0f)
            return;


        var di = GetComponent<DroneInventory>();
        var obj = new GameObject("Drone Inventory");
        DontDestroyOnLoad(obj);
        var obj_di = obj.AddComponent<DroneInventory>();
        obj_di.Energy = di.Energy;
        obj_di.Metal = di.Metal;

        FindObjectOfType<DroneController>().DroneDestroyed(this);
    }

    // Update is called once per frame
    void Update()
    {
        var clickNavigation = FindObjectOfType<MiniMapClickNavigation>();

        if (Health <= 0.0f)
        {
            var explosion = Instantiate(ExplosionPrefab);
            explosion.transform.position = transform.position;
            Destroy(gameObject);
        }
    }

    public void SetActive()
    {
        CameraController.instance.ActiveCamera = Camera;
    }

    public void SetInactive()
    {
        CameraController.instance.ActiveCamera = null;
    }

    public void Damage(object[] info)
    {
        float ammount = (float)info[0];
        Vector3 position = (Vector3)info[1];
        Health -= ammount;
        DroneController.Instance.DroneDamaged(this, position);
    }
}
