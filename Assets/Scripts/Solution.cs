﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Solution : ChemistryLogic
{
    #region Attributes
    //Mesh Attributes
    private Vector3[] vertices;
    private int[] triangles;
    private MeshFilter mf;
    private MeshRenderer mr;

    //Dynamic Attributes
    private float height;
    private float heightPH;
    private float critHeight;
    private float controlHeight;
    private float critRatio;
    private float controlRatio;

    //GameObjects
    private GameObject SolutionObject;
    private GameObject LevelingPlane;
    private GameObject LevelingPositioner;
    private GameObject LowestPlane;
    private GameObject[] verticeObjects;
    private GameObject[] verticeObjects2;

    //Flags
    private bool critFlag;
    private bool deleteFlag;

    //ChemistryManager
    private ChemistryManager chemManager;

    //Container Chemicals
    Dictionary<Chemical, float> ContainerChemicals;

    #endregion

    #region MonoBehaviours
    // Start is called before the first frame update
    private void Start()
    {

        GameObject parent = gameObject.transform.parent.gameObject;

        chemManager = parent.GetComponent<ChemistryManager>();

        ContainerChemicals = chemManager.ContainerChemicals1;

        print(gameObject.name);

        critFlag = false;
        deleteFlag = false;

        SolutionObject = MethodLibrary.CreateGameObject(1, 0.01f, default, false, gameObject.transform, "Solution")[0];
        LevelingPlane = MethodLibrary.CreateGameObject(1, 2, PrimitiveType.Plane, true, SolutionObject.transform, "Leveling Plane")[0];
        LevelingPositioner = MethodLibrary.CreateGameObject(1, 1, default, false, SolutionObject.transform, "Leveling Positioner ")[0];
        LowestPlane = MethodLibrary.CreateGameObject(1, 2, PrimitiveType.Plane, true, gameObject.transform, "Lowest Plane")[0];
        verticeObjects = MethodLibrary.CreateGameObject(16, 0.1f, default, true, SolutionObject.transform, "vertice ");
        /*verticeObjects2 = MethodLibrary.CreateGameObject(16, 0.1f, default, true, SolutionObject.transform, "second vertice");*/

        SolutionObject.AddComponent<MeshFilter>();
        mf = SolutionObject.GetComponent<MeshFilter>();

        SolutionObject.AddComponent<MeshRenderer>();
        mr = SolutionObject.GetComponent<MeshRenderer>();

        chemManager.SolutionRenderer1 = mr;

        LevelingPlane.GetComponent<MeshRenderer>().enabled = false;
        LowestPlane.GetComponent<MeshRenderer>().enabled = false;

        LevelingPositioner.transform.localPosition = new Vector3(0.5f, heightPH, 1.2071f);
        LevelingPlane.transform.localPosition = LevelingPositioner.transform.position;

        SolutionObject.transform.parent = gameObject.transform;

        if (SpawnQueue == "Beaker")
        {
            SolutionObject.transform.localScale = new Vector3(1500f, 1500f, 1500f);
            SolutionObject.transform.localPosition = new Vector3(0f, 0f, 0f);
            SolutionObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
        else
        {
            //One unit 0.0001 is unit starting is -0.00068 add 0.0001 each unit. radius = 0.00013
            SolutionObject.transform.localScale = new Vector3(0.000062f, 0.000062f, 0.000062f);
            SolutionObject.transform.localPosition = new Vector3(-0.00003f, 0.000075f, -0.00068f);
            SolutionObject.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
        }

        CreateMeshData();
        CreateMesh();

        for (int i = 0; i < 16; i++)
        {
            verticeObjects[i].transform.localPosition = mf.mesh.vertices[i];
            verticeObjects[i].GetComponent<SphereCollider>().enabled = false;
            verticeObjects[i].GetComponent<MeshRenderer>().enabled = false;
        }

        /*for (int i = 0; i < 16; i++)
        {
            verticeObjects2[i].transform.localPosition = mf.mesh.vertices[i];
            verticeObjects2[i].GetComponent<SphereCollider>().enabled = false;
        }*/

        print(getHeight());
        StartCoroutine(MyEvent());
        /*StartCoroutine(MyEvent2());*/
    }


    #endregion

    #region Custom Methods
    private void CreateMeshData()
    {
        vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(1,0,0),
            new Vector3(1+Mathf.Sqrt(0.5f),0,Mathf.Sqrt(0.5f)),
            new Vector3(1+Mathf.Sqrt(0.5f),0,1+Mathf.Sqrt(0.5f)),
            new Vector3(1,0,1+2*(Mathf.Sqrt(0.5f))),
            new Vector3(0,0,1+2*(Mathf.Sqrt(0.5f))),
            new Vector3(-1*Mathf.Sqrt(0.5f),0,1+Mathf.Sqrt(0.5f)),
            new Vector3(-1*Mathf.Sqrt(0.5f),0,Mathf.Sqrt(0.5f)),
            new Vector3(0,heightPH,0),
            new Vector3(1,heightPH,0),
            new Vector3(1+Mathf.Sqrt(0.5f),heightPH,Mathf.Sqrt(0.5f)),
            new Vector3(1+Mathf.Sqrt(0.5f),heightPH,1+Mathf.Sqrt(0.5f)),
            new Vector3(1,heightPH,1+2*(Mathf.Sqrt(0.5f))),
            new Vector3(0,heightPH,1+2*(Mathf.Sqrt(0.5f))),
            new Vector3(-1*Mathf.Sqrt(0.5f),heightPH,1+Mathf.Sqrt(0.5f)),
            new Vector3(-1*Mathf.Sqrt(0.5f),heightPH,Mathf.Sqrt(0.5f))
        };

        triangles = new int[]
        {
            0,1,2,
            2,7,0,
            7,2,3,
            3,6,7,
            6,3,4,
            4,5,6,

            0,8,1,
            8,9,1,
            1,9,2,
            9,10,2,
            2,10,3,
            10,11,3,
            3,11,4,
            11,12,4,
            4,12,5,
            12,13,5,
            5,13,6,
            13,14,6,
            6,14,7,
            14,15,7,
            7,15,0,
            15,8,0,

            10,9,8,
            8,15,10,
            11,10,15,
            15,14,11,
            12,11,14,
            14,13,12
        };
    }

    //Updates mesh properties using my mesh data.
    private void CreateMesh()
    {
        mf.mesh.vertices = vertices;
        mf.mesh.triangles = triangles;
        mf.mesh.RecalculateNormals();
    }

    private void AdjustLowestPlane()
    {
        int index = MethodLibrary.FindObjectOfLowestHeight(verticeObjects, 0);

        LowestPlane.transform.position = verticeObjects[index].transform.position;
    }

    private void findCritHeight()
    {
        float opposite = 1f + (2f * Mathf.Sqrt(0.5f));
        float theta = Mathf.Atan(opposite / (2 * heightPH));
        critHeight = heightPH * Mathf.Sin(theta);

        critRatio = critHeight / heightPH;

    }

    private void AdjustTopVertices()
    {
        for (int i = 0; i < 8; i++)
        {
            RaycastHit hitInfo;

            Vector3 startPos = verticeObjects[i].transform.position;

            //Dest - Origin 
            Vector3 heading = verticeObjects[i + 8].transform.position - startPos;

            Vector3 direction = heading.normalized;

            float distance = 0;

            if (Physics.Raycast(startPos, direction, out hitInfo, 100f))
            {
                if (hitInfo.collider)
                {
                    Vector3 localPoint = SolutionObject.transform.InverseTransformPoint(hitInfo.point);
                    /*print(distance);
                    print(controlHeight);
                    print(test);*/

                    /*print(heightPH);
                    

                    float new_distance = distance / controlHeight;

                    print(new_distance);
*/
                    vertices[i + 8] = localPoint;
                    /*verticeObjects2[i + 8].transform.localPosition = verticeObjects2[i+8].transform.InverseTransformPoint(hitInfo.point);*/
                }
            }
        
            /*print(distance);

            vertices[i + 8] = new Vector3(vertices[i + 8].x, distance / controlHeight, vertices[i + 8].z);
            verticeObjects2[i + 8].transform.localPosition = new Vector3(vertices[i + 8].x, distance / controlHeight, vertices[i + 8].z);*/
        }

        CreateMesh();
    }

    private void findCritFlag()
    {
        RaycastHit hitInfo;

        int lowestIndex = MethodLibrary.FindObjectOfLowestHeight(verticeObjects, 8);

        /*print(lowestIndex);*/

        if (Physics.Raycast(verticeObjects[lowestIndex].transform.position, Vector3.down, out hitInfo, 100f))
        {
            if (hitInfo.collider)
            {
                /*print(hitInfo.collider.name);*/
                if (hitInfo.distance * controlRatio < critHeight)
                {
                    /*print(hitInfo.distance / controlHeight);
                    print(critHeight);*/
                    /*print("crit");*/
                    //CHANGE THIS BACK TO TRUE WHEN DONE DEBUGGING
                    critFlag = true;
                }
                else
                {
                    /*print(controlHeight);
                    print(hitInfo.distance / controlHeight);
                    print(critHeight);*/
                    /*print("fine");*/
                    critFlag = false;
                }
            }
        }
    }

    private float getHeight()
    {
        //If I know the volume when height is 1. Then I can figure out the height of any given volume.

        //Beaker
        //Volume of Beaker when height is 1 == TBD
        if (chemManager.ContainerFlag)
        {
            return 1f;
        }

        //TestTube
        //Volume of Test Tube when height is 1 == 4.828428
        else
        {
            if (chemManager.Volume1 / 4.828428f - heightPH > 0.00001f)
            {
                print(chemManager.Volume1 / 4.828428f - heightPH);
                return (chemManager.Volume1 / 4.828428f);
            }
            else
            {
                return heightPH;
            }
        }
    }

    private void findControlHeight()
    {

        /*Vector3 original_rotation = SolutionObject.transform.eulerAngles;

        SolutionObject.transform.eulerAngles = Vector3.zero;*/

        float distance = Vector3.Distance(verticeObjects[0].transform.position, verticeObjects[8].transform.position);

        /*SolutionObject.transform.eulerAngles = original_rotation;*/

/*        print(distance);*/

        controlHeight = distance;

        controlRatio = heightPH / controlHeight;
    }


    protected void SolutionColor()
    {
        List<Color> colors = new List<Color>();
        mr.material = Resources.Load<Material>("TransparentMaterial");
        mr.material.EnableKeyword("_EMISSION");

        //Effective Volume for mixing Oppacity
        float Volume = chemManager.Volume1;

        //Effective Volume for mixing Color
        float ColorVolume = 0;

        //Volume of Opaque contents
        float OpaqueVolume = 0;

        //Volume of Transparent contents
        float TransparentVolume = 0;

        float red = 0f;
        float green = 0f;
        float blue = 0f;
        float alpha = 0f;

        
        foreach (Chemical chemical in ContainerChemicals.Keys)
        {
/*            print(chemical.ChemicalName + " container chems");*/
            colors.Add(chemical.ChemicalColor);
            if (chemical.ChemicalColor.a == 1.0f)
            {
                OpaqueVolume += ContainerChemicals[chemical];
            }
            else
            {
                TransparentVolume += ContainerChemicals[chemical];
            }
        }

        foreach (Color color in colors)
        {
            if (color != Color.clear)
            {
                ColorVolume++;
            }
            red += color.r;
            green += color.g;
            blue += color.b;
            alpha += color.a;
        }

        Color solutionColor = new Color(red, blue, green, alpha);

        if (solutionColor != Color.clear)
        {
    /*        print("entering color");*/
            red /= ColorVolume;
            green /= ColorVolume;
            blue /= ColorVolume;
            alpha /= Volume;
            if (alpha <= 0.01)
            {
                //Transparent Colored Solutions
                solutionColor = new Color(red, green, blue, alpha);
                mr.material.SetColor("_EmissionColor", solutionColor);
            }
            else
            {
                if (OpaqueVolume/Volume < 0.5)
                {
                    alpha = OpaqueVolume / Volume;
                }
                else
                {
                    alpha = 1 - (0.75f * TransparentVolume / Volume);
                }
                //Opaque Solutions
                solutionColor = new Color(red, green, blue, alpha);
                mr.material.SetColor("_EmissionColor", solutionColor * 0.25f);
            }
        }

        else
        {
            print("trans colorless");
            //Transparent Colorless Solutions
            solutionColor = new Color(1f, 1f, 1f, 0.01f);
            mr.material.SetColor("_EmissionColor", new Vector4(1.0f, 1.0f, 1.0f, 0f) * 0.25f);
        }

        mr.material.renderQueue = 2450;
        mr.material.color = solutionColor;

    }

    private IEnumerator MyEvent()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            height = getHeight();

            for (int i = 0; i < 8; i++)
            {
                verticeObjects[i].transform.localPosition = new Vector3(mf.mesh.vertices[i].x, 0, mf.mesh.vertices[i].z);
            }
            for (int i = 8; i < 16; i++)
            {
                verticeObjects[i].transform.localPosition = new Vector3(mf.mesh.vertices[i-8].x, heightPH, mf.mesh.vertices[i-8].z);
            }

            if (heightPH != height)
            {
                CreateMeshData();
                CreateMesh();
                SolutionColor();
            }

            heightPH = height;

            findControlHeight();

            /* if (deleteFlag)
             {
                 break;
             }*/
            /*
                        if (heightPH != height)
                        {*/
            /*print("get");*/

            LevelingPositioner.transform.localPosition = new Vector3(0.5f, heightPH, 1.2071f);
            LevelingPlane.transform.position = LevelingPositioner.transform.position;


            LevelingPlane.transform.eulerAngles = new Vector3(180f, 0f, 0f);
            LowestPlane.transform.eulerAngles = Vector3.zero;


            /*CreateMeshData();
            CreateMesh();*/

 /*           for (int i = 0; i < 16; i++)
            {
                verticeObjects[i].transform.localPosition = mf.mesh.vertices[i];
            }
*/
            


            AdjustLowestPlane();

            findCritHeight();
            findControlHeight();

            /*print(controlHeight);*/

            findCritFlag();

            if (critFlag == false)
            {
                /*mr.material.color = Color.red;*/
                /*print("hey1");*/
                AdjustTopVertices();

            }

           
        }

    }

    private IEnumerator MyEvent2()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.35f);

            height = getHeight();

            if (critFlag)
            {
                if (height >= 0.15)
                {
                    if (mr.enabled == false)
                    {
                        mr.enabled = true;
                    }
                    height -= 0.15f;

                    heightPH = height;

                    findCritHeight();

                    CreateMeshData();
                    CreateMesh();

                    for (int i = 0; i < 16; i++)
                    {
                        verticeObjects[i].transform.localPosition = mf.mesh.vertices[i];
                    }

                    LevelingPositioner.transform.localPosition = new Vector3(0.5f, heightPH, 1.2071f);
                    LevelingPlane.transform.position = LevelingPositioner.transform.position;

                }
                else
                {
                    mr.enabled = false;

                    deleteFlag = true;

                    break;
                }
            }
        }
    }

   
    private float FindVolume()
    {
        float volume = 0f;
        for (int i = 0; i < mf.mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = mf.mesh.vertices[triangles[i + 0]];
            Vector3 p2 = mf.mesh.vertices[triangles[i + 1]];
            Vector3 p3 = mf.mesh.vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume);

    }

    float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    #endregion

}
