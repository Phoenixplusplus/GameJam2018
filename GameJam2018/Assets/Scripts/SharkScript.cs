using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkScript : MonoBehaviour {

    public int ID;
    public SharkManager SM;

    public SharkScript[] Others;

	// Use this for initialization
	void Start () {
        SM = FindObjectOfType<SharkManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
