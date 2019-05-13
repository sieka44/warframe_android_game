using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarframeCharacterScript : MonoBehaviour
{
    Spawner spawner;
    int health;
    int shields;
    int MAX_HEALTH = 300;
    int MAX_SHIELDS = 100;

    Transform healthBar;
    Transform shieldBar;
    Text healthBarText;
    Text shieldBarText;


    private void updateStatusBar()
    {
        int allHitPoints = MAX_HEALTH + MAX_SHIELDS;

        float healthLevelInPercents = (float)System.Math.Round((float)health / MAX_HEALTH, 2)/2;
        float shieldLevelInPercents = (float)System.Math.Round((float)shields / MAX_SHIELDS, 2)/2;

        healthBar.localScale = new Vector3(healthLevelInPercents, 1f, 1f);
        shieldBar.localScale = new Vector3(shieldLevelInPercents, 1f, 1f);

        healthBarText.text = health.ToString();
        shieldBarText.text = shields.ToString();
    }

    public void getDamageFromEnemy(int damage)
    {
        if(health > 0)
        {
            shields -= damage;
            damage = Mathf.Clamp(shields * -1, 0, Mathf.Abs(shields));
            shields = Mathf.Clamp(shields, 0, MAX_SHIELDS);
            health = Mathf.Clamp(health - damage, 0, MAX_HEALTH);

            if (health <= 0)
            {
                spawner.stopTheGame();
                GameObject.Find("GameOverTable").GetComponent<Animation>().Play();
                gameObject.SetActive(false);
            }
        }
        updateStatusBar();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GameObject.Find("UI/WarframeStatusContainer/HealthBar/HealthPointsBar").transform;
        shieldBar = GameObject.Find("UI/WarframeStatusContainer/HealthBar/ShieldPointsBar").transform;
        healthBarText = GameObject.Find("UI/WarframeStatusContainer/HealthBar/Canvas/TextHealth").GetComponent<Text>();
        shieldBarText = GameObject.Find("UI/WarframeStatusContainer/HealthBar/Canvas/TextShield").GetComponent<Text>();

        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        health = MAX_HEALTH;
        shields = MAX_SHIELDS;
        updateStatusBar();

    }

    // Update is called once per frame
    void Update()
    {

    }
}