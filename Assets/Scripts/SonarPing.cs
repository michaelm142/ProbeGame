using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SonarPing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var sphere = GetComponent<SphereCollider>();
        var overlap = Physics.OverlapSphere(transform.position + sphere.center, sphere.radius).ToList().FindAll(c => c.tag == "Enemy" && c.GetComponent<EnemyBehavior>() != null);
        foreach (var enemy in overlap)
        {
            Vector3 randomVelocity = Vector3.up * Random.Range(-1.0f, 1.0f) + Vector3.right * Random.Range(-1.0f, 1.0f) + Vector3.forward * Random.Range(-1.0f, 1.0f);
            float randomSpeed = Random.Range(-10.0f, 10.0f);
            var alertState = new Alert(enemy.GetComponent<EnemyBehavior>(), transform.position, randomVelocity.normalized * randomSpeed);
            enemy.GetComponent<EnemyBehavior>().SetCurrentState(alertState);
        }
    }
}
