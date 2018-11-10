using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform LowerPoint;
    public Transform UpperPoint;
    private GameObject Following;
    private bool isFollowing;
    private Transform Anchor;
    public FishManager FM;
    public Vector3 Disp = new Vector3(0, 2, -1);
    public float LowerFoV = 30;
    public float UpperFoV = 50;
    public float LowerRot = 8.5f;
    public float UpperRot = 14f;
    public float ScrollSense = 0.5f;
    public float SWShift = 150;
    public float ADShift = 50;
    public float scrollZoom = 20;
    [SerializeField] private float _Lerp = 1;
    [SerializeField] private bool LMBDown;

    public void Activate()
    {
        FM = FindObjectOfType<FishManager>();
        LowerPoint.transform.localPosition = new Vector3(0, FM.height / 2, 0);
        UpperPoint.transform.localPosition = new Vector3(FM.tankRadius, FM.height + FM.waterOffset + 1, 0);
        UpdateCameraZoom();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("t")) FollowRandomFISH();
        if (Input.GetKey("y")) FollowRandomShark();

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse is down");

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.tag == "HasCameraBoom")
                {
                    Debug.Log("It's working!");
                    Following = hitInfo.transform.gameObject;
                    StartFollowing();
                }
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && _Lerp <= 0.01f)
            {
                ZoomIN();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && _Lerp >= 1)
            {
                ZoomOUT();
            }
            else
            {
                _Lerp -= Input.GetAxis("Mouse ScrollWheel") * ScrollSense;
                _Lerp = Mathf.Clamp(_Lerp, 0.01f, 1);
            }
        }

        if (isFollowing)
        {
            transform.position = Anchor.transform.position - (Anchor.transform.forward * 0.5f);
            transform.LookAt(Anchor.transform.position);
            return;
        }


        if (Input.GetMouseButtonDown(1)) LMBDown = true;
        if (Input.GetMouseButtonUp(1)) LMBDown = false;
        if (LMBDown)
        {
            if (Input.GetAxis("Mouse X") !=0) RollRIGHT(Input.GetAxis("Mouse X"));   
            if (Input.GetAxis("Mouse Y") != 0) RollUP(-Input.GetAxis("Mouse Y"));
        }

        if (Input.GetKey("w")) RollUP(-1f);
        if (Input.GetKey("s")) RollUP(1f);
        if (Input.GetKey("a")) RollRIGHT(-1f);
        if (Input.GetKey("d")) RollRIGHT(1f);
        if (Input.GetKey("q")) ZoomOUT();
        if (Input.GetKey("e")) ZoomIN();




        // For Dev purposes ... to find optimal camera angle/ boom positions 
        // Re-locate into above condition when ready to save a little overhead
        UpdateCameraZoom();
    }

    void RollUP (float mod)
    {
        transform.RotateAround(LowerPoint.transform.localPosition, Vector3.right, SWShift * Time.deltaTime * mod);
    }

    void RollRIGHT(float mod)
    {
        transform.RotateAround(LowerPoint.transform.localPosition, Vector3.up, -ADShift * Time.deltaTime * mod);
    }

    void ZoomIN()
    {
        transform.Translate(Camera.main.transform.forward * -scrollZoom * Time.deltaTime, Space.World);
    }

    void ZoomOUT()
    {
        transform.Translate(Camera.main.transform.forward * scrollZoom * Time.deltaTime, Space.World);
    }

    void StartFollowing()
    {
        Anchor = Following.transform.Find("CameraAnchor").transform;
        isFollowing = true;
    }

    public void StopFollowing()
    {
        transform.parent = null;
        isFollowing = false;
    }

    public void FollowRandomFISH()
    {
        FishScript meh = FM.Fish[Random.Range(0, FM.Fish.Length - 1)];
        if (meh.isActive)
        {
            Anchor = meh.transform;
            isFollowing = true;
        }
        else
        {
            FollowRandomFISH();
        }
    }

    public void FollowRandomShark()
    {
        SharkScript meh = FM.Sharks[Random.Range(0, FM.Sharks.Length - 1)];
        Anchor = meh.transform;
        isFollowing = true;
    }


    void UpdateCameraZoom()
    {
        Camera.main.transform.localPosition = Vector3.Lerp(LowerPoint.localPosition, UpperPoint.localPosition, _Lerp);
        Camera.main.fieldOfView = LowerFoV + ((UpperFoV - LowerFoV) * _Lerp);
        Camera.main.transform.LookAt(LowerPoint.transform.position);
    }


}
