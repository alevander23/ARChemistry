using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Insures this script always has an EventHandler (I tried this to fix the null reference issue but it didn't fix it)
[RequireComponent(typeof(EventHandler))]
public class ObjectCreator : MonoBehaviour
{
    
    private EventHandler eventHandler = null;

    private TargetManager targetManager;

    private ChemicalManager chemManager;

    private MeshRenderer[] renderers = new MeshRenderer[3];


    //Initial method which is run before anything else
    private void Awake()
    {
        //Sets up all of my game objects
        Setup();

        //References the target manager so that I can 
        //reference the chemical manager
        targetManager = GameObject.FindObjectOfType<TargetManager>();
        //Referencing the chemical manager
        chemManager = targetManager.GetChemicalManager();
        //Checking to see whether this is even working
        print(chemManager + "Instance of Chem Manager");
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
            GameObject Beaker = GameObject.Find("Beaker");
            Beaker.transform.parent = transform;
            MeshRenderer BeakerRenderer = Beaker.GetComponent<MeshRenderer>();
            GameObject Volume = Beaker.transform.GetChild(0).gameObject;
            Volume.transform.parent = transform;
            Beaker.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            MeshRenderer VolumeRenderer = Volume.GetComponent<MeshRenderer>();
            VolumeRenderer.material.SetColor("_Color", Color.clear);
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
        if (this.gameObject.tag == "Chemical")
        { 
            if (GameObject.Find("Beaker").GetComponent<MeshRenderer>().enabled == true)
            {
                //Spawns a capsule which does not have a renderer but does have a collider and rigidbody.
                //spawns it inside the beaker as of right now.
                print("Renderer is Enabled so I will continue");
                GameObject pill = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                pill.name = chemicalNames[System.Convert.ToInt32(pill.name)];
                pill.transform.parent = transform;
                pill.transform.localPosition = new Vector3(0, 0, -0.00025f);
                pill.AddComponent<Rigidbody>();
                pill.AddComponent<CapsuleCollider>();
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
        }
    }

    private void Hide()
    {
        if(this.gameObject.name == "Beaker")
        {
            renderers[0].enabled = false;
            renderers[1].enabled = false;
        }
    }
}
