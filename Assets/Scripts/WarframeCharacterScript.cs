using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarframeCharacterScript : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip shieldRegenerationAudioClip;
    AudioClip shieldDownAudioClip;

    Spawner spawner;
    int health;
    int shields;
    int MAX_HEALTH = 300;
    int MAX_SHIELDS = 100;
    bool isShieldRegenerating = true;
    float shieldRegenerationSpeed;

    Transform healthBar;
    Transform shieldBar;
    Text healthBarText;
    Text shieldBarText;

    Coroutine shieldRegenerationCoroutine;

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



    // Start is called before the first frame update
    void Start()
    {
        healthBar = GameObject.Find("UI/WarframeStatusContainer/HealthBar/HealthPointsBar").transform;
        shieldBar = GameObject.Find("UI/WarframeStatusContainer/HealthBar/ShieldPointsBar").transform;
        healthBarText = GameObject.Find("UI/WarframeStatusContainer/HealthBar/Canvas/TextHealth").GetComponent<Text>();
        shieldBarText = GameObject.Find("UI/WarframeStatusContainer/HealthBar/Canvas/TextShield").GetComponent<Text>();

        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        audioSource = GetComponent<AudioSource>();
        shieldRegenerationAudioClip = Resources.Load<AudioClip>("Sounds/Warframe/shieldRegeneration");
        shieldDownAudioClip = Resources.Load<AudioClip>("Sounds/Warframe/shieldDown");
        health = MAX_HEALTH;
        shields = MAX_SHIELDS;
        shieldRegenerationSpeed = 15 + (float)(0.05 * MAX_SHIELDS);
;
        updateStatusBar();
    }

    public void getDamageFromEnemy(int damage)
    {
        try
        { 
            StopCoroutine(shieldRegenerationCoroutine);
        }
        catch(System.NullReferenceException)
        {

        }
        shields -= damage;
        damage = Mathf.Clamp(shields * -1, 0, Mathf.Abs(shields));
        shields = Mathf.Clamp(shields, 0, MAX_SHIELDS);
        health = Mathf.Clamp(health - damage, 0, MAX_HEALTH);

        if(shields <= 0 && isShieldRegenerating)
        {
            isShieldRegenerating = false;
            audioSource.PlayOneShot(shieldDownAudioClip);
        }

        if (health <= 0)
        {
            spawner.stopTheGame();
            GameObject.Find("GameOverTable").GetComponent<Animation>().Play();
            gameObject.SetActive(false);
        }
        else
        {
            shieldRegenerationCoroutine = StartCoroutine(shieldRegeneration());
        }
        updateStatusBar();
    }

    private IEnumerator shieldRegeneration()
    {
        yield return new WaitForSeconds(3f);
        audioSource.PlayOneShot(shieldRegenerationAudioClip);
        float shieldRegen = 0;
        isShieldRegenerating = true;
        while (true)
        {
            shieldRegen += shieldRegenerationSpeed * Time.deltaTime;
            if (shieldRegen >= 1)
            {
                shields++;
                
                shieldRegen--;
                updateStatusBar();
            }
            if (shields >= MAX_SHIELDS)
            {
                StopCoroutine(shieldRegenerationCoroutine);
                break;
            }
            yield return null;
        }
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {

    }
}