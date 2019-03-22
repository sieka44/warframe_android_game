using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorpusCrewmanEnemyScript : MonoBehaviour {

    Rigidbody2D rigidBody2d;
    Text label;
    int health;
	// Use this for initialization
	void Start ()
    {
        health = 100;
        rigidBody2d = GetComponent<Rigidbody2D>();
        label = transform.Find("Canvas").Find("Text").GetComponent<Text>();
        rigidBody2d.velocity = new Vector2(2, 13);
        rigidBody2d.angularVelocity = 3;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        health -= collision.gameObject.GetComponent<SaberScript>().getDamage();
        Debug.Log(health);
    }

    // Update is called once per frame
    void Update ()
    {
        if (health <= 0) Destroy(gameObject);
        label.text = "Health: " + health + "%";
        
	}
}
