using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPush : MonoBehaviour
{
    float pushStrength = 3;
    float pushOffset;

    private void Start()
    {
        pushOffset = GetComponent<CircleCollider2D>().radius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            Vector2 dir = (collision.transform.position - transform.position).normalized;
            collision.gameObject.transform.position = transform.position + (Vector3)dir * pushOffset * 2;
            collision.GetComponent<Rigidbody2D>().AddForce(dir * pushStrength, ForceMode2D.Impulse);
        }
    }
}
