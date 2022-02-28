using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneScanner : MonoBehaviour
{
    public float Value;
    public float MaxValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.tag == "Enemy")
                Value += Time.deltaTime;
            else if (Value > 0.0f)
                Value -= Time.deltaTime;
        }
        else if (Value > 0.0f)
            Value -= Time.deltaTime;

        Value = Mathf.Clamp(Value, 0.0f, MaxValue);
        if (Value == MaxValue && hit.collider.tag == "Enemy")
        {
            hit.collider.GetComponent<Outline>().enabled = true;
            hit.collider.GetComponent<EnemyBehavior>().Scanned = true;
        }
    }
}
