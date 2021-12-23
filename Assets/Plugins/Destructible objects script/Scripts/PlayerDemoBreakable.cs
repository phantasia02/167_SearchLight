using UnityEngine;
using System.Collections;

public class PlayerDemoBreakable : MonoBehaviour {

    public Texture2D crosshair;
    public Transform projectile;
    public Transform managerObject;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {

            //Debug.Log("shoot");

            Transform newProj;
            newProj = Instantiate(projectile, transform.position + transform.forward * 1, transform.rotation) as Transform;
            newProj.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 10000);

        }

        if (Input.GetKeyDown(KeyCode.R))
        {

            if (managerObject != null)
            {

                managerObject.GetComponent<BreakableObjectManager>().ResetObjects();

            }

        }

    }

    void OnGUI() {

        GUI.DrawTexture(new Rect(Screen.width * 0.5f - 25, Screen.height * 0.5f - 25, 50, 50), crosshair);

    }

}