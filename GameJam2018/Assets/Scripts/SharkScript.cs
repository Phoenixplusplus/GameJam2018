using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkScript : MonoBehaviour {

    private Vector3 _steering;

    public Vector3 prey;
    public float preyRange;
    public Vector3 Tank;
    public Vector3 shark;
    public float sharkRange;

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
        shark = Vector3.zero;
        prey = Vector3.zero;
        Tank = Vector3.zero;

        SearchForLunch();
        SearchForSharks();
        SearchForTank();

        ApplyWeighting();
        ApplySteering();

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
    }

    private void SearchForTank()
    {

    }
    private void ApplyWeighting()
    {
        _steering = Vector3.zero + (prey * SM.PreyWeight) - (shark * SM.SharkAvoidWeight) - (Tank * SM.TankAvoidWeight);
    }
    private void ApplySteering()
    {

    }

}
