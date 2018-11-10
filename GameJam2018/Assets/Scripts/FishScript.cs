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
    public LineRenderer LR_SHARK;

    public Vector3 CoM;
    int CoMCount;

    public Vector3 CoR;
    int CoRCount;

    public float AvoidFishVisionRatio = 0.2f;
    public Vector3 AvoidFish;
    public float ClosestFishRange;
    public Vector3 Shark;
    public float ClosestSharkRange;

    public float tankRange;
    public Vector3 Tank;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update ()
    {
        // Reset counters and references
        CoM = Vector3.zero;
        CoMCount = 0;
        CoR = Vector3.zero;
        CoRCount = 0;
        AvoidFish = Vector3.zero;
        ClosestFishRange = FM.tankRadius * 2;
        Shark = Vector3.zero;
        ClosestSharkRange = FM.tankRadius * 2;
        Tank = Vector3.zero;

        ScanFish();
        ScanSharks();
        ScanTank();
        ApplyWeighting();
        ApplySteering();

        if (Input.GetKeyUp("p")) ToggleDebug();
        if (DEBUG) UpdateLineRenderers();

    }

    private void ToggleDebug()
    {
        if (DEBUG)
        {
            LR_CoM.enabled = false;
            LR_CoR.enabled = false;
            LR_AvoidFish.enabled = false;
            LR_Steering.enabled = false;
            LR_SHARK.enabled = false;
            DEBUG = false;
        }
        else
        {
            LR_CoM.enabled = true;
            LR_CoR.enabled = true;
            LR_AvoidFish.enabled = true;
            LR_Steering.enabled = true;
            LR_SHARK.enabled = true;
            DEBUG = true;
        }
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
                    if (range <= FM.Vision * FM.AvoidFishVisionRatio)
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
        foreach (SharkScript ss in FM.Sharks)
        {
            float range = Vector3.Distance(transform.position, ss.transform.position);
            if (range <= FM.Vision)
            {
                if (range < ClosestSharkRange)
                {
                    ClosestSharkRange = range;
                    Shark = ss.transform.position - transform.position;
                }
            }
        }
        Shark.Normalize();
    }

    private void ScanTank()
    {
        tankRange = FM.tankRadius * 2;
        Tank = Vector3.zero;

        // bottom
        Vector3 BottomTankPos = new Vector3(0, 0, 0);
        Vector3 TopTankPos = new Vector3(0, FM.height - FM.waterOffset, 0);
        float rangeBottom = Vector3.Distance(new Vector3(0, transform.position.y, 0), BottomTankPos);
        if (rangeBottom < FM.Vision)
        {
            if (rangeBottom <= FM.Vision * FM.AvoidTankVisionRatio)
            {
                if (rangeBottom <= FM.TankAvoidWeight)
                    Tank = Vector3.down;
            }
        }
        // top
        float rangeTop = Vector3.Distance(new Vector3(0, transform.position.y, 0), TopTankPos);
        if (rangeTop < FM.Vision)
        {
            if (rangeTop <= FM.Vision * FM.AvoidTankVisionRatio)
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

    private void ApplyWeighting()
    {
        _steering = (CoM * FM.CoMWeight) + (CoR * FM.CoRWeight) - (AvoidFish * FM.AvoidFishWeight) -  (Tank * FM.TankAvoidWeight) - (Shark * FM. SharkAvoidWeight);
        if (_steering == Vector3.zero) _steering = FM.GetWand(transform.forward);
    }

    private void ApplySteering()
    {
        Quaternion Steer = Quaternion.Euler(_steering) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_steering, transform.up), Time.deltaTime);
        transform.Translate(transform.forward * FM.Speed * Time.deltaTime, Space.World);
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

        LR_SHARK.SetPosition(0, transform.position);
        LR_SHARK.SetPosition(1, transform.position + Shark * FM.SharkAvoidWeight);
    }



}
