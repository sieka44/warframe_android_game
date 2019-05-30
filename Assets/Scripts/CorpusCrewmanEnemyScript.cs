using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorpusCrewmanEnemyScript : MonoBehaviour
{
    WarframeCharacterScript warframeCharacter;

    Rigidbody2D rigidBody2d;

    Renderer helmetRenderer;
    Renderer bodyRenderer;
    
    int health;
    int maxHealth;
    int shields;
    int maxShields;
    bool isAlive;
    Vector2 startVelocity;
    Animation corpusCrewmanAnimation;
    Transform corpusCrewmanTransform;
    Vector3 rotationAxis;

    Transform healthBar;
    Transform shieldsBar;
    AudioSource corpusCrewmanVoice;

    const float healthBarSize = 1.5f;
    const int BASE_HEALTH = 75;
    const int BASE_SHIELDS = 150;

    public Vector2 getVelocity()
    {
        return rigidBody2d.velocity;
    }

    public void setVelocity(Vector2 velocity)
    {
        try
        {
            rigidBody2d.velocity = velocity;
        }
        catch(System.NullReferenceException)
        {
            startVelocity = velocity;
        }
    }

    public void spawn(int enemyLevel, Vector3 spawnPosition, Vector2 spawnVelocity)
    {
        transform.position = spawnPosition;
        startVelocity = spawnVelocity;
        maxHealth = (int)(BASE_HEALTH * (1 + System.Math.Pow(enemyLevel - 1, 2) * 0.015));
        maxShields = (int)(BASE_SHIELDS * (1 + System.Math.Pow(enemyLevel - 1, 2)* 0.0075));
        health = maxHealth;
        shields = maxShields;
        transform.Find("Canvas").Find("Text").GetComponent<Text>().text = enemyLevel.ToString();

        updateBar();
    }

    private void updateBar()
    {
        if (healthBar == null) healthBar = transform.Find("HealthBar").Find("HealthPointsBar");
        if (shieldsBar == null) shieldsBar = transform.Find("HealthBar").Find("ShieldPointsBar");

        int allBarPoints = maxHealth + maxShields;
        float healthLevelInPercents = (float)System.Math.Round((float) health / allBarPoints, 2);
        float shieldsLevelInPercents = (float)System.Math.Round((float) shields / allBarPoints, 2);

        healthBar.localScale = new Vector3(healthLevelInPercents, 1f, 1f);
        shieldsBar.localPosition = new Vector3(-0.74f + (float)System.Math.Round(healthLevelInPercents * healthBarSize, 2), 0f, 0f);
        shieldsBar.localScale = new Vector3(shieldsLevelInPercents, 1f, 1f);

    }

    // Use this for initialization
    void Start ()
    {
        isAlive = true;

        warframeCharacter = GameObject.Find("WarframeCharacter").GetComponent<WarframeCharacterScript>();
        rigidBody2d = GetComponent<Rigidbody2D>();
        corpusCrewmanVoice = GetComponent<AudioSource>();

        rotationAxis = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
        corpusCrewmanTransform = transform.GetChild(1);
        corpusCrewmanTransform.Rotate(rotationAxis);

        rigidBody2d.velocity = startVelocity;

        corpusCrewmanAnimation = transform.Find("corpusCrewmanContainer").Find("corpusCrewman").GetComponent<Animation>();
        corpusCrewmanAnimation.Play("CorpusCrewmanPose" + Random.Range(1, 3));

        List<Material> bodyMaterials = new List<Material>();
        for (int i = 1; i < 11; i++)
        {
            bodyMaterials.Add(Resources.Load<Material>("Materials/CorpusCrewman/body" + i));
        }

        helmetRenderer = transform.Find("corpusCrewmanContainer").Find("corpusCrewman").Find("crewman_helmet").GetComponent<Renderer>();
        bodyRenderer = transform.Find("corpusCrewmanContainer").Find("corpusCrewman").Find("crewman_body").GetComponent<Renderer>();
        bodyRenderer.material = bodyMaterials[(int)Random.Range(0, bodyMaterials.Count - 1)];
        bodyMaterials = null;

        if(Random.Range(1, 100) < 15)
        {
            List<AudioClip> corpusSpeech = new List<AudioClip>();
            for (int i = 1; i < 8; i++) corpusSpeech.Add(Resources.Load<AudioClip>("Sounds/CorpusCrewman/Quiet/corpus_quiet_0" + i));
            corpusCrewmanVoice.clip = corpusSpeech[Random.Range(0, 7)];
            corpusCrewmanVoice.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Saber")
        {
            if(isAlive)
            {
                SaberDamage saberDamage = collision.gameObject.GetComponent<SaberScript>().GetDamage();

                if (shields > 0)
                {
                    int damage = saberDamage.getSlash() + (int)(saberDamage.getCold() * 1.5) + saberDamage.getElectricity() + saberDamage.getHeat() + saberDamage.getToxin();
                    shields = Mathf.Clamp(shields - damage, 0, shields);
                }
                else
                {
                    int damage = (int)(saberDamage.getSlash() * 1.25) + saberDamage.getCold() + saberDamage.getElectricity() + saberDamage.getHeat() + (int)(saberDamage.getToxin() * 1.5);
                    health -= damage;
                }

                if (Random.Range(1, 100) < 15)
                {
                    List<AudioClip> corpusSpeech = new List<AudioClip>();
                    for (int i = 1; i < 5; i++) corpusSpeech.Add(Resources.Load<AudioClip>("Sounds/CorpusCrewman/Pain/corpus_pain_0" + i));
                    corpusCrewmanVoice.clip = corpusSpeech[Random.Range(0, 4)];
                    corpusCrewmanVoice.Play();
                }

                checkHealthStatus();

                updateBar();
            }
        }
    }

    public void getDamageFromBomb (int damage)
    {
        int shieldsTemp = shields;
        int healthTemp = health;
        shields -= damage;
        damage = Mathf.Clamp(shields * -1, 0, Mathf.Abs(shields));
        shields = Mathf.Clamp(shields, 0, shieldsTemp);
        health = Mathf.Clamp(health - damage, 0, healthTemp);

        checkHealthStatus();

        updateBar();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!isAlive && bodyRenderer.material.color.a >= 0)
        {
            Color newColor = new Color(bodyRenderer.material.color.r, bodyRenderer.material.color.g, bodyRenderer.material.color.b, bodyRenderer.material.color.a - (2f * Time.deltaTime));
            helmetRenderer.material.color = newColor;
            bodyRenderer.material.color = newColor;
        }

        if (transform.position.y < -5.5 && !corpusCrewmanVoice.isPlaying)
        {
            if(isAlive) warframeCharacter.getDamageFromEnemy(100);
            Destroy(gameObject);
        }
            
        corpusCrewmanTransform.Rotate(rotationAxis, 100 * Time.deltaTime);
	}

    private void checkHealthStatus()
    {
        if (health <= 0)
        {
            List<AudioClip> corpusSpeech = new List<AudioClip>();
            for (int i = 1; i < 7; i++) corpusSpeech.Add(Resources.Load<AudioClip>("Sounds/CorpusCrewman/Death/corpus_death_0" + i));
            corpusCrewmanVoice.clip = corpusSpeech[Random.Range(0, 5)];
            corpusCrewmanVoice.volume = 0.33f;
            corpusCrewmanVoice.Play();
            isAlive = false;
            health = 0;
            //Destroy(gameObject);
            transform.Find("Canvas").gameObject.SetActive(false);
            transform.Find("HealthBar").gameObject.SetActive(false);
        }
    }
}
