using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour {

    FishManager FM;
    public Vector3 Velocity;
    public Vector3 Position;
    public Vector3 Speed;
    public Vector3 MaxSpeed;
    public Vector3 CoM;
    public int ID;
    public FishScript[] Others;



	// Use this for initialization
	void Start () {
        // find Manager
        FM = FindObjectOfType<FishManager>();
        Position = GetComponentInParent<Transform>().position;

	}
	
	// Update is called once per frame
	void Update ()
    {
        CoM = Vector3.zero;
        int CoMCount = 0;
        Vector3 CoR = Vector3.zero;
        int CoRCount = 0;
        //double start = Time.realtimeSinceStartup;
        foreach (FishScript FS in Others)
        {

            if (FS.ID != ID) // don't compare on self
            {
                if (Vector3.Distance(transform.position,FS.transform.position) < FM.Vision)
                {
                    CoM += (FS.transform.position - transform.position);
                    CoMCount++;
                    CoR += (FS.Velocity - Velocity);
                    CoRCount++;
                }
            }
            if (ID == 4) Debug.Log(CoMCount + " found");
            if (CoMCount > 0) CoM /= (float)CoMCount;
            if (CoRCount > 0) CoR /= (float)CoRCount;
        }
        //Debug.Log("Time to Search " + (Time.realtimeSinceStartup - start));


        //Quaternion target = Quaternion.Euler(CoR.x, CoR.y, CoR.z);
        // Dampen towards the target rotation
        //transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * FM.TurnRate);
        //transform.Rotate(transform.up, FM.TurnRate * Time.deltaTime);

        transform.rotation = Quaternion.LookRotation(CoM, Vector3.up);
        transform.Translate(transform.forward * FM.Speed * Time.deltaTime,Space.World);
     

	}
}
