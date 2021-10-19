using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MethodLibrary
{
    public static GameObject[] CreateGameObject(int arraySize = 1, float scale = 1f, PrimitiveType primitiveType = PrimitiveType.Cube, bool typeFlag = true, Transform objectParent = null, string name = "")
    {
        GameObject[] objectArray = new GameObject[arraySize];
        for (int i = 0; i < arraySize; i++)
        {
            if (typeFlag == false)
            {
                objectArray[i] = new GameObject();
            }
            else
            {
                objectArray[i] = GameObject.CreatePrimitive(primitiveType);
            }
            if (null != objectParent)
            {
                objectArray[i].transform.parent = objectParent;
            }
            if (arraySize < 2)
            {
                objectArray[i].name = name;
            }
            else
            {
                objectArray[i].name = name + " " + (i + 1);
            }
            objectArray[i].transform.localScale = new Vector3(scale, scale, scale);

        }

        return objectArray;
    }

    public static int FindObjectOfLowestHeight(GameObject[] objects, int start_index)
    {
        GameObject[] object_list = objects;

        float lowest = object_list[start_index].transform.position.y;

        int index = start_index;

        for (int i = start_index; i < object_list.Length; i++)
        {
            if (object_list[i].transform.position.y < lowest)
            {
                lowest = object_list[i].transform.position.y;
                index = i;
            }
        }

        return index;

    }

    public static void parentObjects(GameObject[] children, GameObject parent)
    {
        foreach (GameObject child in children)
        {
            child.transform.parent = parent.transform;
        }
    }

    public static Vector3[] transformPointsToWorld(Vector3[] points, Transform parent)
    {
        Vector3[] returnVectors = new Vector3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            returnVectors[i] = parent.TransformPoint(points[i]);
        }

        return returnVectors;
    }

    public static void positionObjects(GameObject[] objects, Vector3[] locations)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].transform.position = locations[i];
        }
    }



}
