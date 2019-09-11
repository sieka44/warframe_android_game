using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorpusLootLocker : MonoBehaviour
{
    Vector2 startVelocity;

    Rigidbody2D rigidBody2d;
    Vector3 rotationAxis;
    Transform corpusLockerTransform;
    Animation corpusLockerAnimation;
    Animation modAnimation;
    AudioSource audioSource;
    Text modName;
    string[] mods = new string[]{ "Fever Strike", "North Wind", "Shocking Touch", "Molten Impact" };

bool isDestroyed;

    public void spawn(Vector3 spawnPosition, Vector2 spawnVelocity)
    {
        transform.position = spawnPosition;
        startVelocity = spawnVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Saber")
        {
            rigidBody2d.velocity = new Vector2(0, 0);
            rigidBody2d.bodyType = RigidbodyType2D.Static;
            rotationAxis = Vector3.zero;
            isDestroyed = true;
            corpusLockerAnimation.Play();
            modAnimation.Play();
            audioSource.Play();
            Destroy(gameObject, 3);
            modName.text = mods[(int)Random.Range(0, 4)];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isDestroyed = false;
        rigidBody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        rotationAxis = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
        corpusLockerTransform = transform.Find("corpusLocker");
        corpusLockerTransform.Rotate(rotationAxis);

        modName = transform.Find("mod").Find("Canvas").Find("Text").GetComponent<Text>();
        corpusLockerAnimation = transform.Find("corpusLocker").GetComponent<Animation>();
        modAnimation = transform.Find("mod").GetComponent<Animation>();

        rigidBody2d.velocity = startVelocity;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y < -5.5) Destroy(gameObject);
        corpusLockerTransform.Rotate(rotationAxis, 150 * Time.deltaTime);
    }
}
