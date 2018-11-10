using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [Header("Prefabs")]
    // Managers
    public GameObject fishManager;
    public GameObject sharkManager;
    // Prefabs
    public GameObject tankInnerPrefab;
    public GameObject tankOutterPrefab;
    public GameObject tankTopPrefab;
    public GameObject floorPrefab;
    public GameObject waterPrefab;
    public GameObject fogTriggerPrefab;

    private GameObject tankInner;
    private GameObject tankOutter;
    private GameObject tankTop;
    private GameObject floor;
    private GameObject water;
    private GameObject fogTrigger;

    [Header("Tank Attributes")]
    [Range(200, 1000)]
    public float tankRadius = 200f;
    [Range(100, 300)]
    public float tankHeight = 100f;
    [Range(0, 50)]
    public float waterOffset = 3f;

    [Header("Spawn Attributes")]
    [Range(0,1)]
    public float fishSpawnCutoff = 0.2f;


    // Use this for initialization
    void Start ()
    {
        // instantiate level
        water = Instantiate(waterPrefab, Vector3.zero, Quaternion.identity);
        tankInner = Instantiate(tankInnerPrefab, Vector3.zero, Quaternion.identity);
        tankOutter = Instantiate(tankOutterPrefab, Vector3.zero, Quaternion.identity);
        tankTop = Instantiate(tankTopPrefab, Vector3.zero, Quaternion.identity);
        floor = Instantiate(floorPrefab, new Vector3(0f, -1f, 0f), Quaternion.identity);
        floor.transform.localScale = new Vector3(tankRadius / 2f, 1, tankRadius / 2f);
        fogTrigger = Instantiate(fogTriggerPrefab, Vector3.zero, Quaternion.identity);

        FishManager FM = fishManager.GetComponent<FishManager>();
        FM.tankRadius = tankRadius;
        FM.height = tankHeight;
        FM.spawnRadius = tankRadius * fishSpawnCutoff;
        FM.waterOffset = waterOffset;

        // delete water to fix texture bug
        Destroy(water);
    }
	
	// Update is called once per frame
	void Update ()
    {
        tankOutter.transform.localScale = new Vector3(tankRadius, tankHeight, tankRadius);
        tankInner.transform.localScale = tankOutter.transform.localScale;
        tankTop.transform.localScale = new Vector3(tankOutter.transform.localScale.x, 1, tankOutter.transform.localScale.z);
        fogTrigger.transform.localScale = new Vector3(tankRadius * 2, tankHeight - (waterOffset / 2), tankRadius * 2);

        tankRadius = tankOutter.transform.localScale.x;
        tankTop.transform.position = new Vector3(0f, tankHeight - waterOffset, 0f);
    }


}
