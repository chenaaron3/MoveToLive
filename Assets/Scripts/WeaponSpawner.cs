using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public static WeaponSpawner instance;
    public Vector2Int topLeft;
    public int width;
    public int height;
    public GameObject[] weapons; // FT, FM, IT, IM, PT, PM

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        topLeft = EnemySpawner.instance.topLeft;
        width = EnemySpawner.instance.width;
        height = EnemySpawner.instance.height;

        SpawnWeapon();
    }

    public void SpawnWeapon()
    {
        Vector2 origin = topLeft + new Vector2(Random.value * width, -1 * Random.value * height);
        Instantiate(weapons[(int)(Random.value * weapons.Length)], origin, Quaternion.identity);
    }
}