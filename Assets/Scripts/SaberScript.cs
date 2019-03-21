using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaberScript : MonoBehaviour
{

    private Vector3 defaultLocation;
    private int damage = 25;
    private readonly float RAY_DISTANCE = 40;
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
        if(Input.GetMouseButton(0) || ( Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved ))
        {
            /*
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 0;
            transform.position = newPosition;
            */
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 pos = ray.GetPoint(RAY_DISTANCE);
            transform.position = pos;
        }
        else
        {
            transform.position = defaultLocation;
        }
    }
}
    