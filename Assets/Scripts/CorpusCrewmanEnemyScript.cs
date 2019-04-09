using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorpusCrewmanEnemyScript : MonoBehaviour {

    Rigidbody2D rigidBody2d;
    Text label;
    int health;
    Vector2 startVelocity;
	// Use this for initialization

    public void setStartVelocity(Vector2 startVelocity)
    {
        this.startVelocity= startVelocity;
    }

	void Start ()
    {
        health = 100;
        rigidBody2d = GetComponent<Rigidbody2D>();

        Vector3 rotationAxis = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
        Transform corpusCrewmanTransform = transform.GetChild(1);
        corpusCrewmanTransform.Rotate(rotationAxis);

        label = transform.Find("Canvas").Find("Text").GetComponent<Text>();
        rigidBody2d.velocity = startVelocity;
        rigidBody2d.angularVelocity = 3;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Saber")
        {
            health -= collision.gameObject.GetComponent<SaberScript>().GetDamage();
            Debug.Log(health);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (health <= 0) Destroy(gameObject);
        if (transform.position.y < -5.5) Destroy(gameObject);
        label.text = "Health: " + health + "%";
	}
}
