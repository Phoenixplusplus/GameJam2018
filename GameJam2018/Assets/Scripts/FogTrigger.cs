using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTrigger : MonoBehaviour {

    public Material tankMaterial;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Main Camera")
        {
            RenderSettings.fog = true;
            tankMaterial.color = new Color(0.08957814f, 0.2421157f, 0.3113208f, 1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Main Camera")
        {
            RenderSettings.fog = false;
            tankMaterial.color = new Color(tankMaterial.color.r, tankMaterial.color.g, tankMaterial.color.b, 0.25f);
        }
    }
}
