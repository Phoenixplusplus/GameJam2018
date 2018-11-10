using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkManager : MonoBehaviour {

    public Transform shark;
    public FishScript[] Fish;
    public SharkScript[] Sharks;
    public FishManager FM;
    public Wander Wand = new Wander();

    public int SharkCount = 5;
    public int tankheight = 50;
    public int Spawnradius = 100;
    public float Vision = 30;
    public float Speed = 4;
    public float TurnRate = 90;
    public float SharkAvoidVisionFactor = 0.9f;
    public float SharkAvoidWeight = 10;
    public float PreyWeight = 10;
    public float TankAvoidVisionFactor = 0.5f;
    public float TankAvoidWeight = 10;
    public float WanderRange = 50;
    public float WanderRadius = 20;

    // Use this for initialization
    void Start () {
        Sharks = new SharkScript[SharkCount];
        for (int i = 0; i < SharkCount; i++)
        {
            float SpawnHeight = Random.Range(1f, tankheight - 1f);
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

}
