using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour {

    public Transform fish;
    public float height = 50;
    public float Radius = 50;
    public int FishCount = 20;
    public FishScript[] Pool;
    private Vector3 _target;

    [Header("Fish Variables")]
    [Range(0, 20)]
    public float Speed = 3;
    [Range(0, 90)]
    public float TurnRate = 90;
    [Range(0, 20)]
    public float Vision = 20;


    // Use this for initialization
    void Start () {

        Pool = new FishScript[FishCount];
        for(int i = 0; i < FishCount; i++)
        {
            float SpawnHeight = Random.Range(1f, height - 1f);
            float SpawnAngle = Random.Range(0f, 2*Mathf.PI);
            Transform MyFish = Instantiate(fish, new Vector3(Mathf.Cos(SpawnAngle) * Random.Range(0,Radius), SpawnHeight, Mathf.Sin(SpawnAngle) * Random.Range(0, Radius)),Quaternion.identity);
            Pool[i] = MyFish.GetComponent<FishScript>();
            Pool[i].ID = i;
        }
        for (int i = 0; i < FishCount; i++)
        {
            Pool[i].Others = Pool;
        }

        }
	
	// Update is called once per frame
	void Update () {
		
	}
}
