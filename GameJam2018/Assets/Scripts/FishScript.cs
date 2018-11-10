using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour {

    FishManager FM;
    public bool DEBUG = true;
    public int ID;
    public Vector3 Speed;
    public Vector3 MaxSpeed;
    private Vector3 _steering;

    public LineRenderer LR_CoM;
    public LineRenderer LR_CoR;
    public LineRenderer LR_AvoidFish;
    public LineRenderer LR_Steering;

    public FishScript[] Others;

    public Vector3 CoM;
    int CoMCount;
    public float CoMWeight = 10;

    public Vector3 CoR;
    int CoRCount;
    public float CoRWeight = 1;

    public float AvoidFishVisionRatio = 0.2f;
    public Vector3 AvoidFish;
    public float ClosestFishRange;
    public float AvoidFishWeight = 2;



	// Use this for initialization
	void Start () {
        // find Manager
        FM = FindObjectOfType<FishManager>();
        // spin around
        transform.rotation = Random.rotation;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Reset counters and references
        // CentreOfMass
        CoM = Vector3.zero;
        CoMCount = 0;
        // Centre of Rotation
        CoR = Vector3.zero;
        CoRCount = 0;
        // Avoid 1 (Fish)
        AvoidFish = Vector3.zero;
        ClosestFishRange = 10000000; // good enough for now, tie to tank size eventually

        // Populates CoM, CoR and AvoidFish
        ScanFish();

        // Populates AvoidShark
        ScanSharks();

        // Determines TankEdge Avoidance
        ScanTank();

        //double start = Time.realtimeSinceStartup;

        // TO DO .... AVOID1 - Fish impact ---- DONE
        // TO DO .... AVOID2 - Nearest Shark
        // TO DO .... AVOID3 - Tank Edge
        // TO DO .... AVOID4 - Tank Bottom / Top

        // TO DO .... Apply Weighting to each
        // FIRST Pass weighting

        _steering = (CoM * CoMWeight) + (CoR * CoRWeight) - (AvoidFish * AvoidFishWeight);


        //Debug.Log("Time to Search " + (Time.realtimeSinceStartup - start));


        //Quaternion target = Quaternion.Euler(CoR.x, CoR.y, CoR.z);
        // Dampen towards the target rotation
        //transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * FM.TurnRate);
        //transform.Rotate(transform.up, FM.TurnRate * Time.deltaTime);

        //transform.rotation = Quaternion.LookRotation(_steering, Vector3.up);

        //Quaternion Steer = Quaternion.Euler(_steering) * transform.rotation;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Steer, Time.deltaTime * 25);

        //Quaternion.Euler(_steering);

        Vector3 finalangle = new Vector3(Mathf.LerpAngle(transform.eulerAngles.x, _steering.x, Time.deltaTime * 25),
                                        Mathf.LerpAngle(transform.eulerAngles.y, _steering.y, Time.deltaTime * 25),
                                        Mathf.LerpAngle(transform.eulerAngles.z, _steering.z, Time.deltaTime * 25));

        transform.eulerAngles = finalangle;

        transform.Translate(transform.forward * FM.Speed * Time.deltaTime, Space.World);

        if (DEBUG) UpdateLineRenderers();

    }

    private void ScanFish()
    {
        foreach (FishScript FS in Others)
        {
            if (FS.ID != ID) // don't compare on self
            {
                float range = Vector3.Distance(transform.position, FS.transform.position);
                if (range < FM.Vision)
                {
                    if (range <= FM.Vision * AvoidFishVisionRatio)
                    {
                        if (range <= ClosestFishRange)
                        {
                            // AvoidFish
                            ClosestFishRange = range;
                            AvoidFish = FS.transform.position - transform.position;
                        }
                    }
                    else
                    {
                        // CentreOfMass
                        CoM += (FS.transform.position - transform.position);
                        CoMCount++;
                        // Centre of Rotation
                        CoR += (FS.transform.forward - transform.forward);
                        CoRCount++;
                    }
                }
            }
            if (CoMCount > 0)
            {
                CoM.Normalize();
            }
            if (CoRCount > 0)
            {
                CoR.Normalize();
            }
            AvoidFish.Normalize();
        }
    }

    private void ScanSharks()
    {

    }

    private void ScanTank()
    {

    }

    private void UpdateLineRenderers()
    {
        // CoM Orange
        LR_CoM.SetPosition(0, transform.position);
        LR_CoM.SetPosition(1, transform.position + (CoM * CoMWeight));
        // CoR Green
        LR_CoR.SetPosition(0, transform.position);
        LR_CoR.SetPosition(1, transform.position + (CoR * CoRWeight));
        // FishAvoid Red
        LR_AvoidFish.SetPosition(0, transform.position);
        LR_AvoidFish.SetPosition(1, transform.position - (AvoidFish * AvoidFishWeight));
        // Final Steering Black
        LR_Steering.SetPosition(0, transform.position);
        LR_Steering.SetPosition(1, transform.position + _steering);
    }



}
