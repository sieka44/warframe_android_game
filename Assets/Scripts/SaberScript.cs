using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaberScript : MonoBehaviour
{

    private Vector3 defaultLocation;
    private int damage = 25; 
    // Use this for initialization
    void Start ()
    {
        defaultLocation.Set(-10, 0, 0);
	}

    public int getDamage()
    {
        return damage;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 0;
            transform.position = newPosition;
        }
        else
        {
            transform.position = defaultLocation;
        }
    }
}
    