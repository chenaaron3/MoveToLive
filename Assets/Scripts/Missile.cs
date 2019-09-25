using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject aoe;

    Rigidbody2D rb;
    GameObject target;
    float acceleration = .5f;
    float velocity = 1f;
    float turningSpeed = .1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject[] targets = EnemySpawner.instance.targetableEnemies.ToArray();
        if (targets.Length > 0)
        {
            target = targets[(int)(Random.value * targets.Length)];
        }
    }

    private void Update()
    {
        if (rb.velocity.magnitude < 4)
        {
            velocity += acceleration * Time.deltaTime;
            rb.velocity = transform.right * velocity;
        }

        if (target == null)
        {
            // UPGRADE MISSILE 3 REHONE
            if (EnhancementManager.instance.upgrades["Missile"].upgrade[3])
            {
                GameObject[] targets = EnemySpawner.instance.targetableEnemies.ToArray();
                if (targets.Length > 0)
                {
                    target = targets[(int)(Random.value * targets.Length)];
                }
            }

            return;
        }

        try
        {
            Vector2 facing = transform.right;
            Vector2 targetDirection = target.transform.position - transform.position;

            transform.right = Vector2.Lerp(facing, targetDirection, turningSpeed).normalized;
            rb.velocity = transform.right * velocity;
        }
        catch
        {
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            // UPGRADE MISSILE 2 IGNORE
            if (EnhancementManager.instance.upgrades["Missile"].upgrade[2])
            {
                // if non-debuffed enemy
                if (collision.transform.Find("Debuff").childCount == 0)
                {
                    Enemy e = collision.GetComponent<Enemy>();
                    Destroy(gameObject);
                    GameObject a = Instantiate(aoe, transform.position, Quaternion.identity);
                    // UPGRADE MISSILE 1 SCALE
                    if (EnhancementManager.instance.upgrades["Missile"].upgrade[1])
                    {
                        a.transform.localScale *= 1.5f;
                    }
                }
                // else ignore enemy
            }
            else
            {
                Enemy e = collision.GetComponent<Enemy>();
                Destroy(gameObject);
                GameObject a = Instantiate(aoe, transform.position, Quaternion.identity);
                // UPGRADE MISSILE 1 SCALE
                if (EnhancementManager.instance.upgrades["Missile"].upgrade[1])
                {
                    a.transform.localScale *= 1.5f;
                }
            }
        }
    }
}
