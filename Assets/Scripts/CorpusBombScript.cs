using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorpusBombScript : MonoBehaviour
{

    Rigidbody2D rigidBody2d;
    Text label;
    int health;
    Vector2 startVelocity;
    Transform corpusBombTransform;
    Vector3 rotationAxis;

    public float explosionPower = 1;

    public void setStartVelocity(Vector2 startVelocity)
    {
        this.startVelocity = startVelocity;
    }

    // Use this for initialization
    void Start()
    {
        health = 50;
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
            health -= collision.gameObject.GetComponent<SaberScript>().GetDamage();
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
                float distance = Mathf.Sqrt(Mathf.Pow(enemyPosition.x - transform.position.x, 2) + Mathf.Pow(enemyPosition.y - transform.position.y, 2) + Mathf.Pow(enemyPosition.z - transform.position.z, 2));
                
                Vector3 velocity = enemy.gameObject.GetComponent<CorpusCrewmanEnemyScript>().getVelocity();
                enemy.gameObject.GetComponent<CorpusCrewmanEnemyScript>().setVelocity(velocity + explosionDirection.normalized * (explosionPower/distance));
            }
            Destroy(gameObject);
        }
            
        if (transform.position.y < -5.5) Destroy(gameObject);
        corpusBombTransform.Rotate(rotationAxis, 150 * Time.deltaTime);
        label.text = "Health: " + health + "%";
    }
}
