using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisionStart : MonoBehaviour
{
    public Material redMat;
    public Material startMat;

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
        anyCollision = true;
        for (int j = 0; j < gameObject.transform.childCount; j++)
        {
            gameObject.transform.GetChild(j).GetComponent<Renderer>().material = redMat;
        }
    }

    void OnTriggerExit(Collider other)
    {
        anyCollision = false;
        for (int j = 0; j < gameObject.transform.childCount; j++)
        {
            gameObject.transform.GetChild(j).GetComponent<Renderer>().material = startMat;
        }
    }
}
