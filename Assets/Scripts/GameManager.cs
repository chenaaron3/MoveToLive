using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Statistics gameStats;
    public GameObject gameOver;
    public int difficulty;
    public bool running;
    int totalWeapons = 1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        running = true;
        difficulty = 1;
        StartCoroutine(IncreaseDifficulty());
    }

    public IEnumerator IncreaseDifficulty()
    {
        yield return new WaitForSeconds(30);
        difficulty++;
        // cap weapons at 4
        if (totalWeapons < 4)
        {
            WeaponSpawner.instance.SpawnWeapon();
            totalWeapons++;
        }

        EnhancementManager.instance.PromptEnhance();
    }

    public void GameOver()
    {
        if (running)
        {
            running = false;
            gameOver.SetActive(true);
            Invoke("LoadStats", 1);
        }
    }

    private void LoadStats()
    {
        gameOver.SetActive(false);
        gameStats.DisplayStats();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
