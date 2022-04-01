using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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
        for (int j = 0; j < gameObject.transform.childCount; j++)
        {
            gameObject.transform.GetChild(j).GetComponent<Renderer>().material = greenMat;
        }
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
