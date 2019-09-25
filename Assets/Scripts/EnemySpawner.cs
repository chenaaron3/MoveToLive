using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    public Vector2Int topLeft;
    public int width;
    public int height;
    public List<GameObject> targetableEnemies;

    public GameObject[] enemies;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Statistics.instance.startTime = Time.time;
        targetableEnemies = new List<GameObject>();
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnEnemies());
    }

    public IEnumerator SpawnEnemies()
    {
        int waveIndex = (int)(Random.value * 5);

        // BLOB
        if (waveIndex == 0)
        {
            BlobSpawn(10, enemies[0]);
            if (GameManager.instance.difficulty >= 2)
            {
                BlobSpawn(3 + GameManager.instance.difficulty, enemies[1]);
            }
            if (GameManager.instance.difficulty >= 3)
            {
                BlobSpawn(3 + GameManager.instance.difficulty, enemies[2]);
            }
            if (GameManager.instance.difficulty >= 4)
            {
                BlobSpawn(3 + GameManager.instance.difficulty, enemies[3]);
            }

        }
        // CIRCLE
        else if (waveIndex == 1)
        {
            bool followPlayer = false;
            if (Random.value > .5f) followPlayer = true;

            CircularSpawn(20, enemies[0], followPlayer);
            if (GameManager.instance.difficulty >= 2)
            {
                CircularSpawn(3 + GameManager.instance.difficulty, enemies[0], followPlayer);
            }
            if (GameManager.instance.difficulty >= 3)
            {
                CircularSpawn(3 + GameManager.instance.difficulty, enemies[0], followPlayer);
            }
            if (GameManager.instance.difficulty >= 4)
            {
                CircularSpawn(3 + GameManager.instance.difficulty, enemies[0], followPlayer);
            }
        }
        // SIDE
        else if (waveIndex == 2)
        {
            bool followPlayer = false;
            if (Random.value > .5f) followPlayer = true;

            if (Random.value > .5f)
            {
                if (Random.value > .5f)
                {
                    LinearSpawn(15, enemies[0], followPlayer, new Vector2Int(-1, 0));
                }
                else
                {
                    LinearSpawn(15, enemies[0], followPlayer, new Vector2Int(1, 0));
                }
            }
            else
            {
                if (Random.value > .5f)
                {
                    LinearSpawn(30, enemies[0], followPlayer, new Vector2Int(0, 1));
                }
                else
                {
                    LinearSpawn(30, enemies[0], followPlayer, new Vector2Int(0, -1));
                }
            }
        }
        // RANDOM
        else if (waveIndex == 3)
        {
            RandomSpawn(20, enemies[0]);
            if (GameManager.instance.difficulty >= 2)
            {
                RandomSpawn(3 + GameManager.instance.difficulty, enemies[0]);
            }
            if (GameManager.instance.difficulty >= 3)
            {
                RandomSpawn(3 + GameManager.instance.difficulty, enemies[0]);
            }
            if (GameManager.instance.difficulty >= 4)
            {
                RandomSpawn(3 + GameManager.instance.difficulty, enemies[0]);
            }
        }
        // PERIMETER
        else if (waveIndex == 4)
        {
            bool followPlayer = false;
            if (Random.value > .5f) followPlayer = true;
            PerimeterSpawn(20, 10, enemies[0], followPlayer);
        }

        // scales difficulty 1-5 to 5-10 second delays
        yield return new WaitForSeconds(scale(1, 5, 10, 5, GameManager.instance.difficulty));

        StartCoroutine(SpawnEnemies());
    }

    // spawns a blob
    public void BlobSpawn(int num, GameObject enemy)
    {
        Vector2 origin = topLeft + new Vector2(Random.value * width, -1 * Random.value * height);
        for (int j = 0; j < num; j++)
        {
            Vector2 offset = new Vector2(Random.value - .5f, -1 * Random.value + .5f);
            GameObject e = Instantiate(enemy, origin + offset, Quaternion.identity);
            e.GetComponent<Enemy>().type = "Targeter";
        }
    }

    // spawns in a circle
    public void CircularSpawn(int num, GameObject enemy, bool followPlayer)
    {
        Vector2 origin = topLeft + new Vector2(Random.value * width, -1 * Random.value * height);
        float angle = 0;
        float margin = 360 / num;
        for (int j = 0; j < num; j++)
        {
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            angle += margin;
            Enemy e = Instantiate(enemy, origin + direction, Quaternion.identity).GetComponent<Enemy>();

            if (followPlayer)
            {
                e.type = "Targeter";
            }
            else
            {
                e.type = "Linear";
                e.initVelocity = direction;
            }
        }
    }

    // spawns at a wall
    public void LinearSpawn(int num, GameObject enemy, bool followPlayer, Vector2Int initWall)
    {
        Vector2 offset = Vector2.zero;
        Vector2 origin = topLeft + new Vector2(.5f, -.5f);
        if (initWall.x == 0) // --
        {
            offset = new Vector2(width * 1.0f / num, 0);
            if (initWall.y == 1) // top wall going down
            {
                origin += Vector2.zero;
            }
            else // bottom wall goign up
            {
                origin += new Vector2(0, (height - 1) * -1);
            }
        }
        else // |
        {
            offset = new Vector2(0, height * -1.0f / num);
            if (initWall.x == 1) // right wall going left
            {
                origin += new Vector2(width - 1, 0);
            }
            else // left wall going right
            {
                origin += Vector2.zero;
            }
        }
        for (int j = 0; j < num; j++)
        {
            Enemy e = Instantiate(enemy, origin + offset * j, Quaternion.identity).GetComponent<Enemy>();

            if (followPlayer)
            {
                e.type = "Targeter";
            }
            else
            {
                e.type = "Linear";
                e.initVelocity = initWall * -1;
            }
        }
    }

    // spawns randomly
    public void RandomSpawn(int num, GameObject enemy)
    {
        for (int j = 0; j < num; j++)
        {
            Vector2 origin = topLeft + new Vector2(Random.value * width, -1 * Random.value * height);
            Enemy e = Instantiate(enemy, origin, Quaternion.identity).GetComponent<Enemy>();
            e.type = "Wanderer";
            e.initVelocity = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1);
        }
    }

    // spawns at perimeter
    public void PerimeterSpawn(int numx, int numy, GameObject enemy, bool followPlayer)
    {
        Vector2Int direction = new Vector2Int(1, 0);
        LinearSpawn(numy, enemy, followPlayer, direction);
        direction = new Vector2Int(-1, 0);
        LinearSpawn(numy, enemy, followPlayer, direction);
        direction = new Vector2Int(0, 1);
        LinearSpawn(numy, enemy, followPlayer, direction);
        direction = new Vector2Int(0, -1);
        LinearSpawn(numy, enemy, followPlayer, direction);
    }

    float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }
}
