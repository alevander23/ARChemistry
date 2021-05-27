using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventHandler))]
public class ObjectCreator : MonoBehaviour
{
    private EventHandler mEventHandler = null;
    private List<MeshRenderer> Renderers;
    


    private void Awake()
    {
        Setup();

        mEventHandler = GetComponent<EventHandler>();
        mEventHandler.OnTrackingFound += Show;
        mEventHandler.OnTrackingLost += Hide;
    }

    private void OnDestroy()
    {
        mEventHandler.OnTrackingFound -= Show;
        mEventHandler.OnTrackingLost -= Hide;
    }

    private void Setup()
    {
        if(this.name == "Beaker")
        {
            /*print("hey 2");
            *//*GameObject Beaker = new GameObject();
            Mesh BeakerMesh = Resources.Load("Beaker Model") as Mesh;*/
            /*
                        Beaker.GetComponent<MeshFilter>().mesh = BeakerMesh;*//*
            GameObject Beaker = GameObject.Instantiate(Resources.Load("Prefabs/Beaker Model")) as GameObject;
            print("hey 3");*/

            GameObject Beaker = Resources.Load<GameObject>("Beaker Model");

            Mesh BeakerMesh = Beaker.GetComponent<MeshFilter>().sharedMesh;
            Beaker.transform.parent = transform;
            Beaker.transform.position = Vector3.zero;
            
            MeshRenderer BeakerRenderer = Beaker.GetComponent<MeshRenderer>();

            Renderers.Add(BeakerRenderer);

            Hide();
        }
        if(this.name == "Chemical")
        {
            
        }
        if(this.name == "AR_marker")
        {

        }
    }

    private void Show()
    {
        if(this.name == "Beaker")
        {
            foreach(MeshRenderer Renderer in Renderers)
            {
                if (Renderer.name == "BeakerRenderer")
                {
                    print("Enabling Beaker Renderer");
                    Renderer.enabled = true;
                }
            }
        }
       
    }

    private void Hide()
    { 
        if(this.name == "Beaker")
        {
            foreach (MeshRenderer Renderer in Renderers)
            {
                if (Renderer.name == "BeakerRenderer")
                {
                    Renderer.enabled = false;
                }
            }
        }
            
    }

    
}
