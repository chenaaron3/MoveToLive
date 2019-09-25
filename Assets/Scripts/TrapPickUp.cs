using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPickUp : MonoBehaviour
{
    public GameObject trap;
    public GameObject missile;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // if fire, just spawn in world
            if (name.Contains("Fire"))
            {
                GameObject t = Instantiate(trap, transform.position, Quaternion.identity);

                // UPGRADE TNT 1 RANGE
                if (EnhancementManager.instance.upgrades["TNT"].upgrade[1])
                {
                    t.transform.localScale *= 1.5f;
                }
            }
            // if not, spawn under character
            else if (name.Contains("Ice") || name.Contains("Poison"))
            {
                GameObject t = Instantiate(trap, collision.transform);
                t.transform.localPosition = Vector3.zero;

                // UPGRADE TNT 1 RANGE
                if (EnhancementManager.instance.upgrades["TNT"].upgrade[1])
                {
                    t.transform.localScale *= 1.5f;
                }
            }

            // UPGRADE TNT 2 MISSILES
            if (EnhancementManager.instance.upgrades["TNT"].upgrade[2])
            {
                float angle = 0;
                float margin = 360 / 5;
                for (int j = 0; j < 5; j++)
                {
                    Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                    angle += margin;

                    GameObject m = Instantiate(missile, (Vector3)direction * .25f + transform.position, Quaternion.identity);
                    m.transform.right = direction;
                    m.GetComponent<Rigidbody2D>().velocity = direction;
                }
            }

            Destroy(gameObject);
            WeaponSpawner.instance.SpawnWeapon();
            Statistics.instance.weaponsCollected++;
        }
    }
}
