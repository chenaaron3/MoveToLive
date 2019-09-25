using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyLeader : Enemy
{
    public GameObject[] branches;
    public GameObject enemy;
    public float rotateSpeed = 90;

    public override void ExtendedStart()
    {
        int branchCounter = 0;
        EnemyNormie[] enemies = FindObjectsOfType<EnemyNormie>();
        enemies = enemies.OrderBy(x => Vector2.Distance(this.transform.position, x.transform.position)
   ).ToArray();

        foreach (Enemy e in enemies)
        {
            // if all branches are filled
            if (branchCounter == branches.Length)
            {
                break;
            }

            // if enemy is already targeting another leader
            if (e.target != null && e.target.CompareTag("Enemy"))
            {
                continue;
            }

            // leads enemy to form up with leader
            e.target = branches[branchCounter];
            e.speed *= 5;
            e.type = "Targeter";
            // makes the enemies unharmful
            e.col.enabled = false;
            // lowers the alpha as visual queue
            SpriteRenderer esr = e.transform.Find("Graphics").GetComponent<SpriteRenderer>();
            Color c = esr.color;
            c.a /= 2;
            esr.color = c;
            // goes to next branch
            branchCounter++;
        }

        //if no existing enemies, make own
        if (branchCounter < branches.Length)
        {
            for (int j = branchCounter; j < branches.Length; j++)
            {
                Instantiate(enemy, branches[j].transform).transform.localPosition = Vector3.zero;
            }
        }
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime));
    }
}
