using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisionMaze : MonoBehaviour
{
    public Material redMat;
    public Material mazeMat;

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
        gameObject.GetComponent<Renderer>().material = redMat;
    }

    void OnTriggerExit(Collider other)
    {
        anyCollision = false;
        gameObject.GetComponent<Renderer>().material = mazeMat;
    }
}
