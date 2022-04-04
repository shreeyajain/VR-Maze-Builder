using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisionEnd : MonoBehaviour
{
    public Material redMat;
    public Material endMat1;
    public Material endMat2;
    public Material endMat3;

    public bool anyCollision;

    // Start is called before the first frame update
    void Start()
    {
        anyCollision = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Hand"))
        {
            anyCollision = true;
            for (int j = 0; j < gameObject.transform.childCount; j++)
            {
                gameObject.transform.GetChild(j).GetComponent<Renderer>().material = redMat;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Hand"))
        {
            anyCollision = false;
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
}
