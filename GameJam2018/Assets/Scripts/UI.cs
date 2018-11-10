using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public GameObject FMO;
    private FishManager FM;
    public float timeleft;

    public bool toggleUI = false;

    [Header("Sliders & Text")]
    public Text TimeT;
    public Text FishiesLeftT;
    public GameObject WanderRangeO;
    public Slider WanderRangeS;
    public Text WanderRangeT;
    public GameObject WanderRadiusO;
    public Slider WanderRadiusS;
    public Text WanderRadiusT;
    public GameObject SpeedO;
    public Slider SpeedS;
    public Text SpeedT;
    public GameObject TurnRateO;
    public Slider TurnRateS;
    public Text TurnRateT;
    public GameObject VisionO;
    public Slider VisionS;
    public Text VisionT;
    public GameObject AvoidFishRatioO;
    public Slider AvoidFishRatioS;
    public Text AvoidFishRatioT;
    public GameObject CoMWeightO;
    public Slider CoMWeightS;
    public Text CoMWeightT;
    public GameObject CoRWeightO;
    public Slider CoRWeightS;
    public Text CoRWeightT;
    public GameObject FishAvoidWeightO;
    public Slider FishAvoidWeightS;
    public Text FishAvoidWeightT;
    public GameObject SharkAvoidWeightO;
    public Slider SharkAvoidWeightS;
    public Text SharkAvoidWeightT;
    public GameObject NoTimeO;
    public Text NotimeT;

    // Use this for initialization
    void Start ()
    {
        FM = FMO.GetComponent<FishManager>();
        timeleft = 120f;

        NoTimeO.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        FishiesLeftT.text = FM.FishCount.ToString("0");
        WanderRangeT.text = FM.WanderRange.ToString("0.0");
        WanderRadiusT.text = FM.WanderRadius.ToString("0.0");
        SpeedT.text = FM.Speed.ToString("0.0");
        TurnRateT.text = FM.TurnRate.ToString("0.0");
        VisionT.text = FM.Vision.ToString("0.0");
        AvoidFishRatioT.text = FM.AvoidFishVisionRatio.ToString("0.0");
        CoMWeightT.text = FM.CoMWeight.ToString("0.0");
        CoRWeightT.text = FM.CoRWeight.ToString("0.0");
        SharkAvoidWeightT.text = FM.SharkAvoidWeight.ToString("0.0");
        ToggleUI();
        SetTimer();

        // ran out of time splash
        if (timeleft < 0)
        {
            timeleft = 0;
            NoTimeO.gameObject.SetActive(true);
            NotimeT.text = "You ran out of time with " + FM.FishCount + " fishies left";
        }
	}

    // sets
    public void SetWanderRange(float i) { FM.WanderRange = i; }
    public void SetWanderRadius(float i) { FM.WanderRadius = i; }
    public void SetSpeed(float i) { FM.Speed = i; }
    public void SetTurnRate(float i) { FM.TurnRate = i; }
    public void SetVision(float i) { FM.Vision = i; }
    public void SetAvoidFishRatio(float i) { FM.AvoidFishVisionRatio = i; }
    public void SetCoMWeight(float i) { FM.CoMWeight = i; }
    public void SetCoRWeight(float i) { FM.CoRWeight = i; }
    public void SetFishAvoidWeight(float i) { FM.AvoidFishWeight = i; }
    public void SetSharkAvoidWeight(float i) { FM.SharkAvoidWeight = i; }

    // toggle UI
    public void ToggleUI() { transform.GetChild(0).gameObject.SetActive(toggleUI); }
    public void toggleBool() { toggleUI = !toggleUI; }

    // set timer
    public void SetTimer()
    {
        if (timeleft > 0)
        {
            timeleft -= Time.deltaTime;

            float minutes = Mathf.Floor(timeleft / 60);
            float seconds = timeleft % 60;
            if (seconds > 59) seconds = 59;

            TimeT.text = "Timeleft: " + minutes.ToString("0") + ":" + seconds.ToString("0");
        }

        if (timeleft == 0) TimeT.text = "Timeleft: 0:0";
    }

    public void ResetTimer() { Start(); }

    public void ResetSliders()
    {
        FishiesLeftT.text = FM.FishCount.ToString();
        WanderRangeS.value = FM.WanderRange;
        WanderRadiusS.value = FM.WanderRadius;
        SpeedS.value = FM.Speed;
        TurnRateS.value = FM.TurnRate;
        VisionS.value = FM.Vision;
        AvoidFishRatioS.value = FM.AvoidFishVisionRatio;
        CoMWeightS.value = FM.CoMWeight;
        CoRWeightS.value = FM.CoRWeight;
        FishAvoidWeightS.value = FM.AvoidFishWeight;
        SharkAvoidWeightS.value = FM.SharkAvoidWeight;
    }
}
