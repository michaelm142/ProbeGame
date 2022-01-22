using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCameraShake : MonoBehaviour
{
    private Animator anim;

    private Vector3 positionPrev;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        positionPrev = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        int diceRoll = Random.Range(0, 100);
        anim.SetInteger("DiceRoll", diceRoll);

        Vector3 deltaPosition = positionPrev - transform.position;
        float speed = deltaPosition.magnitude;
        var drone = GetComponentInParent<Drone>();
        anim.SetFloat("Blend", speed / Time.deltaTime);

        positionPrev = transform.position;
    }
}
