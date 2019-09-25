using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    public static Statistics instance;

    public float startTime;
    public int weaponsCollected;
    public int enemiesKilled;

    public Text timeText;
    public Text weaponsText;
    public Text enemiesText;

    public Text timeTextHS;
    public Text weaponsTextHS;
    public Text enemiesTextHS;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {       
        startTime = Time.time;
        weaponsCollected = 0;
        enemiesKilled = 0;
    }

    public void DisplayStats()
    {
        transform.Find("Stats").gameObject.SetActive(true);
        float time = Time.time - startTime;
        timeText.text += time;
        weaponsText.text += weaponsCollected;
        enemiesText.text += enemiesKilled;

        float oldHighscore = PlayerPrefs.GetFloat("timehs", 0.0f);
        if (time > oldHighscore)
        {
            PlayerPrefs.SetFloat("timehs", time);
        }
        oldHighscore = PlayerPrefs.GetInt("weaponshs", 0);
        if (weaponsCollected > oldHighscore)
        {
            PlayerPrefs.SetInt("weaponshs", weaponsCollected);
        }
        oldHighscore = PlayerPrefs.GetInt("enemieshs", 0);
        if (enemiesKilled > oldHighscore)
        {
            PlayerPrefs.SetInt("enemieshs", enemiesKilled);
        }

        timeTextHS.text += PlayerPrefs.GetFloat("timehs", 0.0f);
        weaponsTextHS.text += PlayerPrefs.GetInt("weaponshs", 0);
        enemiesTextHS.text += PlayerPrefs.GetInt("enemieshs", 0);
    }

    private void Update()
    {
        if(transform.Find("Stats").gameObject.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.ReloadScene();
        }
    }
}
