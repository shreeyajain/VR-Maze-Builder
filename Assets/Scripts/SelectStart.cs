using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectStart : MonoBehaviour
{
    public Material greenMat;
    public Material startMat;

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
        for (int j = 0; j < gameObject.transform.childCount; j++)
        {
            gameObject.transform.GetChild(j).GetComponent<Renderer>().material = startMat;
        }
    }
}
