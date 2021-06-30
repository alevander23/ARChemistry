using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventHandler))]
public class TargetFunctionality : MainFlow
{
    private EventHandler eventHandler = null;

 /*   protected EventHandler InstanceEventHandler { get => eventHandler; }
    protected float Instance_identifier { get => instance_identifier; }

    private int instance_identifier = 0;*/
/*
    private readonly System.Random rndom = new System.Random();*/

    private void Awake()
    { 
       /* instance_identifier = rndom.Next();*/
        print(this.gameObject + " Target Func");
        eventHandler = GetComponent<EventHandler>();

        if (this.gameObject.name == "Beaker")
        {
            eventHandler.OnTrackingFound += QueueBeaker;
        }
        else if (this.gameObject.name == "Test_Tube")
        {
            eventHandler.OnTrackingFound += QueueTestTube;
        }
        else if (this.gameObject.name.Contains("PlaceMat"))
        {
            eventHandler.OnTrackingFound += SpawnContainer;
        }
        else
        {
            //Assigning "Chemical" tag to the object
            this.gameObject.tag = "Chemical";

            int index = System.Convert.ToInt32(this.gameObject.name);

            //Renaming object
            this.gameObject.name = ChemicalManager.GetNameOfChemicalIndex(index);

            //OnTrackingFound Spawn Pill
            eventHandler.OnTrackingFound += SpawnPill;
        }
    }

    private void OnDestroy()
    {
        if (this.gameObject.name == "Beaker")
        {
            eventHandler.OnTrackingFound -= QueueBeaker;
        }
        else if (this.gameObject.name == "TestTube")
        {
            eventHandler.OnTrackingFound -= QueueTestTube;
        }
        else if (this.gameObject.name.Contains("PlaceMat") && SpawnQueue != "")
        {
            eventHandler.OnTrackingFound -= SpawnContainer;
        }
        else
        {
            eventHandler.OnTrackingFound -= SpawnPill;
        }
    }

    private void QueueBeaker()
    {
        SpawnQueue = "Beaker";
    }

    private void QueueTestTube()
    {
        SpawnQueue = "TestTube";
    }
    
    private void SpawnContainer()
    {

        if (SpawnQueue == "Beaker" || SpawnQueue == "TestTube")
        {
            this.gameObject.AddComponent<ChemistryManager>();
            eventHandler.OnTrackingFound -= SpawnContainer;
        }
        else
        {
            print("Error: Queue Empty");
        }
    }

    private void SpawnPill()
    {
        print("Spawn pill " + this.gameObject.name);

        if (TestTubes.Count > 0 || Beakers.Count > 0)
        {
            GameObject pill = GameObject.CreatePrimitive(PrimitiveType.Capsule);

            if (TestTubes.Count > 0)
            {
                pill.transform.parent = TestTubes[0].transform;
            }
            else if (Beakers.Count > 0)
            {
                pill.transform.parent = Beakers[0].transform;
            }

            pill.transform.localPosition = new Vector3(0f, 0f, 0f);

            pill.transform.localScale = new Vector3(0.0001f, 0.00005f, 0.0001f);

            pill.transform.localEulerAngles = new Vector3(0f, 0f, 0f);

            pill.GetComponent<MeshRenderer>().enabled = false;

            pill.AddComponent<Rigidbody>();

            pill.AddComponent<CapsuleCollider>();

            pill.GetComponent<CapsuleCollider>();

            pill.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            pill.name = this.gameObject.name;

            pill.tag = "Chemical";

            
        }
        else
        {
            print("No Container Avaliable");
        }
    }
}
