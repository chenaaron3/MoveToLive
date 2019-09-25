using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGusher : Enemy
{
    public GameObject basicEnemy;
    private int gushPower = 7;

    public override void ExtendedStart()
    {
        StartCoroutine(Gush());
    }

    private IEnumerator Gush()
    {
        for (int j = 0; j < gushPower; j++)
        {
            transform.localScale *= 1.2f;
            yield return new WaitForSeconds(1);
        }

        Destroy(gameObject);

        for (int j = 0; j < gushPower * 2; j++)
        {
            Vector2 offset = new Vector2(Random.value - .5f, -1 * Random.value + .5f);
            GameObject e = Instantiate(basicEnemy, (Vector2)transform.position + offset, Quaternion.identity);
            Vector2 dir = (e.transform.position - transform.position).normalized;
            e.GetComponent<Rigidbody2D>().AddForce(dir * gushPower / 4, ForceMode2D.Impulse);
            e.GetComponent<Enemy>().type = "Targeter";

            if (transform.Find("Debuff").childCount != 0)
            {
                foreach (Transform child in transform.Find("Debuff"))
                {
                    Instantiate(child.gameObject, e.transform).transform.localPosition = Vector3.zero;
                }
            }
        }
    }
}
