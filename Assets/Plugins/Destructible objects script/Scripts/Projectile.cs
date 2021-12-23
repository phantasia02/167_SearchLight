using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    float timer;

	// Use this for initialization
	void Start () {

        timer = 5;

    }
	
	// Update is called once per frame
	void Update () {

        if (timer > 0)
        {

            timer -= Time.deltaTime;

        }
        else {

            Destroy(gameObject);

        }

	}
}
