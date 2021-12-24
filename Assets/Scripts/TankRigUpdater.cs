using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRigUpdater : MonoBehaviour
{
    public List<WheelCollider> Wheels;

    public List<Transform> Bones;

    Vector3[] WheelsStartPositions;
    Vector3[] BonesStartPositions;

    Quaternion[] BonesStartRotations;

    // Start is called before the first frame update
    void Start()
    {
        WheelsStartPositions = new Vector3[Wheels.Count];
        BonesStartPositions = new Vector3[Bones.Count];

        BonesStartRotations = new Quaternion[Wheels.Count];
        for (int i =0;i<Wheels.Count;i++)
        {
            BonesStartRotations[i] = Bones[i].transform.localRotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        WheelHit wheelHit;

        for (int i = 0; i < Bones.Count; i++)
        {
            if (Wheels[i].GetGroundHit(out wheelHit))
            {
                Debug.Log("Wheel Hit force: " + wheelHit.force.ToString());
                Bones[i].rotation = Quaternion.Slerp(Bones[i].rotation, Quaternion.LookRotation(transform.forward, wheelHit.normal), Time.deltaTime * (wheelHit.force / 10000.0f));
            }
            else
            {
                Bones[i].localRotation = Quaternion.Slerp(Bones[i].localRotation, BonesStartRotations[i], Quaternion.Angle(Bones[i].localRotation, BonesStartRotations[i]));
            }
        }
    }
}
