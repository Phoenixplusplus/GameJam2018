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
    [Range(1, 200)]
    public int FishCount = 20;

    public FishScript[] Pool;

    [Header("Fish Variables")]
    [Range(0, 20)]
    public float Speed = 3;
    [Range(0, 90)]
    public float TurnRate = 90;
    [Range(0, 20)]
    public float Vision = 20;
    [Range (0,1)]
    public float AvoidFishVisionRatio = 0.2f;
    
    [Header("WEIGHTINGS")]
    [Range(0, 20)]
    public float CoMWeight = 10;
    [Range(0, 20)]
    public float CoRWeight = 1;
    [Range(0, 20)]
    public float AvoidFishWeight = 2;
    [Range(0, 20)]
    public float SharkAvoidWeight = 10;
    [Range(0, 20)]
    public float TankAvoidWeight = 100;


    // Use this for initialization
    void Start () {

        Pool = new FishScript[FishCount];
        for(int i = 0; i < FishCount; i++)
        {
            float SpawnHeight = Random.Range(1f, height - waterOffset);
            float SpawnAngle = Random.Range(0f, 2*Mathf.PI);
            Transform MyFish = Instantiate(fish, new Vector3(Mathf.Cos(SpawnAngle) * Random.Range(0, spawnRadius), SpawnHeight, Mathf.Sin(SpawnAngle) * Random.Range(0, spawnRadius)), Random.rotation);
            Pool[i] = MyFish.GetComponent<FishScript>();
            Pool[i].ID = i;
        }
        for (int i = 0; i < FishCount; i++)
        {
            Pool[i].Others = Pool;
        }
    }

    void RecieveSharks (SharkScript[] Sharkpool, SharkManager sender)
    {

    }

    public FishScript[] SendFish ()
    {
        return Pool;
    }

	
}
