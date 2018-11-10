using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkManager : MonoBehaviour {

    public Transform shark;
    public SharkScript[] SharkPool;

    public int SharkCount = 5;
    public int tankheight = 50;
    public int Spawnradius = 100;
    public float Vision = 30;
    public float Speed = 4;
    public float TurnRate = 90;
    public float SharkAvoidVisionFactor = 0.9f;
    public float SharkAvoidFactor = 10;
    public float PreyWeight = 10;
    public float TankAvoidWeight = 10;

    // Use this for initialization
    void Start () {
        SharkPool = new SharkScript[SharkCount];
        for (int i = 0; i < SharkCount; i++)
        {
            float SpawnHeight = Random.Range(1f, tankheight - 1f);
            float SpawnAngle = Random.Range(0f, 2 * Mathf.PI);
            Transform MyShark = Instantiate(shark, new Vector3(Mathf.Cos(SpawnAngle) * Random.Range(0, Spawnradius), SpawnHeight, Mathf.Sin(SpawnAngle) * Random.Range(0, Spawnradius)), Random.rotation);
            SharkPool[i] = MyShark.GetComponent<SharkScript>();
            SharkPool[i].ID = i;
        }
        for (int i = 0; i < SharkCount; i++)
        {
            SharkPool[i].Others = SharkPool;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
