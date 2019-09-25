using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE : MonoBehaviour
{
    string killCode = "";
    public GameObject debuff;
    float trapDuration;

    private void Start()
    {
        trapDuration = 4;
        // UPGRADE TNT 1 TIME
        if (EnhancementManager.instance.upgrades["TNT"].upgrade[0])
        {
            trapDuration += 2;
        }

        char c = name.ToCharArray()[0];
        if (c == 'F')
        {
            killCode = "Fire";
        }
        else if (c == 'I')
        {
            killCode = "Ice";
        }
        else if (c == 'P')
        {
            killCode = "Poison";
        }

        if (name.Contains("AOE"))
        {
            Destroy(gameObject, .5f);
        }

        if (name.Contains("FireTrap"))
        {
            Destroy(gameObject, trapDuration);
        }

        if (name.Contains("PoisonTrap") || name.Contains("IceTrap"))
        {
            StartCoroutine(TriggerTrap());
        }
    }

    IEnumerator TriggerTrap()
    {
        // UPGRADE TNT 3 SPEEDBOOST
        if (EnhancementManager.instance.upgrades["TNT"].upgrade[3])
        {
            HeroController h = FindObjectOfType<HeroController>();
            float initSpeed = h.speed;
            h.speed += 1;

            while(h.speed > initSpeed)
            {
                h.speed -= 1 / trapDuration * Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            h.speed = initSpeed;
        }
        else
        {
            yield return new WaitForSeconds(trapDuration);
        }

        GameObject graphics = transform.Find("Graphics").gameObject;
        for (int j = 0; j < 5; j++)
        {
            graphics.SetActive(false);
            yield return new WaitForSeconds(.1f);
            graphics.SetActive(true);
            yield return new WaitForSeconds(.1f);
        }

        transform.Find("Graphics").GetComponent<Animator>().SetTrigger("Despawn");
        yield return new WaitForSeconds(.5f);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (killCode.Equals("Fire"))
            {
                collision.GetComponent<Enemy>().Die(killCode);
            }
            else if (killCode.Equals("Ice") || killCode.Equals("Poison"))
            {
                Instantiate(debuff, collision.transform.Find("Debuff")).transform.localPosition = Vector3.zero;
            }
        }
    }
}
