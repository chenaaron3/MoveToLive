using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    public GameObject missile;
    int numMissiles;

    private void Start()
    {
        numMissiles = 10;
        // UPGRADE MISSILE 0 NUM
        if(EnhancementManager.instance.upgrades["Missile"].upgrade[0])
        {
            numMissiles += 5;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HeroController hc = collision.GetComponent<HeroController>();
            hc.StartCoroutine(hc.CameraShake());

            float angle = 0;
            float margin = 360 / numMissiles;
            for (int j = 0; j < numMissiles; j++)
            {
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                angle += margin;

                GameObject m = Instantiate(missile, (Vector3)direction * .25f + transform.position, Quaternion.identity);
                m.transform.right = direction;
                m.GetComponent<Rigidbody2D>().velocity = direction;
            }

            Destroy(gameObject);
            WeaponSpawner.instance.SpawnWeapon();
            Statistics.instance.weaponsCollected++;
        }
    }
}
