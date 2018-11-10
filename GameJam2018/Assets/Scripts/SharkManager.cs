using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkManager : MonoBehaviour {

    public Transform shark;
    public FishScript[] Fish;
    public SharkScript[] Sharks;
    public FishManager FM;
    public Wander Wand = new Wander();

    [Header("Shark Variables")]
    [Range(1,200)]
    public int SharkCount = 5;
    [Range(0,50)]
    public float WanderRange = 50;
    [Range(0,20)]
    public float WanderRadius = 20;
    [Range(0,50)]
    public float Vision = 30;
    [Range(0,10)]
    public float Speed = 4;
    [Range(0,90)]
    public float TurnRate = 90;
    [Range(0,1)] 
    public float TankAvoidVisionFactor = 0.5f;
    [Range(0,1)]
    public float SharkAvoidVisionFactor = 0.9f;

    [Header("WEIGHTINGS")]
    [Range(0,20)]
    public float SharkAvoidWeight = 10;
    [Range(0, 20)]
    public float PreyWeight = 10;
    [Range(0, 20)]
    public float TankAvoidWeight = 10;

    // Use this for initialization
    void Start () {
        Sharks = new SharkScript[SharkCount];
        for (int i = 0; i < SharkCount; i++)
        {
            float SpawnHeight = Random.Range(1f, FM.height - FM.waterOffset - 1f);
            float SpawnAngle = Random.Range(0f, 2 * Mathf.PI);
            Transform MyShark = Instantiate(shark, new Vector3(Mathf.Cos(SpawnAngle) * Random.Range(FM.spawnRadius + 1, FM.tankRadius- FM.spawnRadius -1), SpawnHeight, Mathf.Sin(SpawnAngle) * Random.Range(FM.spawnRadius + 1, FM.tankRadius - FM.spawnRadius - 1)), Random.rotation);
            Sharks[i] = MyShark.GetComponent<SharkScript>();
            Sharks[i].ID = i;
            Sharks[i].SM = this;
        }
        FM.Sharks = Sharks;
    }

    public Vector3 GetWand (Vector3 forward)
    {
        return Wand.WanderSteer(forward, WanderRadius, WanderRange);
    }

    // delete sharks and restart
    public void DeleteShark()
    {
        foreach (SharkScript ss in Sharks)
        {
            Destroy(ss.gameObject);
        }
        Start();
    }
}
