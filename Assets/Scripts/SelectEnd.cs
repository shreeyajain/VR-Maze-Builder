using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectEnd : MonoBehaviour
{
    public Material greenMat;
    public Material endMat1;
    public Material endMat2;
    public Material endMat3;

    private bool select;

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
    }
}
