using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorpusCrewmanEnemyScript : MonoBehaviour {

    Rigidbody2D rigidBody2d;
    int health;
    int maxHealth;
    int shields;
    int maxShields;
    Vector2 startVelocity;
    Animation corpusCrewmanAnimation;
    Transform corpusCrewmanTransform;
    Vector3 rotationAxis;

    Transform healthBar;
    Transform shieldsBar;

    const float healthBarSize = 1.5f;
    const int BASE_HEALTH = 60;
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
        maxHealth = BASE_HEALTH * enemyLevel;
        maxShields = BASE_SHIELDS * enemyLevel;
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
        rigidBody2d = GetComponent<Rigidbody2D>();

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
        transform.Find("corpusCrewmanContainer").Find("corpusCrewman").Find("crewman_body").GetComponent<Renderer>().material = bodyMaterials[(int)Random.Range(0, bodyMaterials.Count - 1)];
        bodyMaterials = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Saber")
        {
            int saberDamage = collision.gameObject.GetComponent<SaberScript>().GetDamage();
            if (shields >0)
            {
                shields -= saberDamage;
            }
            else
            {
                shields = 0;
                health -= saberDamage;
            }
            
            updateBar();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (health <= 0) Destroy(gameObject);
        if (transform.position.y < -5.5) Destroy(gameObject);
        corpusCrewmanTransform.Rotate(rotationAxis, 100 * Time.deltaTime);
	}
}
