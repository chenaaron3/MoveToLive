using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff : MonoBehaviour
{
    public GameObject poisonConnect;
    Enemy myEnemy;
    public bool frozen = false;
    float freezeTime = 5;

    private void Start()
    {
        name = name.Replace("(Clone)", "").Trim();

        myEnemy = GetComponentInParent<Enemy>();

        // UPGRADE ICE 1 DEBUFF KILL
        if (EnhancementManager.instance.upgrades["Ice"].upgrade[1])
        {
            foreach (Debuff d in transform.parent.GetComponentsInChildren<Debuff>())
            {
                if(d.frozen)
                {
                    Destroy(gameObject);
                    myEnemy.Die("Ice");
                }
            }
        }

        // Destroy if is dupe
        foreach (Transform child in transform.parent)
        {
            if (child != transform && child.name.Equals(name))
            {
                Destroy(gameObject);
                return;
            }
        }

        myEnemy.debuffed = true;

        if (name.ToCharArray()[0] == 'P')
        {
            StartCoroutine(PoisonDebuff());
        }
        else if (name.ToCharArray()[0] == 'F')
        {
            StartCoroutine(FreezeDebuff());
        }
    }

    IEnumerator PoisonDebuff()
    {
        // slows enemy
        myEnemy.rb.velocity *= .1f;

        // UPGRADE POISON 0 FAST KILL
        if (EnhancementManager.instance.upgrades["Poison"].upgrade[0])
        {
            yield return new WaitForSeconds(1);
        }
        else
        {
            yield return new WaitForSeconds(2);
        }

        // UPGRADE POISON 1 SINGLE SPREAD
        if (EnhancementManager.instance.upgrades["Poison"].upgrade[1])
        {
            if (Random.value < .5f)
            {
                ContactFilter2D cf = new ContactFilter2D();
                cf.layerMask = 1 << 8;
                Collider2D[] res = new Collider2D[20];
                Physics2D.OverlapCircle(transform.position, 1, cf, res);

                foreach (Collider2D c in res)
                {
                    if (c == null)
                    {
                        break;
                    }

                    try
                    {
                        GameObject infected = c.gameObject;
                        GameObject debuff = infected.transform.Find("Debuff").gameObject;
                        // go next if already infected
                        if (debuff.transform.Find("PoisonDebuff") != null)
                        {
                            continue;
                        }
                        // gets guarantee infection
                        else
                        {
                            Instantiate(poisonConnect, debuff.transform).transform.localPosition = Vector3.zero;
                            infected.GetComponentInChildren<PoisonConnect>().source = transform.parent.gameObject;
                            Instantiate(gameObject, debuff.transform).transform.localPosition = Vector3.zero;
                            break;
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        // UPGRADE POISON 2 CIRCLE SPREAD S
        if (EnhancementManager.instance.upgrades["Poison"].upgrade[2])
        {
            if (Random.value < .25f)
            {
                ContactFilter2D cf = new ContactFilter2D();
                cf.layerMask = 1 << 8;
                Collider2D[] res = new Collider2D[20];
                Physics2D.OverlapCircle(transform.position, .75f, cf, res);

                foreach (Collider2D c in res)
                {
                    if (c == null)
                    {
                        break;
                    }

                    try
                    {
                        GameObject infected = c.gameObject;
                        GameObject debuff = infected.transform.Find("Debuff").gameObject;
                        Instantiate(poisonConnect, debuff.transform).transform.localPosition = Vector3.zero;
                        infected.GetComponentInChildren<PoisonConnect>().source = transform.parent.gameObject;
                        Instantiate(gameObject, debuff.transform).transform.localPosition = Vector3.zero;
                    }
                    catch
                    {
                    }
                }
            }
        }

        // UPGRADE POISON 3 CIRCLE SPREAD L
        if (EnhancementManager.instance.upgrades["Poison"].upgrade[3])
        {
            if (Random.value < .10f)
            {
                ContactFilter2D cf = new ContactFilter2D();
                cf.layerMask = 1 << 8;
                Collider2D[] res = new Collider2D[20];
                Physics2D.OverlapCircle(transform.position, 1.25f, cf, res);

                foreach (Collider2D c in res)
                {
                    if (c == null)
                    {
                        break;
                    }

                    try
                    {
                        GameObject infected = c.gameObject;
                        GameObject debuff = infected.transform.Find("Debuff").gameObject;
                        Instantiate(poisonConnect, debuff.transform).transform.localPosition = Vector3.zero;
                        infected.GetComponentInChildren<PoisonConnect>().source = transform.parent.gameObject;
                        Instantiate(gameObject, debuff.transform).transform.localPosition = Vector3.zero;
                    }
                    catch
                    {
                    }
                }
            }
        }

        // kills enemy
        Destroy(gameObject);
        myEnemy.Die("Poison");
    }

    IEnumerator FreezeDebuff()
    {
        // stops the player
        GameObject player = myEnemy.target;
        myEnemy.target = myEnemy.gameObject;
        myEnemy.rb.velocity = Vector2.zero;
        // makes enemy unharmable
        myEnemy.tag = "Untagged";
        // makes enemy killable
        frozen = true;

        // UPGRADE ICE 0 FREEZE TIME
        if (EnhancementManager.instance.upgrades["Ice"].upgrade[0])
        {
            yield return new WaitForSeconds(freezeTime + 3);
        }
        else
        {
            yield return new WaitForSeconds(freezeTime);
        }

        // flashes when about to unfreeze
        Light l = GetComponent<Light>();
        for (int j = 0; j < 3; j++)
        {
            l.range *= 1.2f;
            yield return new WaitForSeconds(.1f);
            l.range /= 1.2f;
            yield return new WaitForSeconds(.1f);
        }

        // moves enemy again
        myEnemy.target = player;
        myEnemy.rb.velocity = myEnemy.initVelocity;
        // makes enemy prone to being debuffed again
        myEnemy.debuffed = false;
        // makes enemy harmable
        myEnemy.tag = "Enemy";
        // makes enemy untouchable
        frozen = false;
        // destroys debuff
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if player touches and is frozen
        if (collision.CompareTag("Player") && frozen)
        {
            Destroy(gameObject);
            myEnemy.Die("Ice");
        }
    }
}
