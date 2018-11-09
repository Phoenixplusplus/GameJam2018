using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour {

    FishManager FM;
    public Vector3 Velocity;
    public Vector3 Speed;
    public Vector3 MaxSpeed;



	// Use this for initialization
	void Start () {
        // find Manager
        FM = FindObjectOfType<FishManager>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
