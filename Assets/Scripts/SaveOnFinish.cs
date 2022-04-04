using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SaveOnFinish : MonoBehaviour
{
    private GameObject[] mazeBlocks;
    private GameObject[] fences;
    private GameObject start;
    private GameObject end;

    public Sprite inactiveImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSaveButtonPress()
    {
        mazeBlocks = GameObject.FindGameObjectsWithTag("maze");
        fences = GameObject.FindGameObjectsWithTag("fence");
        start = GameObject.FindWithTag("start");
        end = GameObject.FindWithTag("end");

        string path = Application.persistentDataPath + "/test.json";

        JSONData data = new JSONData();

        foreach (GameObject maze in mazeBlocks)
        {
            ObjectsBuilt obj = new ObjectsBuilt("maze", maze.transform.position, maze.transform.eulerAngles);
            data.objects.Add(obj);
        }

        foreach (GameObject fence in fences)
        {
            ObjectsBuilt obj = new ObjectsBuilt("fence", fence.transform.position, fence.transform.eulerAngles);
            data.objects.Add(obj);
        }

        data.objects.Add(new ObjectsBuilt("start", start.transform.position, start.transform.eulerAngles));
        data.objects.Add(new ObjectsBuilt("end", end.transform.position, end.transform.eulerAngles));

        File.WriteAllText(path, JsonUtility.ToJson(data));
        gameObject.GetComponent<Image>().sprite = inactiveImage;
    } 
}

[System.Serializable]
public class JSONData{
    public List<ObjectsBuilt> objects = new List<ObjectsBuilt>();
}

[System.Serializable]
public class ObjectsBuilt
{
    public ObjectsBuilt(string name, Vector3 pos, Vector3 rot)
    {
        prefabType = name;
        position = new XYZ(pos);
        rotation = new XYZ(rot);
    }

    public string prefabType;
    public XYZ position;
    public XYZ rotation;
}

[System.Serializable]
public class XYZ
{
    public XYZ(Vector3 vect)
    {
        x = vect.x;
        y = vect.y;
        z = vect.z;
    }

    public float x;
    public float y;
    public float z;
}
