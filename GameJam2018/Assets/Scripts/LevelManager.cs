﻿using System.Collections;
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

    private GameObject tankInner;
    private GameObject tankOutter;
    private GameObject tankTop;
    private GameObject floor;

    [Header("Tank Attributes")]
    [Range(200, 1000)]
    public float tankRadius = 500f;
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
        tankInner = Instantiate(tankInnerPrefab, Vector3.zero, Quaternion.identity);
        tankOutter = Instantiate(tankOutterPrefab, Vector3.zero, Quaternion.identity);
        tankTop = Instantiate(tankTopPrefab, Vector3.zero, Quaternion.identity);
        floor = Instantiate(floorPrefab, Vector3.zero, Quaternion.identity);

        FishManager FM = fishManager.GetComponent<FishManager>();
        FM.tankRadius = tankRadius;
        FM.height = tankHeight;
        FM.spawnRadius = tankRadius * fishSpawnCutoff;

    }
	
	// Update is called once per frame
	void Update ()
    {
        tankOutter.transform.localScale = new Vector3(tankRadius, tankHeight, tankRadius);
        tankInner.transform.localScale = tankOutter.transform.localScale;
        tankTop.transform.localScale = new Vector3(tankOutter.transform.localScale.x, 1, tankOutter.transform.localScale.z);

        tankRadius = tankOutter.transform.localScale.x;
        tankTop.transform.position = new Vector3(0f, tankHeight - waterOffset, 0f);
    }
}