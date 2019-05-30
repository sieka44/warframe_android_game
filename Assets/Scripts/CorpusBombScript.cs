using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorpusBombScript : MonoBehaviour
{
    public GameObject explosionSpritePrefab;

    Rigidbody2D rigidBody2d;
    Text label;
    int health;
    int maxHealth = 100;
    int BASE_DAMAGE = 225;
    int damage;
    Vector2 startVelocity;
    Transform corpusBombTransform;
    Vector3 rotationAxis;

    Transform healthBar;

    public float explosionPower = 1;

    public void setStartVelocity(Vector2 startVelocity)
    {
        this.startVelocity = startVelocity;
    }

    public void spawn(Vector3 spawnPosition, Vector2 spawnVelocity, int level)
    {
        transform.position = spawnPosition;
        startVelocity = spawnVelocity;
        health = maxHealth;
        damage = (int)(BASE_DAMAGE * (1 + System.Math.Pow(level - 1, 2) * 0.0075));

        updateBar();
    }

    private void updateBar()
    {
        if (healthBar == null) healthBar = transform.Find("HealthBar").Find("HealthPointsBar");

        healthBar.localScale = new Vector3((float)System.Math.Round((float)health / maxHealth, 2), 1f, 1f);
    }

    // Use this for initialization
    void Start()
    {
        rigidBody2d = GetComponent<Rigidbody2D>();

        rotationAxis = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
        corpusBombTransform = transform.GetChild(1);
        corpusBombTransform.Rotate(rotationAxis);

        label = transform.Find("Canvas").Find("Text").GetComponent<Text>();
        rigidBody2d.velocity = startVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Saber")
        {
            health -= 34;
            updateBar();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Vector3 enemyPosition = enemy.transform.position;
                Vector3 explosionDirection = enemyPosition - transform.position;
                float distance = Vector3.Distance(enemyPosition, transform.position);

                Vector3 velocity = enemy.gameObject.GetComponent<CorpusCrewmanEnemyScript>().getVelocity();
                enemy.gameObject.GetComponent<CorpusCrewmanEnemyScript>().setVelocity(velocity + explosionDirection.normalized * (5/Mathf.Pow(distance, 1/8)));
                enemy.gameObject.GetComponent<CorpusCrewmanEnemyScript>().getDamageFromBomb((int)(damage * 1 / Mathf.Pow(distance, 1 / 2)));
            }

            var newExplosion = Instantiate(explosionSpritePrefab);
            newExplosion.transform.position = transform.position - new Vector3(0f, 0.8f, 0f);

            Destroy(gameObject);
        }
            
        if (transform.position.y < -5.5) Destroy(gameObject);
        corpusBombTransform.Rotate(rotationAxis, 150 * Time.deltaTime);
        label.text = "Health: " + health + "%";
    }
}
