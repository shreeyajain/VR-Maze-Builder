using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UndoRedo : MonoBehaviour
{
    private static List<ObjectsInGame> undoList = new List<ObjectsInGame>();

    public Button start;
    public Button end;
    public Sprite inactiveImage;
    public Sprite activeImage;

    public UnityEngine.Object mazePrefab;
    public UnityEngine.Object startPrefab;
    public UnityEngine.Object endPrefab;
    public UnityEngine.Object fencePrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void AddObjToList(string objType, string name, string funcType, Vector3 pos, Quaternion rot)
    {
        if (undoList.Count < 10)
        {
            undoList.Add(new ObjectsInGame(objType, name, funcType, pos, rot));
        }
        else if (undoList.Count == 10)
        {
            undoList.RemoveAt(0);
            undoList.Add(new ObjectsInGame(objType, name, funcType, pos, rot));
        }
    }

    public void OnUndoButtonPress()
    {
        if (undoList.Count > 0)
        {
            ObjectsInGame obj = undoList[undoList.Count - 1];
            undoList.RemoveAt(undoList.Count - 1);

            if (obj.funcType == "build")
            {
                Destroy(GameObject.Find(obj.name));
                if (obj.objType == "start")
                {
                    // Toggle interactable state of the button on and off
                    start.interactable = !start.interactable;
                    // Change the image of the button to BigPink
                    start.GetComponent<Image>().sprite = activeImage;
                }
                else if (obj.objType == "end")
                {
                    // Toggle interactable state of the button on and off
                    end.interactable = !end.interactable;
                    // Change the image of the button to BigPink
                    end.GetComponent<Image>().sprite = activeImage;
                }
            }
            else if (obj.funcType == "delete")
            {
                if (obj.objType == "start")
                {
                    Instantiate(startPrefab, obj.pos, obj.rot);
                    // Toggle interactable state of the button on and off
                    start.interactable = !start.interactable;
                    // Change the image of the button to BigPink
                    start.GetComponent<Image>().sprite = inactiveImage;
                }
                else if (obj.objType == "end")
                {
                    Instantiate(endPrefab, obj.pos, obj.rot);
                    // Toggle interactable state of the button on and off
                    end.interactable = !end.interactable;
                    // Change the image of the button to BigPink
                    end.GetComponent<Image>().sprite = inactiveImage;
                }
                else if (obj.objType == "maze")
                {
                    Instantiate(mazePrefab, obj.pos, obj.rot);
                }
                else if (obj.objType == "fence")
                {
                    Instantiate(fencePrefab, obj.pos, obj.rot);
                }
            }
            else if (obj.funcType == "manipulate")
            {
                GameObject manipulatedObj = GameObject.Find(obj.name);
                manipulatedObj.transform.position = obj.pos;
                manipulatedObj.transform.rotation = obj.rot;
            }
        }
    }
}

public class ObjectsInGame
{
    public ObjectsInGame(string objT, string n, string funcT, Vector3 p, Quaternion r)
    {
        objType = objT;
        name = n;
        funcType = funcT;
        pos = p;
        rot = r;
    }

    public string objType;
    public string name;
    public string funcType;
    public Vector3 pos = new Vector3();
    public Quaternion rot = new Quaternion();
}
