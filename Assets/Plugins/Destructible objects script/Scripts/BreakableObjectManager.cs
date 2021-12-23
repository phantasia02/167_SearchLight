using UnityEngine;
using System.Collections;

public class BreakableObjectManager : MonoBehaviour {

    public bool REPAIR;
    public GameObject[] BreakableObjects;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (REPAIR == true) {

            ResetObjects();

        }
	
	}

    public void ResetObjects() {

        for (int i = 0; i < BreakableObjects.Length; i++) {

            if (BreakableObjects[i])
            {
                /*if (BreakableObjects[i].GetComponent<BreakableObject>())
                {
                    BreakableObjects[i].GetComponent<BreakableObject>().ResetObject();
                }*/
                if (BreakableObjects[i].GetComponent<BreakableV2>())
                {
                    BreakableObjects[i].GetComponent<BreakableV2>().Reset();
                }

            }

        }

        REPAIR = false;

    }

}
