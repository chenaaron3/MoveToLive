using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormie : Enemy
{
    public override void ExtendedStart()
    {
        speed = 1;
        rb.velocity = initVelocity * speed;
    }

    private void Update()
    {
        if (!awake)
        {
            return;
        }

        if (type.Equals("Targeter"))
        {
            if (target == null)
            {
                Die("Fire");
            }
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 
                scale(0, Mathf.Sqrt(Mathf.Pow(EnemySpawner.instance.width, 2) + Mathf.Pow(EnemySpawner.instance.height, 2)), 
                .5f, 2, (target.transform.position - transform.position).magnitude) * speed * Time.deltaTime);

            // lock on to leader 
            if (transform.position.Equals(target.transform.position))
            {
                transform.parent = target.transform;
                col.enabled = true;
                SpriteRenderer esr = transform.Find("Graphics").GetComponent<SpriteRenderer>();
                Color c = esr.color;
                c.a = 255;
                esr.color = c;
            }
        }
        else if (type.Equals("Wanderer"))
        {
            if (rb.velocity != Vector2.zero)
            {
                rb.velocity = initVelocity + new Vector2(Mathf.PerlinNoise(Time.time + gameObject.GetInstanceID(), 0) * 2 - 1,
    Mathf.PerlinNoise(0, Time.time + gameObject.GetInstanceID()) * 2 - 1) * speed;
            }
        }
    }

    // scales distance into speed multiplier
    // high distance => high multiplier
    // low distance => low multiplier
    // distance range: 
    float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }
}
