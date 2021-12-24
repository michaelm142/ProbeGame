using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAnimationControl : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Drive", Vector3.Dot(transform.forward, GetComponent<Rigidbody>().velocity));
    }
}
