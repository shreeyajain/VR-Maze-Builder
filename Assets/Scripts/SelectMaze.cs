using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectMaze : MonoBehaviour
{
    public Material greenMat;
    public Material mazeMat;

    private bool select;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        select = true;
        gameObject.GetComponent<Renderer>().material = greenMat;
    }

    public void Deselect()
    {
        select = false;
        gameObject.GetComponent<Renderer>().material =  mazeMat;
    }
}
