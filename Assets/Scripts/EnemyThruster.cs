using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThruster : Enemy
{
    LineRenderer lr;

    public override void ExtendedStart()
    {
        lr = GetComponent<LineRenderer>();
        StartCoroutine(Charge());
    }

    IEnumerator Charge()
    {
        lr.enabled = true;
        float chargeTime = 0;
        Vector3 direction;
        while (chargeTime < 3)
        {
            direction = (target.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + direction * .25f, direction, 100, 1 << 8 | 1 << 9);
            lr.SetPositions(new Vector3[] { transform.position, hit.point });

            chargeTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        lr.enabled = false;

        direction = (target.transform.position - transform.position).normalized;
        rb.AddForce(direction * 6, ForceMode2D.Impulse);

        yield return new WaitForSeconds(3);
        StartCoroutine(Charge());
    }
}
