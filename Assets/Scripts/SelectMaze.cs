using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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

    private float targetY;

    void Awake()
    {
        select = false;
        wasBuildActive = false;
        wasBuildingActive = false;

        canvas = GameObject.Find("Canvas");
        build = canvas.transform.GetChild(0).gameObject;
        building = canvas.transform.GetChild(1).gameObject;
        selecting = canvas.transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        select = true;
        gameObject.GetComponent<Renderer>().material = greenMat;
        targetY = gameObject.transform.position.y;
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
        gameObject.GetComponent<Renderer>().material =  mazeMat;
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
