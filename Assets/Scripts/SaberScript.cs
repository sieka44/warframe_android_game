using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaberScript : MonoBehaviour
{

    private Vector3 defaultLocation;
    private readonly int damage = 25;
    private readonly float speed = 100.0f;
    private ParticleSystem particle = null;
    SaberDamage saberDamage;

    // Use this for initialization
    void Start ()
    {
        saberDamage = new SaberDamage();
        defaultLocation.Set(0, 0, 0);
        particle = this.gameObject.GetComponentInChildren<ParticleSystem>();
	}

    public SaberDamage GetDamage()
    {
        return saberDamage;
    }
	
	// Update is called once per frame
	void Update()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0;
        Vector3 position = Vector3.Lerp(transform.position, newPosition, 1.0f - Mathf.Exp(-speed * Time.deltaTime));

        transform.position = position;        
    }
}
    