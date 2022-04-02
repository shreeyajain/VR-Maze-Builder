using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.UI;

public class SelectMaze : MonoBehaviour
{
    public Material greenMat;
    public Material mazeMat;

    private GameObject canvas;
    private GameObject selecting;
    private GameObject build;
    private GameObject building;

    private bool select;
    private bool wasBuildActive;
    private bool wasBuildingActive;

    [SerializeField]
    private XRNode xrNode = XRNode.RightHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    private bool pButtonPressInPrevFrame;
    private bool sButtonPressInPrevFrame;

    private Vector3 target;
    private Vector3 prevPos;
    private Vector3 targetRot;
    private Vector3 prevRot;
    private bool canMove;
    private bool rotate;

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xrNode, devices);
        device = devices.FirstOrDefault();
    }

    void OnEnable()
    {
        if (!device.isValid)
        {
            GetDevice();
        }
    }

    void Awake()
    {
        select = false;
        wasBuildActive = false;
        wasBuildingActive = false;
        canMove = true;
        rotate = false;
        pButtonPressInPrevFrame = false;
        sButtonPressInPrevFrame = false;

        canvas = GameObject.Find("Canvas");
        build = canvas.transform.GetChild(0).gameObject;
        building = canvas.transform.GetChild(1).gameObject;
        selecting = canvas.transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Sanity check for getting device
        if (!device.isValid)
        {
            GetDevice();
        } 

        bool primaryButtonAction = false; // check if primary button was pressed
        bool secondaryButtonAction = false; // check if secondary button was pressed

        Vector2 move = Vector2.zero;
        if (select)
        {
            gameObject.GetComponent<Renderer>().material = greenMat;
            target = transform.position;

            // Joystick movement to move the maze in x and z axes
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out move) && move != Vector2.zero)
            {
                // Move in x direction
                if (Mathf.Abs(move.x) > Mathf.Abs(move.y) && Mathf.Abs(move.x) > 0.6f && canMove)
                {
                    canMove = false;
                    prevPos = transform.position;
                    prevRot = transform.eulerAngles;
                    if (move.x > 0.0f)
                    {
                        target = transform.position + new Vector3(1, 0, 0);
                    }
                    else
                    {
                        target = transform.position - new Vector3(1, 0, 0);
                    }
                }
                // Move in z direction
                else if (Mathf.Abs(move.x) < Mathf.Abs(move.y) && Mathf.Abs(move.y) > 0.6f && canMove)
                {
                    canMove = false;
                    prevPos = transform.position;
                    prevRot = transform.eulerAngles;
                    if (move.y > 0.0f)
                    {
                        target = transform.position + new Vector3(0, 0, 1);
                    }
                    else
                    {
                        target = transform.position - new Vector3(0, 0, 1);
                    }
                }
            }
            else
            {
                canMove = true;
            }

            // Primary button press moves the object down
            device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonAction);
            if (primaryButtonAction != pButtonPressInPrevFrame)
            {
                if (!primaryButtonAction)
                {
                    if (transform.position.y > 0.5f)
                    {
                        prevPos = transform.position;
                        target = transform.position - new Vector3(0, 1, 0);
                    }
                }
                pButtonPressInPrevFrame = primaryButtonAction;
            }
            // Secondary button press moves the object up
            device.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonAction);
            if (secondaryButtonAction != sButtonPressInPrevFrame)
            {
                if (!secondaryButtonAction)
                {
                    if (transform.position.y < 2.5f)
                    {
                        prevPos = transform.position;
                        target = transform.position + new Vector3(0, 1, 0);
                    }
                }
                sButtonPressInPrevFrame = secondaryButtonAction;
            }

            // If the object is being incorrectly placed 
            // Conditions include being inside the 12x3x12 workspace boundary
            // (x in [-5.5, 5.5], y in [0.5, 2.5], z in [-5.5, 5.5])
            // Or if it's not currently colliding with any other object
            if (target.x >= -5.5 && target.x <= 5.5 && target.y >= 0.5 && target.y <= 2.5 &&
                    target.z >= -5.5 && target.z <= 5.5)
            {
                transform.position = target;
            }

            if (rotate)
            {
                // Only rotate it once
                rotate = false;
                transform.Rotate(targetRot);
            }

            if (gameObject.GetComponent<CheckCollisionMaze>().anyCollision)
            {
                transform.position = prevPos;
                transform.eulerAngles = prevRot;
            }
        }
    }

    public void Select()
    {
        select = true;
        gameObject.GetComponent<Renderer>().material = greenMat;
        gameObject.GetComponent<Collider>().isTrigger = true;

        selecting.SetActive(true);
        if (build.activeSelf)
        {
            wasBuildActive = true;
            build.SetActive(false);
        }
        else if (building.activeSelf)
        {
            wasBuildingActive = true;
            building.SetActive(false);
        }

        Button Xplus = selecting.transform.GetChild(0).GetComponent<Button>();
		Xplus.onClick.AddListener(() => RotatePlusX());
        Button Xneg = selecting.transform.GetChild(1).GetComponent<Button>();
		Xneg.onClick.AddListener(() => RotateNegX());
        Button Yplus = selecting.transform.GetChild(2).GetComponent<Button>();
		Yplus.onClick.AddListener(() => RotatePlusY());
        Button Yneg = selecting.transform.GetChild(3).GetComponent<Button>();
		Yneg.onClick.AddListener(() => RotateNegY());
        Button Zplus = selecting.transform.GetChild(4).GetComponent<Button>();
		Zplus.onClick.AddListener(() => RotatePlusZ());
        Button Zneg = selecting.transform.GetChild(5).GetComponent<Button>();
		Zneg.onClick.AddListener(() => RotateNegZ());
    }

    public void Deselect()
    {
        select = false;
        gameObject.GetComponent<Renderer>().material =  mazeMat;
        gameObject.GetComponent<Collider>().isTrigger = false;

        Button Xplus = selecting.transform.GetChild(0).GetComponent<Button>();
		Xplus.onClick.RemoveListener(() => RotatePlusX());
        Button Xneg = selecting.transform.GetChild(1).GetComponent<Button>();
		Xneg.onClick.RemoveListener(() => RotateNegX());
        Button Yplus = selecting.transform.GetChild(2).GetComponent<Button>();
		Yplus.onClick.RemoveListener(() => RotatePlusY());
        Button Yneg = selecting.transform.GetChild(3).GetComponent<Button>();
		Yneg.onClick.RemoveListener(() => RotateNegY());
        Button Zplus = selecting.transform.GetChild(4).GetComponent<Button>();
		Zplus.onClick.RemoveListener(() => RotatePlusZ());
        Button Zneg = selecting.transform.GetChild(5).GetComponent<Button>();
		Zneg.onClick.RemoveListener(() => RotateNegZ());
        
        selecting.SetActive(false);
        if (wasBuildActive)
        {
            wasBuildActive = false;
            build.SetActive(true);
        }
        else if (wasBuildingActive)
        {
            wasBuildingActive = false;
            building.SetActive(true);
        }
    }

    public void RotatePlusX()
    {
        rotate = true;
        prevPos = transform.position;
        prevRot = transform.eulerAngles;
        targetRot = new Vector3(90, 0, 0);
    }

    public void RotateNegX()
    {
        rotate = true;
        prevPos = transform.position;
        prevRot = transform.eulerAngles;
        targetRot = new Vector3(-90, 0, 0);
    }

    public void RotatePlusY()
    {
        rotate = true;
        prevPos = transform.position;
        prevRot = transform.eulerAngles;
        targetRot = new Vector3(0, 90, 0);
    }

    public void RotateNegY()
    {
        rotate = true;
        prevPos = transform.position;
        prevRot = transform.eulerAngles;
        targetRot = new Vector3(0, -90, 0); 
    }

    public void RotatePlusZ()
    {
        rotate = true;
        prevPos = transform.position;
        prevRot = transform.eulerAngles;
        targetRot = new Vector3(0, 0, 90);
    }

    public void RotateNegZ()
    {
        rotate = true;
        prevPos = transform.position;
        prevRot = transform.eulerAngles;
        targetRot = new Vector3(0, 0, -90); 
    }
}
