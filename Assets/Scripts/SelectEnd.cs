using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class SelectEnd : MonoBehaviour
{
    public Material greenMat;
    public Material endMat1;
    public Material endMat2;
    public Material endMat3;

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

    private Vector3 target;
    private Vector3 prevPos;
    private bool canMove;

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

        Vector2 move = Vector2.zero;
        if (select)
        {
            for (int j = 0; j < gameObject.transform.childCount; j++)
            {
                gameObject.transform.GetChild(j).GetComponent<Renderer>().material = greenMat;
            }
            target = transform.position;
            // Joystick movement to move the end well in x and z axes
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out move) && move != Vector2.zero)
            {
                // Move in x direction
                if (Mathf.Abs(move.x) > Mathf.Abs(move.y) && Mathf.Abs(move.x) > 0.6f && canMove)
                {
                    canMove = false;
                    prevPos = transform.position;
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

            // If the object is being incorrectly placed 
            // Conditions include being inside the 12x3x12 workspace boundary
            // (x in [1.19, 11.19], y in {0.0}, z in [-3.06, 7.06])
            // Or if it's not currently colliding with any other object
            if (target.x >= 1.19 && target.x <= 11.20 && target.y == 0.0 &&
                    target.z >= -3.06 && target.z <= 7.06)
            {
                transform.position = target;
            }
            if (gameObject.GetComponent<CheckCollisionEnd>().anyCollision)
            {
                transform.position = prevPos;
            }
        }
    }

    public void Select()
    {
        select = true;
        for (int j = 0; j < gameObject.transform.childCount; j++)
        {
            gameObject.transform.GetChild(j).GetComponent<Renderer>().material = greenMat;
        }
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
    }

    public void Deselect()
    {
        select = false;
        gameObject.transform.GetChild(0).GetComponent<Renderer>().material = endMat1;
        for (int j = 1; j < 3; j++)
        {
            gameObject.transform.GetChild(j).GetComponent<Renderer>().material = endMat2;
        }
        for (int j = 3; j < gameObject.transform.childCount; j++)
        {
            gameObject.transform.GetChild(j).GetComponent<Renderer>().material = endMat3;
        }
        gameObject.GetComponent<Collider>().isTrigger = false;
        
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
}
