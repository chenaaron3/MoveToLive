using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Enemy e = collision.GetComponent<Enemy>();
            if (e.type.Equals("Linear") || e.type.Equals("Wanderer"))
            {
                collision.gameObject.GetComponent<Enemy>().Die("Fire");
            }
        }
        else if(collision.gameObject.layer == 10)
        {
            Destroy(collision.gameObject);
        }
    }
}
