using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class BuildObjects : MonoBehaviour
{
    public UnityEngine.Object mazePrefab;
    public UnityEngine.Object fencePrefab;
    public Transform cameraTransform;
    public Sprite inactiveImage;
    public Sprite activeImage;

    public Material redMat;
    public Material greenMat;
    public Material mazeMat;
    // private Material[] fenceMat;

    private bool mazeButton;
    private bool startButton;
    private bool endButton;
    private bool fenceButton;

    private GameObject build;
    private Vector3 target;
    private float targetY;

    [SerializeField]
    private XRNode xrNode = XRNode.LeftHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    private bool pButtonPressInPrevFrame;
    private bool sButtonPressInPrevFrame;

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

    // Start is called before the first frame update
    void Start()
    {
        mazeButton = false;
        startButton = false;
        endButton = false;
        fenceButton = false;
        pButtonPressInPrevFrame = false;
        sButtonPressInPrevFrame = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Sanity check for getting device
        if (!device.isValid)
        {
            GetDevice();
        } 

        bool canBuild = false; // check if object can be correctly built
        bool triggerButtonAction = false; // check if trigger button was pressed
        bool primaryButtonAction = false; // check if primary button was pressed
        bool secondaryButtonAction = false; // check if secondary button was pressed

        // If user is currently building a maze block
        if (mazeButton)
        {
            /*
            // Primary button press moves the object down
            device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonAction);
            if (primaryButtonAction != pButtonPressInPrevFrame)
            {
                if (!primaryButtonAction)
                {
                    if (targetY > 0.5f)
                    {
                        targetY -= 1.0f;
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
                    targetY += 1.0f;
                }
                sButtonPressInPrevFrame = secondaryButtonAction;
            }
            */

            // Store the vector3 position of a point in front of the camera
            target = new Vector3((float)Math.Floor(cameraTransform.position.x) + 0.5f, targetY,
                                    (float)Math.Ceiling(cameraTransform.position.z) + 3.5f);
            // Build the new object in front of the camera position
            build.transform.position = target; 

            // If the object is being incorrectly placed 
            // Conditions include being outside the 10x3x10 workspace boundary
            // (x in [-4.5, 4.5], y in [0.5, 2.5], z in [-4.5, 4.5])
            // Or if it's currently colliding with any other object
            if (target.x < -4.5 || target.x > 4.5 || target.y < 0.5 || target.y > 2.5 ||
                    target.z < -4.5 || target.z > 4.5 || build.GetComponent<CheckCollisionMaze>().anyCollision)
            {
                build.GetComponent<Renderer>().material = redMat;
                canBuild = false;
            }
            else
            {
                build.GetComponent<Renderer>().material = greenMat;
                canBuild = true;
            }

            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonAction) &&
                    triggerButtonAction && canBuild)
            {
                mazeButton = false;
                build.GetComponent<Renderer>().material = mazeMat;
                for (int j = 0; j < gameObject.transform.childCount; j++)
                {
                    Button button = gameObject.transform.GetChild(j).GetComponent<Button>();
                    // Toggle interactable state of the button on and off
                    button.interactable = !button.interactable;
                    // Change the image of the button to BigPink
                    button.GetComponent<Image>().sprite = activeImage;
                }
            }
        }
        /*
        // If user is currently building a fence 
        if (fenceButton)
        {
            // Store the vector3 position of a point in front of the camera
            target = new Vector3((float)Math.Floor(cameraTransform.position.x), 0.75f,
                                    (float)Math.Ceiling(cameraTransform.position.z) + 3.5f);
            // Build the new object in front of the camera position
            build.transform.position = target; 

            // If the object is being incorrectly placed 
            // Conditions include being outside the 10x3x10 workspace boundary
            // (x in [-4, 4], y in {0.5}, z in [-4.5, 4.5])
            // Or if it's currently colliding with any other object
            if (target.x < -4 || target.x > 4 || target.z < -4.5 || target.z > 4.5 || 
                    build.GetComponent<CheckCollisionFence>().anyCollision)
            {
                for (int j = 0; j < build.transform.childCount; j++)
                {
                    build.transform.GetChild(j).GetComponent<Renderer>().material = redMat;
                }
                canBuild = false;
            }
            else
            {
                for (int j = 0; j < build.transform.childCount; j++)
                {
                    build.transform.GetChild(j).GetComponent<Renderer>().material = greenMat;
                }
                canBuild = true;
            }

            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonAction) &&
                    triggerButtonAction && canBuild)
            {
                fenceButton = false;
                for (int j = 0; j < build.transform.childCount; j++)
                {
                    build.transform.GetChild(j).GetComponent<Renderer>().material = fenceMat[j];
                }
                for (int j = 0; j < gameObject.transform.childCount; j++)
                {
                    Button button = gameObject.transform.GetChild(j).GetComponent<Button>();
                    // Toggle interactable state of the button on and off
                    button.interactable = !button.interactable;
                    // Change the image of the button to BigPink
                    button.GetComponent<Image>().sprite = activeImage;
                }
            }
        }*/
    }

    public void OnMazeButtonPress()
    {
        mazeButton = true;
        targetY = 0.5f;
        target = new Vector3((float)Math.Floor(cameraTransform.position.x) + 0.5f, targetY,
                                (float)Math.Ceiling(cameraTransform.position.z) + 3.5f);
        build = (GameObject) Instantiate(mazePrefab, target, Quaternion.identity);
        for (int j = 0; j < gameObject.transform.childCount; j++)
        {
            Button button = gameObject.transform.GetChild(j).GetComponent<Button>();
            // Toggle interactable state of the button on and off
            button.interactable = !button.interactable;
            // Change the image of the button to BigGrey
            button.GetComponent<Image>().sprite = inactiveImage;
        }
    }

    public void OnStartButtonPress()
    {
        startButton = true;
        for (int j = 0; j < gameObject.transform.childCount; j++)
        {
            Button button = gameObject.transform.GetChild(j).GetComponent<Button>();
            // Toggle interactable state of the button on and off
            button.interactable = !button.interactable;
            // Change the image of the button to BigGrey
            button.GetComponent<Image>().sprite = inactiveImage;
        }
    }

    public void OnEndButtonPress()
    {
        endButton = true;
        for (int j = 0; j < gameObject.transform.childCount; j++)
        {
            Button button = gameObject.transform.GetChild(j).GetComponent<Button>();
            // Toggle interactable state of the button on and off
            button.interactable = !button.interactable;
            // Change the image of the button to BigGrey
            button.GetComponent<Image>().sprite = inactiveImage;
        }
    }

    public void OnFenceButtonPress()
    {
        fenceButton = true;
        // target = new Vector3((float)Math.Floor(cameraTransform.position.x), 0.75f,
        //                         (float)Math.Ceiling(cameraTransform.position.z) + 3.5f);
        // build = (GameObject) Instantiate(fencePrefab, target, Quaternion.identity);
        // for (int j = 0; j < build.transform.childCount; j++)
        // {
        //     fenceMat[j] = build.transform.GetChild(j).GetComponent<Renderer>().material;
        // }
        for (int j = 0; j < gameObject.transform.childCount; j++)
        {
            Button button = gameObject.transform.GetChild(j).GetComponent<Button>();
            // Toggle interactable state of the button on and off
            button.interactable = !button.interactable;
            // Change the image of the button to BigGrey
            button.GetComponent<Image>().sprite = inactiveImage;
        }
    }
}
