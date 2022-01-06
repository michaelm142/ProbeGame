using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour
{
    public DoorState State;
    DoorState statePrev;

    public bool Locked
    {
        get { return State == DoorState.Locked || State == DoorState.Disabled; }

        set { State = value ? DoorState.Locked : State; }
    }

    MiniMapIcon mapIcon;

    Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        mapIcon = GetComponent<MiniMapIcon>();
        mapIcon.OnClicked.AddListener(delegate { FindObjectOfType<DoorControler>().ToggleDoorLocked(this); });
    }

    // Update is called once per frame
    void Update()
    {
        if (State != statePrev)
        {
            if (State == DoorState.Open)
            {
                mapIcon.Color = Color.white;
                anim.SetBool("Open", true);
            }
            else
            {
                mapIcon.Color = Color.white;
                anim.SetBool("Open", false);
            }

            if (State == DoorState.Locked)
            {
                anim.SetBool("Open", false);
                mapIcon.Color = Color.red;
            }
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
            if (FindObjectOfType<DoorControler>().enabled)
                mapIcon.MapIcon.GetComponent<Animator>().SetBool("Open", false);
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

