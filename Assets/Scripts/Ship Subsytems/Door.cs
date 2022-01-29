using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public DoorState State = DoorState.Closed;
    DoorState statePrev;

    public bool Locked
    {
        get { return State == DoorState.Locked || State == DoorState.Disabled; }

        set { State = value ? DoorState.Locked : State; }
    }

    MiniMapIcon mapIcon;

    public Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
        mapIcon = GetComponent<MiniMapIcon>();
        mapIcon.OnClicked.AddListener(delegate { FindObjectOfType<DoorControler>().ToggleDoorLocked(this); });
    }

    // Update is called once per frame
    void Update()
    {
        if (statePrev != State)
        {
            if (FindObjectOfType<DoorControler>().enabled)
                mapIcon.MapIcon.GetComponent<Animator>().SetBool("Open", State == DoorState.Open);
            
            anim.SetBool("Open", State == DoorState.Open);

            if (State == DoorState.Open || State == DoorState.Closed)
                mapIcon.Color = Color.white;
            if (State == DoorState.Locked)
                mapIcon.Color = Color.red;
            else if (State == DoorState.Disabled)
                mapIcon.Color = Color.gray;
        }

        statePrev = State;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!Locked && (other.tag == "Player" || other.tag == "Enemy"))
        {
            State = DoorState.Open;
            if (FindObjectOfType<DoorControler>().enabled)
                mapIcon.MapIcon.GetComponent<Animator>().SetBool("Open", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!Locked && (other.tag == "Player" || other.tag == "Enemy"))
        {
            State = DoorState.Closed;
        }
    }

    /// <summary>
    /// Called from the animation system
    /// </summary>
    public void UpdateNavMeshObstacle()
    {
        GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = State == DoorState.Open ? false : true;
    }

    public enum DoorState
    {
        Open = 0,
        Closed = 1,
        Locked = 2,
        Disabled = 4,
    }
}

