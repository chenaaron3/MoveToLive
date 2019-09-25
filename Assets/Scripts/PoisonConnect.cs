using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonConnect : MonoBehaviour
{
    public GameObject source;
    LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        Destroy(gameObject, .5f);
    }

    private void Update()
    {
        if (source != null)
        {
            lr.SetPositions(new Vector3[] { transform.position, source.transform.position });
        }
    }
}
