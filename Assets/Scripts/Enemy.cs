using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Vector2 initVelocity;
    public string type; // Wanderer walks around randomly, Targeter follows player, Linear walks around linearly
    public GameObject target;
    public float speed;
    Animator anim;
    public Rigidbody2D rb;
    protected bool awake = false;
    public Collider2D col;
    public bool debuffed = false;
    int deathcode = -1;

    private void Start()
    {
        anim = transform.Find("Graphics").GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        col.enabled = false;
        target = GameObject.FindGameObjectWithTag("Player");
        EnemySpawner.instance.targetableEnemies.Add(gameObject);

        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(1);
        awake = true;
        col.enabled = true;
        ExtendedStart();
    }

    public void Die(string cause)
    {
        col.enabled = false;
        if(cause.Equals("Fire"))
        {
            anim.SetFloat("DeathCode", 0);
            deathcode = 0;
        }
        else if (cause.Equals("Ice"))
        {
            anim.SetFloat("DeathCode", 1);
            deathcode = 1;
        }
        else if (cause.Equals("Poison"))
        {
            anim.SetFloat("DeathCode", 2);
            deathcode = 2;
        }
        anim.SetTrigger("Die");
        EnemySpawner.instance.targetableEnemies.Remove(gameObject);
        Destroy(gameObject, .417f);
        Statistics.instance.enemiesKilled++;
    }

    private void OnDestroy()
    {
        if(deathcode == 1)
        {
            HeroController.instance.IceCombo++;
        }
    }

    public abstract void ExtendedStart();
}
