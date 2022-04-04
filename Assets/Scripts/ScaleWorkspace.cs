using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleWorkspace : MonoBehaviour
{
    public Transform workspace;

    private GameObject canvas;
    private GameObject selecting;
    private GameObject build;
    private GameObject building;
    private GameObject save;
    private GameObject undo;
    private GameObject redo;
    private Slider scale;
    private GameObject done;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        build = canvas.transform.GetChild(0).gameObject;
        save = canvas.transform.GetChild(3).gameObject;
        undo = canvas.transform.GetChild(4).gameObject;
        redo = canvas.transform.GetChild(5).gameObject;
        scale = canvas.transform.GetChild(6).GetComponent<Slider>();
        done = canvas.transform.GetChild(7).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnScaleValueChange()
    {
        build.SetActive(false);
        save.SetActive(false);
        undo.SetActive(false);
        redo.SetActive(false);
        done.SetActive(true);
                  
        workspace.localScale = new Vector3(scale.value, scale.value, scale.value);
    }

    public void OnDoneButtonPress()
    {
        workspace.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        scale.value = 1.0f;

        build.SetActive(true);
        save.SetActive(true);
        undo.SetActive(true);
        redo.SetActive(true);
        done.SetActive(false);
    }
}
