using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventHandler))]
public class TargetFunctionality : MainFlow
{
    private EventHandler eventHandler = null;

    private void Awake()
    {
        eventHandler = GetComponent<EventHandler>();

        Setup();
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

    private void Setup()
    {
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
           /* //Assigning "Chemical" tag to the object
            this.gameObject.tag = "Chemical";*/

            int index = System.Convert.ToInt32(this.gameObject.name);

            //Renaming object
            this.gameObject.name = ChemicalManager.GetNameOfChemicalIndex(index);

            //OnTrackingFound Spawn Pill
            eventHandler.OnTrackingFound += SpawnPill;
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
        print(SpawnQueue);

        if (SpawnQueue == "Beaker" || SpawnQueue == "TestTube")
        {
            this.gameObject.AddComponent<CreateContainer>();
            eventHandler.OnTrackingFound -= SpawnContainer;
            this.gameObject.GetComponent<CreateContainer>().PassEventHandler(eventHandler);
            SpawnQueue = "";
        }
        else
        {
            print("Error: Queue Empty");
        }
    }

    private void SpawnPill()
    {
        print("Spawn pill " + this.gameObject.name);
        GameObject TestTube = MainTestTube;
        if (null != TestTube)
        {
            if (TestTube.GetComponent<MeshRenderer>().enabled)
            {
                GameObject pill = GameObject.CreatePrimitive(PrimitiveType.Capsule);

                pill.transform.parent = TestTube.transform;

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
        }
    }
}
