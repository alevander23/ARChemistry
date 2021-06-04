using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Insures this script always has an EventHandler (I tried this to fix the null reference issue but it didn't fix it)
[RequireComponent(typeof(EventHandler))]
public class ObjectCreator : MonoBehaviour
{
    
    private EventHandler eventHandler = null;

    private ChemicalManager chemManager;

    private MeshRenderer[] renderers = new MeshRenderer[3];


    //Initial method which is run before anything else
    private void Awake()
    {
        //Referencing the chemical manager
        chemManager = GameObject.Find("TargetManager").GetComponent<ChemicalManager>();
        //Sets up all of my game objects
        Setup();

        //Checking to see whether this is even working
        //Gets the EventHandler
        eventHandler = GetComponent<EventHandler>();
        //Assigns my methods to the OnTrackingFound/Lost methods 
        eventHandler.OnTrackingFound += Show;
        eventHandler.OnTrackingLost += Hide;
        eventHandler.OnTrackingFound += SpawnPill;
    }

    private void OnDestroy()
    {
        //Removes my methods from the OnTrackingFound/Lost methods
        eventHandler.OnTrackingFound -= Show;
        eventHandler.OnTrackingLost -= Hide;
        eventHandler.OnTrackingFound -= SpawnPill;
    }

    private void Setup()
    {
        //This list is basically just a list of numbers corresponding to list indices of chemicals in the chemicals list
        List<string> chemicalIndexList = chemManager.chemicalIndexList;

        //Creates the beaker
        if (this.gameObject.name == "Beaker")
        {
            GameObject _Beaker = GameObject.Find("BeakerObject");
            _Beaker.transform.parent = transform;
            MeshRenderer BeakerRenderer = _Beaker.GetComponent<MeshRenderer>();
            GameObject Volume = GameObject.Find("Volume").gameObject;
            Volume.transform.parent = transform;
            /*Beaker.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);*/

            Color colorless = new Color(0f, 0f, 0f, 0f);
            MeshRenderer VolumeRenderer = Volume.GetComponent<MeshRenderer>();
            VolumeRenderer.material = Resources.Load<Material>("Chemical");
            VolumeRenderer.material.color = colorless;
            //Assigns these two renderers to the MeshRenderer array to be used in the hide and show methods
            renderers[0] = BeakerRenderer;
            renderers[1] = VolumeRenderer;

            Hide();
        }
        //Will create the test tube when I get around to this part of development
        else if (this.gameObject.name == "Test_Tube")
        {

        }

        //Is supposed to tag the chemical gameObjects with the tag "Chemical" but this is not working 
        else if (chemicalIndexList.Contains(this.gameObject.name))
        {
            this.gameObject.tag = "Chemical";
        }

    }

    //This is only called OnTrackingFound() and is only followed through if the gameObject
    //has the "Chemical" tag
    private void SpawnPill()
    {
        //Only spawns when eventhandler finds a target and when target is a chemical
        List<string> chemicalNames = chemManager.ChemicalNames;
        //Uses tag created in Setup()
        if (this.gameObject.name == "Beaker" || this.gameObject.name == "Test_Tube")
        {

        }
        else
        {
            GameObject Beaker = GameObject.Find("BeakerObject");
            if (Beaker.GetComponent<MeshRenderer>().enabled == true)
            {
                //Spawns a capsule which does not have a renderer but does have a collider and rigidbody.
                //spawns it inside the beaker as of right now.
                print("Renderer is Enabled so I will continue");
                GameObject pill = GameObject.CreatePrimitive(PrimitiveType.Capsule);

                MeshRenderer pillRenderer = pill.GetComponent<MeshRenderer>();
                pillRenderer.material.color = Color.red;
                renderers[2] = pillRenderer;
                int index = Convert.ToInt32(this.gameObject.name);
                pill.name = chemicalNames[index];
                pill.transform.parent = transform;
                pill.transform.localPosition = new Vector3(0f, 0f, 0.00025f);
                /*pill.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);*/
                /*pill.AddComponent<Rigidbody>();*/
                pill.AddComponent<CapsuleCollider>();
                print("pill successfully made");
            }
            else
            {
                //I don't want anything funky going on while the beaker isnt renderered in. 
                print("Renderer is disabled therefore I will not spawn chemical");
            }

        }
    }
    private void Show()
    {
        if(this.gameObject.name == "Beaker")
        {
            renderers[0].enabled = true;
            renderers[1].enabled = true;
            if (renderers[2] != null)
            {
                renderers[2].enabled = true;
            }
        }
    }

    private void Hide()
    {
        if(this.gameObject.name == "Beaker")
        {
            renderers[0].enabled = false;
            renderers[1].enabled = false;
            if (renderers[2] != null)
            {
                renderers[2].enabled = false;
            }
        }
    }
}
