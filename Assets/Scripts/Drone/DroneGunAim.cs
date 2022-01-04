using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneGunAim : MonoBehaviour
{
    public float Range = 10.0f;

    private GameObject currentTarget;

    private bool fireing;

    private Animator anim;

    public AudioSource FireingSound;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var targets = GetTargets();
        if (targets.Count == 0)
        {
            float dot = Vector3.Dot(transform.parent.forward, transform.right);
            transform.localRotation *= Quaternion.AngleAxis(dot, Vector3.up);
            fireing = false;
            if (FireingSound.isPlaying)
                FireingSound.Stop();
        }
        else
        {
            if (!FireingSound.isPlaying)
                FireingSound.Play();
            fireing = true;
            targets.Sort(TargetHeruistic);
            currentTarget = targets[0];

            Vector3 L = currentTarget.transform.position - transform.position;
            float dot = Vector3.Dot(L, transform.right);
            if (Mathf.Abs(dot) > 0.01f)
                transform.localRotation *= Quaternion.AngleAxis(dot, Vector3.up);
        }

        anim.SetBool("Fireing", fireing);
    }

    protected virtual int TargetHeruistic(GameObject targetA, GameObject targetB)
    {
        int distA = (int)Vector3.Distance(transform.position, targetA.transform.position);
        int distB = (int)Vector3.Distance(transform.position, targetB.transform.position);

        if (distA < distB)
            return -1;

        return 1;
    }

    protected virtual List<GameObject> GetTargets()
    {
        List<GameObject> outval = new List<GameObject>();
        var probes = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var p in probes)
        {
            float distance = Vector3.Distance(p.transform.position, transform.position);
            if (distance < Range)
                outval.Add(p);
        }

        return outval;
    }
}
