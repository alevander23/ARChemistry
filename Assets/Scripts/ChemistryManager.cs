using System.Collections.Generic;
using UnityEngine;


public class ChemistryManager : ChemistryLogic
{
    #region Attributes
    private GameObject containerObject;
    private GameObject solutionObject = null;
    private GameObject placematPlane;
    private EventHandler eventHandler = null;
    private MeshRenderer ContainerRenderer = null;
    private MeshRenderer SolutionRenderer = null;
    private MeshRenderer placematRenderer = null;
    private BoxCollider Trigger = null;
    private bool isColliding = false;
    /*private List<Chemical> ContainerChemicals = null;*/
    private Dictionary<Chemical, float> ContainerChemicals = null;
    private float Volume = 0;
    private bool tipFlag = false;
    private bool containerFlag = false;

    public float Volume1 { get => Volume; set => Volume = value; }
    public Dictionary<Chemical, float> ContainerChemicals1 { get => ContainerChemicals; set => ContainerChemicals = value; }
    public GameObject ContainerObject { get => containerObject; }

    public bool ContainerFlag { get => containerFlag; }
    public MeshRenderer SolutionRenderer1 { set => SolutionRenderer = value; }
    public BoxCollider Trigger1 { get => Trigger; }

    #endregion

    #region MonoBehaviours
    private void Awake()
    {
        eventHandler = GetComponent<EventHandler>();
        ContainerChemicals = new Dictionary<Chemical, float>();

        eventHandler.OnTrackingFound += Show;
        eventHandler.OnTrackingLost += Hide;
        CreateContainer();

    }

    private void OnDestroy()
    {
        eventHandler.OnTrackingFound -= Show;
        eventHandler.OnTrackingLost -= Hide;
    }
    #endregion

    #region Container Creation
    private void CreateContainer()
    {
        /*placematPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        placematPlane.transform.parent = transform;
        placematPlane.transform.position = transform.position;

        placematPlane.transform.eulerAngles = transform.eulerAngles;
        placematPlane.transform.localScale = new Vector3(0.11f, 0.1f, 0.11f);
        placematRenderer = placematPlane.GetComponent<MeshRenderer>();
        placematRenderer.material.color = Color.black;*/


        /*Plane.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        Plane.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", new Color(244, 244, 244) * 0.25f);*/

        if (SpawnQueue == "Beaker")
        {
            CreateBeaker();
            containerFlag = true;
        }
        else if (SpawnQueue == "TestTube")
        {
            CreateTestTube();
            containerFlag = false;
        }

        //Naming 
        containerObject.name = SpawnQueue;
        SpawnQueue = "";

        //Getting Objects and Renderers
        /*solutionObject = containerObject.transform.Find("Solution").gameObject;*/
        ContainerRenderer = containerObject.GetComponent<MeshRenderer>();
        /*SolutionRenderer = solutionObject.GetComponent<MeshRenderer>();*/

        //Parenting it to this instance (placemats position)
        containerObject.transform.parent = this.gameObject.transform;

        //Positioning Container
        PositionContainer();

        //Creating Trigger
        gameObject.AddComponent<BoxCollider>();
        Trigger = gameObject.GetComponent<BoxCollider>();

        
        if (containerObject.name == "Beaker")
        {
            Trigger.center = new Vector3(0f, 0.07f, 0f);
            Trigger.size = new Vector3(0.4f, 0.1f, 0.4f);
            Trigger.isTrigger = true;
        }
        else
        {
            Trigger.center = new Vector3(0f, 0.1f, -0.45f);
            Trigger.size = Vector3.one * 0.1f;
            Trigger.isTrigger = true;
        }
        /*containerObject.AddComponent<Rigidbody>();

        containerObject.GetComponent<Rigidbody>().isKinematic = true;

        containerObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;*/

    }

    private void PositionContainer()
    {
        containerObject.transform.localPosition = new Vector3(0f, 0.54f, 0f);
        containerObject.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
        if (containerObject.name == "Beaker")
        {
            //SEE WHETHER I CAN FIX THIS ISSUE OF 1500f for scale - ITS UGLY 
            containerObject.transform.localScale = new Vector3(900f, 900f, 900f);
            containerObject.transform.localPosition = new Vector3(0f, 0.34f, 0f);
            containerObject.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
            /*//SOLUTION OBJECT WILL BE DIFFERENT FROM NOW ON
            solutionObject = containerObject.transform.Find("Solution").gameObject;
            solutionObject.transform.parent = containerObject.transform;
            solutionObject.transform.localScale = new Vector3(1500f, 1500f, 1500f);
            solutionObject.transform.localPosition = new Vector3(0f, 0f, 0f);
            solutionObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);*/
        }
        else
        {
            containerObject.transform.localScale = new Vector3(700f, 700f, 700f);
            containerObject.transform.localPosition = new Vector3(0f, 0.1f, 0f);
            containerObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            /*//One unit 0.0001 is unit starting is -0.00068 add 0.0001 each unit. radius = 0.00013
            solutionObject = containerObject.transform.Find("Solution").gameObject;
            solutionObject.transform.parent = containerObject.transform;
            solutionObject.transform.localScale = new Vector3(0.00013f, 0f, 0.00013f);
            solutionObject.transform.localPosition = new Vector3(0f, 0f, -0.00068f);
            solutionObject.transform.localEulerAngles = new Vector3(90f, 0f, 0f);*/
        }

    }

    private void CreateBeaker()
    {
        print(this.gameObject.name);
        //Creating clone of BeakerObject
        containerObject = Instantiate(GameObject.Find("BeakerObject"));
        Beakers.Add(containerObject);
    }

    private void CreateTestTube()
    {
        print(this.gameObject.name);
        //Creating clone of TestTubeObject
        containerObject = Instantiate(GameObject.Find("TestTubeObject"));
        TestTubes.Add(containerObject);
    }
    
    #endregion

    #region Trigger Event

    //proBLEM lies here
    private void OnTriggerStay(Collider other)
    {
        //Insures that a single collision will only result in a single execution of method
        if (isColliding == false)
        {
            isColliding = true;
            /*            if (other.tag == "Chemical")
                        {*/
            if (other.tag == "Chemical")
            {
                string[] arr = other.name.Split('#');
                
                for (int i = 0; i < arr.Length; i++)
                {
                    string[] chemical_string = arr[i].Split('&');
                    Chemical chemical = ChemicalManager.Chemicals[0];
                    foreach (Chemical chem in ChemicalManager.Chemicals)
                    {
                        if (chem.ChemicalName == chemical_string[0])
                        {
                            chemical = chem;
                        }
                    }
                    print(chemical_string[1]);
                    addToChemicals(other.gameObject, chemical, float.Parse(chemical_string[1]));
                }

                CreateSolution();
            }

            else
            {
                foreach (Chemical chemical in ChemicalManager.Chemicals)
                {
                    if (chemical.ChemicalName == other.tag)
                    {
                        addToChemicals(other.gameObject, chemical, float.Parse(other.name));
                    }
                }


                CreateSolution();

                /*SolutionColor(ContainerChemicals, gameObject.GetComponent<Solution>().GetComponent<MeshRenderer>());*/
                /*UpdateVolume(containerObject, solutionObject, Volume);*/
                /*            }*/
            }
        }
    }

    private void addToChemicals(GameObject gameobject, Chemical chemical, float volume)
    {
        if (ContainerChemicals.ContainsKey(chemical))
        {
            ContainerChemicals[chemical] += volume;
        }
        else
        {
            ContainerChemicals.Add(chemical, volume);
        }

        /*print(float.Parse(other.name));*/
        Volume += volume;
        /*print(Volume + " Volume ChemManager");*/

        /*    if (SolutionRenderer.enabled == false)
            {
                SolutionRenderer.enabled = true;
            }*/

        Destroy(gameobject);
    }

    private void CreateSolution()
    {
        if (null == solutionObject)
        {
            containerObject.AddComponent<Solution>();
            solutionObject = new GameObject();
        }

        ContainerChemicals = CheckForReaction(ContainerChemicals);
    }
    //COULD TURN THIS INTO A CO ROUTINE TO HELP WITH PERFORMANCE
    private void Update()
    {
        isColliding = false;
        /*if (this.gameObject.transform.rotation.eulerAngles.x < 200f)
        {
            if (tipFlag == false)
            {
                ContainerChemicals = new Dictionary<Chemical, float>();
                Volume = 0;
                UpdateVolume(containerObject, solutionObject, Volume);
                SolutionRenderer.enabled = false;
                tipFlag = true;
            }
        }
        else
        {
            tipFlag = false;
        }*/
    }

    #endregion

    #region Renderer Methods
    private void Show()
    {
        if (containerObject.name == "Beaker")
        {
            Beakers.Add(containerObject);
        }
        else
        {
            TestTubes.Add(containerObject);
        }
        ContainerRenderer.enabled = true;
        /*placematRenderer.enabled = true;*/
        if (null != solutionObject)
        {
            SolutionRenderer.enabled = true;
        }
        Trigger.isTrigger = true;
    }

    private void Hide()
    {
        print("hide");
        if (containerObject.name == "Beaker")
        {
            Beakers.Remove(containerObject);
        }
        else
        {
            TestTubes.Remove(containerObject);
        }
        ContainerRenderer.enabled = false;
        /*placematRenderer.enabled = false;*/
        if (null != solutionObject)
        {
            SolutionRenderer.enabled = false;
        }
        Trigger.isTrigger = false;
    }
    #endregion
}



