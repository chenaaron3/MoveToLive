using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnhancementManager : MonoBehaviour
{
    public static EnhancementManager instance;

    public Dictionary<string, Upgrade> upgrades;
    public Text descriptionText;
    public GameObject enhancerPanel;

    // MISSILES: 1. increase num missiles, 2. increase aoe, 3. ignore debuff enemies, 4. target new enemies
    // TNT: 1. increase time, 2. increase range, 3. spawn missiles, 4. add speed boost
    // POISON: 1. kill faster, 2. infect 1, 3. infection explosion, 4. infection circle
    // ICE: 1. freeze longer, 2. debuff kill, 3. kill 3 for ms boost, 4. kill 6 for ice shield

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        upgrades = new Dictionary<string, Upgrade>();
        upgrades.Add("Missile", new Upgrade());
        upgrades.Add("TNT", new Upgrade());
        upgrades.Add("Poison", new Upgrade());
        upgrades.Add("Ice", new Upgrade());

        Upgrade u = upgrades["Missile"];
        u.description[0] = "Amount of missiles increased by 5.";
        u.description[1] = "Range of missile impact increased by 50%.";
        u.description[2] = "Missiles will ignore debuffed enemies.";
        u.description[3] = "Missiles will target new enemies if their current one dies.";

        u = upgrades["TNT"];
        u.description[0] = "Duration of TNT increased by 2 seconds.";
        u.description[1] = "Range of TNT increased by 50%.";
        u.description[2] = "Triggering a TNT spawns 5 of its respective missiles.";
        u.description[3] = "Gain a burst of movement speed when equipped with a TNT.";

        u = upgrades["Poison"];
        u.description[0] = "Poison kills enemies 50% faster";
        u.description[1] = "When killed from poison, get 50% chance to infect 1 nearby enemy.";
        u.description[2] = "When killed from poison, get 25% chance to infect enemies in a small circle.";
        u.description[3] = "When killed from poison, get 10% chance to infect enemies in a large circle.";

        u = upgrades["Ice"];
        u.description[0] = "Enemies will be frozen for 3 more seconds.";
        u.description[1] = "Debuffing a frozen enemy will instantly kill them.";
        u.description[2] = "Run over 6 frozen enemies to gain a thin ice shield that slightly blocks enemies.";
        u.description[3] = "Run over 3 frozen enemies to gain a boost of movement speed.";

        u = upgrades["Ice"];
        u.upgrade[3] = true;
    }

    // called when difficulty level is increased
    public void PromptEnhance()
    {
        enhancerPanel.SetActive(true);
        EnemySpawner.instance.StopAllCoroutines();
    }

    public void Enhance(string type)
    {
        // opens up description panel
        descriptionText.transform.parent.gameObject.SetActive(true);

        bool[] upgradeArray = upgrades[type].upgrade;
        string[] descriptionArray = upgrades[type].description;

        // checks if an upgrade exists for this category
        bool upgradeExists = false;
        for(int j = 0; j < upgradeArray.Length; j++)
        {
            if(!upgradeArray[j])
            {
                upgradeExists = true;
                break;
            }
        }
        if (!upgradeExists)
        {
            descriptionText.text = "There are no more upgrades for " + type + "!";
            return;
        }

        // upgrades a random upgrade
        int upgradeIndex = (int)(Random.value * 4);
        // while upgradeIndex resolves to true
        while(upgradeArray[upgradeIndex])
        {
            upgradeIndex = (int)(Random.value * 4);
        }
        upgradeArray[upgradeIndex] = true;
        descriptionText.text = descriptionArray[upgradeIndex];
    }

    public void CloseEnhance()
    {
        // starts spawning enemies again
        EnemySpawner.instance.StartCoroutine(EnemySpawner.instance.SpawnEnemies());
        GameManager.instance.StartCoroutine(GameManager.instance.IncreaseDifficulty());
    }

    public class Upgrade
    {
        public bool[] upgrade;
        public string[] description;

        public Upgrade()
        {
            upgrade = new bool[4];
            description = new string[4];
        }
    }
}
