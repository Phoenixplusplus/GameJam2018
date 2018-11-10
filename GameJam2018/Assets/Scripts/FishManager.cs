using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour {

    public Transform fish;

    [Header("Glo-Bowl Variables")]
    public float height;
    public float spawnRadius;
    public float tankRadius;
    public float waterOffset;

    public FishScript[] Fish;
    public SharkScript[] Sharks;
    public SharkManager SM;
    public Wander Wand = new Wander();

    [Header("Fish Variables")]
    [Range(1, 500)]
    public int FishCount = 20;
    [Range(0,50)]
    public float WanderRange = 50;
    [Range(0, 20)]
    public float WanderRadius = 20;
    [Range(0, 20)]
    public float Speed = 3;
    [Range(0, 90)]
    public float TurnRate = 90;
    [Range(0, 30)]
    public float Vision = 20;
    [Range (0,1)]
    public float AvoidFishVisionRatio = 0.2f;
    [Range(0, 1)]
    public float AvoidTankVisionRatio = 0.2f;

    [Header("WEIGHTINGS")]
    [Range(0, 20)]
    public float CoMWeight = 10;
    [Range(0, 20)]
    public float CoRWeight = 1;
    [Range(0, 20)]
    public float AvoidFishWeight = 0.2f;
    [Range(0, 20)]
    public float SharkAvoidWeight = 10;
    [Range(0, 20)]
    public float TankAvoidWeight = 2;


    // Use this for initialization
    void Start () {

        Fish = new FishScript[FishCount];
        for(int i = 0; i < FishCount; i++)
        {
            float SpawnHeight = Random.Range(1f, height - waterOffset);
            float SpawnAngle = Random.Range(0f, 2*Mathf.PI);
            Transform MyFish = Instantiate(fish, new Vector3(Mathf.Cos(SpawnAngle) * Random.Range(0, spawnRadius), SpawnHeight, Mathf.Sin(SpawnAngle) * Random.Range(0, spawnRadius)), Random.rotation);
            Fish[i] = MyFish.GetComponent<FishScript>();
            Fish[i].ID = i;
            Fish[i].FM = this;
        }
        SM.Fish = Fish;
    }



    public Vector3 GetWand(Vector3 forward)
    {
        return Wand.WanderSteer(forward, WanderRadius, WanderRange);
    }

}
