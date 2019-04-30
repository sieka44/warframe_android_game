using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorpusCrewmanEnemyScript : MonoBehaviour {

    Rigidbody2D rigidBody2d;
    Text label;
    int health;
    Vector2 startVelocity;
    Animation corpusCrewmanAnimation;
    Transform corpusCrewmanTransform;
    Vector3 rotationAxis;

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

    // Use this for initialization
    void Start ()
    {
        health = 100;
        rigidBody2d = GetComponent<Rigidbody2D>();

        rotationAxis = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
        corpusCrewmanTransform = transform.GetChild(1);
        corpusCrewmanTransform.Rotate(rotationAxis);

        label = transform.Find("Canvas").Find("Text").GetComponent<Text>();
        rigidBody2d.velocity = startVelocity;

        corpusCrewmanAnimation = transform.Find("corpusCrewmanContainer").Find("corpusCrewman").GetComponent<Animation>();
        corpusCrewmanAnimation.Play("CorpusCrewmanPose" + Random.Range(1, 3));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Saber")
        {
            health -= collision.gameObject.GetComponent<SaberScript>().GetDamage();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (health <= 0) Destroy(gameObject);
        if (transform.position.y < -5.5) Destroy(gameObject);
        corpusCrewmanTransform.Rotate(rotationAxis, 100 * Time.deltaTime);
        label.text = "Health: " + health + "%";
	}
}
