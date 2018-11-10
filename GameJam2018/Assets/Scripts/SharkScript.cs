using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkScript : MonoBehaviour {

    private Vector3 _steering;

    public Vector3 prey;
    public float preyRange;
    public Vector3 Tank;


    public int ID;
    public SharkManager SM;

    public SharkScript[] Others;
    public FishScript[] Dinner;

	// Use this for initialization
	void Start () {
        SM = FindObjectOfType<SharkManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
