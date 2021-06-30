using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventHandler))]
public class ChemistryManager : ChemistryLogic
{
    #region Attributes
    private GameObject containerObject = null;
    private GameObject solutionObject = null;
    private EventHandler eventHandler = null;
    private MeshRenderer ContainerRenderer = null;
    private MeshRenderer SolutionRenderer = null;
    private CapsuleCollider Trigger = null;
    private bool isColliding = false;
    private List<Chemical> ContainerChemicals = null;
    private int Volume = 0;
    private bool tipFlag = false;

    #endregion

    #region MonoBehaviours
    private void Awake()
    {
        eventHandler = GetComponent<EventHandler>();
        ContainerChemicals = new List<Chemical>();

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
        if (SpawnQueue == "Beaker")
        {
            CreateBeaker();
        }
        else if (SpawnQueue == "TestTube")
        {
            CreateTestTube();
        }

        //Naming 
        containerObject.name = SpawnQueue;
        SpawnQueue = "";

        //Getting Objects and Renderers
        solutionObject = containerObject.transform.Find("Solution").gameObject;
        ContainerRenderer = containerObject.GetComponent<MeshRenderer>();
        SolutionRenderer = solutionObject.GetComponent<MeshRenderer>();
        MeshFilter meshFilter = solutionObject.GetComponent<MeshFilter>();
        if (null != meshFilter)
        {
            print("success");
            Mesh mesh = meshFilter.mesh;
            if (null != mesh)
            {
                print("mesh exists");
                print(mesh.vertices);
            }
        }

        //Parenting it to this instance (placemats position)
        containerObject.transform.parent = this.gameObject.transform;

        //Positioning Container
        PositionContainer();

        //Creating Trigger
        gameObject.AddComponent<CapsuleCollider>();
        Trigger = gameObject.GetComponent<CapsuleCollider>();
        //Direction x = 0, y = 1, z = 2
        Trigger.direction = 2;
        Trigger.height = 0;
        Trigger.radius = 0.06f;
        Trigger.center = new Vector3(0f, 0.1f, -0.45f);
        Trigger.isTrigger = true;
        

    }

    private void PositionContainer()
    {
        containerObject.transform.localPosition = new Vector3(0f, 0.54f, 0f);
        containerObject.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
        if (SpawnQueue == "Beaker")
        {
            containerObject.transform.localScale = new Vector3(1500f, 1500f, 1500f);
            containerObject.transform.localPosition = new Vector3(0f, 0.54f, 0f);
            containerObject.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
            solutionObject = containerObject.transform.Find("Solution").gameObject;
            solutionObject.transform.parent = containerObject.transform;
            solutionObject.transform.localScale = new Vector3(1500f, 1500f, 1500f);
            solutionObject.transform.localPosition = new Vector3(0f, 0f, 0f);
            solutionObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
        else
        {
            containerObject.transform.localScale = new Vector3(700f, 700f, 700f);
            containerObject.transform.localPosition = new Vector3(0f, 0.1f, 0f);
            containerObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            //One unit 0.0001 is unit starting is -0.00068 add 0.0001 each unit. radius = 0.00013
            solutionObject = containerObject.transform.Find("Solution").gameObject;
            solutionObject.transform.parent = containerObject.transform;
            solutionObject.transform.localScale = new Vector3(0.00013f, 0f, 0.00013f);
            solutionObject.transform.localPosition = new Vector3(0f, 0f, -0.00068f);
            solutionObject.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
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

    #region Trigger Events

    private void OnTriggerEnter(Collider other)
    {
        //Insures that a single collision will only result in one execution of script
        if (isColliding == false)
        {
            isColliding = true;
            if (other.tag == "Chemical")
            {
                /*ContainerTrigger.isTrigger = false;*/
                foreach (Chemical chemical in ChemicalManager.Chemicals)
                {
                    if (chemical.ChemicalName == other.name)
                    {
                        ContainerChemicals.Add(chemical);
                        Volume++;
                        if (SolutionRenderer.enabled == false)
                        {
                            SolutionRenderer.enabled = true;
                        }
                        Destroy(other.gameObject);
                        if (null == ContainerChemicals)
                        {
                            print("manger null");
                        }
                    }
                }
                ContainerChemicals = CheckForReaction(ContainerChemicals);
                SolutionColor(ContainerChemicals, SolutionRenderer);
                UpdateVolume(containerObject, solutionObject, Volume);
            }
        }
    }

    private void Update()
    {
        isColliding = false;
        if (this.gameObject.transform.rotation.eulerAngles.x < 200f)
        {
            if (tipFlag == false)
            {
                ContainerChemicals = new List<Chemical>();
                Volume = 0;
                UpdateVolume(containerObject, solutionObject, Volume);
                SolutionRenderer.enabled = false;
                tipFlag = true;
            }
        }
        else
        {
            tipFlag = false;
        }
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
        SolutionRenderer.enabled = true;
        Trigger.enabled = true;
    }

    private void Hide()
    {
        if (containerObject.name == "Beaker")
        {
            Beakers.Remove(containerObject);
        }
        else
        {
            TestTubes.Remove(containerObject);
        }
        ContainerRenderer.enabled = false;
        SolutionRenderer.enabled = false;
        Trigger.enabled = false;
    }
    #endregion
}



