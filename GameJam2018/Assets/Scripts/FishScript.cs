using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour {

    public FishManager FM;
    public bool DEBUG = true;
    public int ID;
    public Vector3 Speed;
    public Vector3 MaxSpeed;
    private Vector3 _steering;
    private float turnSpeed;

    public LineRenderer LR_CoM;
    public LineRenderer LR_CoR;
    public LineRenderer LR_AvoidFish;
    public LineRenderer LR_Steering;

    public Vector3 CoM;
    int CoMCount;

    public Vector3 CoR;
    int CoRCount;

    public float AvoidFishVisionRatio = 0.2f;
    public Vector3 AvoidFish;
    public float ClosestFishRange;

    public float AvoidTankVisionRatio = 0.2f;
    public float tankRange;
    public Vector3 Tank;


	// Use this for initialization
	void Start () {

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
        turnSpeed = FM.TurnRate;
        // Avoid 2 (Tank)
        Tank = Vector3.zero;


        // Populates CoM, CoR and AvoidFish
        ScanFish();

        // Populates AvoidShark
        ScanSharks();

        // Determines TankEdge Avoidance
        ScanTank();

        //double start = Time.realtimeSinceStartup;

        // TO DO .... AVOID1 - Fish impact ---- DONE
        // TO DO .... AVOID2 - Nearest Shark
        // TO DO .... AVOID3 - Tank Edge ---- DONE
        // TO DO .... AVOID4 - Tank Bottom / Top ---- DONE

        // TO DO .... Apply Weighting to each
        // FIRST Pass weighting

        _steering = (CoM * FM.CoMWeight) + (CoR * FM.CoRWeight) - (AvoidFish * FM.AvoidFishWeight);
        _steering -= (Tank * FM.TankAvoidWeight);

        //Debug.Log("Time to Search " + (Time.realtimeSinceStartup - start));


        //Quaternion target = Quaternion.Euler(CoR.x, CoR.y, CoR.z);
        // Dampen towards the target rotation
        //transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * FM.TurnRate);
        //transform.Rotate(transform.up, FM.TurnRate * Time.deltaTime);

        //transform.rotation = Quaternion.LookRotation(_steering, Vector3.up);

        Quaternion Steer = Quaternion.Euler(_steering) * transform.rotation;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Steer, Time.deltaTime * 25);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_steering, transform.up), Time.deltaTime);

        transform.Translate(transform.forward * FM.Speed * Time.deltaTime, Space.World);

        if (DEBUG) UpdateLineRenderers();

    }

    private void ScanFish()
    {
        foreach (FishScript FS in FM.Fish)
        {
            if (FS.ID != ID) // don't compare on self
            {
                float range = Vector3.Distance(transform.position, FS.transform.position);
                if (range < FM.Vision)
                {
                    if (range <= FM.Vision * FM.AvoidFishWeight)
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
        tankRange = FM.tankRadius * 2;
        //Tank = Vector3.zero;

        // bottom
        Vector3 BottomTankPos = new Vector3(0, 0, 0);
        Vector3 TopTankPos = new Vector3(0, FM.height - FM.waterOffset, 0);
        float rangeBottom = Vector3.Distance(new Vector3(0, transform.position.y, 0), BottomTankPos);
        if (rangeBottom < FM.Vision)
        {
            if (rangeBottom <= FM.Vision * AvoidTankVisionRatio)
            {
                if (rangeBottom <= FM.TankAvoidWeight)
                    Tank = Vector3.down;
            }
        }
        // top
        float rangeTop = Vector3.Distance(new Vector3(0, transform.position.y, 0), TopTankPos);
        if (rangeTop < FM.Vision)
        {
            if (rangeTop <= FM.Vision * AvoidTankVisionRatio)
            {
                if (rangeTop <= FM.TankAvoidWeight)
                    Tank = Vector3.up;
            }
        }
        // edge
        float rangeEdge = FM.tankRadius - Mathf.Sqrt(transform.position.x * transform.position.x + transform.position.z * transform.position.z);
        if (rangeEdge < FM.Vision && rangeEdge < tankRange)
            Tank = new Vector3(transform.position.x, 0, transform.position.z);
    }

    private void UpdateLineRenderers()
    {
        // CoM Orange
        LR_CoM.SetPosition(0, transform.position);
        LR_CoM.SetPosition(1, transform.position + (CoM * FM.CoMWeight));
        // CoR Green
        LR_CoR.SetPosition(0, transform.position);
        LR_CoR.SetPosition(1, transform.position + (CoR * FM.CoRWeight));
        // FishAvoid Red
        LR_AvoidFish.SetPosition(0, transform.position);
        LR_AvoidFish.SetPosition(1, transform.position - (AvoidFish * FM.AvoidFishWeight));
        // Final Steering Black
        LR_Steering.SetPosition(0, transform.position);
        LR_Steering.SetPosition(1, transform.position + _steering);
    }



}
