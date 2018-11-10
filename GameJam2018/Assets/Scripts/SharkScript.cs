using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkScript : MonoBehaviour {

    private Vector3 _steering;

    public bool DEBUG = true;
    public Vector3 prey;
    public float preyRange;
    public Vector3 Tank;
    public float tankRange;
    public Vector3 shark;
    public float sharkRange;

    public LineRenderer LR_Avoid;
    public LineRenderer LR_Steer;
    public LineRenderer LR_Dinner;
    public LineRenderer LR_Tank;


    public int ID;
    public SharkManager SM;

	// Use this for initialization
	void Start () {
        SM = FindObjectOfType<SharkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //reset Search parameters
        preyRange = SM.FM.tankRadius * 2;
        sharkRange = SM.FM.tankRadius * 2;
        tankRange = SM.FM.tankRadius * 2;
        shark = Vector3.zero;
        prey = Vector3.zero;
        Tank = Vector3.zero;

        SearchForLunch();
        SearchForSharks();
        SearchForTank();

        ApplyWeighting();
        ApplySteering();

        UpdateLineRenderers();

    }

    private void UpdateLineRenderers ()
    {
        LR_Avoid.SetPosition(0,transform.position);
        LR_Avoid.SetPosition(1, transform.position + shark * SM.SharkAvoidWeight);

        LR_Steer.SetPosition(0, transform.position);
        LR_Steer.SetPosition(1, transform.position + _steering);

        LR_Dinner.SetPosition(0, transform.position);
        LR_Dinner.SetPosition(1, transform.position + prey * SM.PreyWeight);

        LR_Tank.SetPosition(0, transform.position);
        LR_Tank.SetPosition(1, transform.position + Tank * SM.TankAvoidWeight);
    }

    private void SearchForLunch()
    {
        foreach (FishScript fs in SM.Fish)
        {
            float range = Vector3.Distance(transform.position, fs.transform.position);
            if (range < SM.Vision)
            {
                if (range < preyRange)
                {
                    preyRange = range;
                    prey = fs.transform.position - transform.position;
                }
            }
        }
        prey.Normalize();
    }

    private void SearchForSharks()
    {
        foreach (SharkScript ss in SM.Sharks)
        {
            float range = Vector3.Distance(transform.position, ss.transform.position);
            {
                if (range < SM.Vision * SM.SharkAvoidVisionFactor)
                {
                    if (range < sharkRange)
                    {
                        sharkRange = range;
                        shark = ss.transform.position - transform.position;
                    }
                }
            }
        }
        shark.Normalize();
    }

    private void SearchForTank()
    {
        float Vis = SM.Vision * SM.TankAvoidVisionFactor;
        if (transform.position.y < Vis)
        {
            tankRange = transform.position.y;
            Tank = Vector3.down;
        }
        if (transform.position.y > SM.FM.height - SM.FM.waterOffset - Vis)
        {
            if (SM.tankheight - SM.Vision < tankRange)
            {
                tankRange = transform.position.y;
                Tank = Vector3.up;
            }
        }
        float toedge = SM.FM.tankRadius - Mathf.Sqrt(transform.position.x * transform.position.x + transform.position.z * transform.position.z);
        if (toedge < Vis && toedge < tankRange)
        {
            Tank = new Vector3(transform.position.x, 0, transform.position.z);
        }
        Tank.Normalize();
    }

    private void ApplyWeighting()
    {
        _steering = Vector3.zero + (prey * SM.PreyWeight) - (shark * SM.SharkAvoidWeight) - (Tank * SM.TankAvoidWeight);
        if (_steering == Vector3.zero) _steering = SM.GetWand(transform.forward);
    }

    private void ApplySteering()
    {
        Quaternion Steer = Quaternion.Euler(_steering) * transform.rotation;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Steer, Time.deltaTime * 25);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_steering, transform.up), Time.deltaTime);

        transform.Translate(transform.forward * SM.Speed * Time.deltaTime, Space.World);
    }

}
