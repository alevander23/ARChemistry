using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreateContainer : MonoBehaviour
{
    private EventHandler eventHandler = null;
    private GameObject Container = null;
    private string ContainerType = "";

    private void Awake()
    {
        //Creating Container
        if (MainFlow.SpawnQueue == "Beaker")
        {
            ContainerType = "Beaker";
            CreateBeaker();
        }
        else if (MainFlow.SpawnQueue == "TestTube")
        {
            ContainerType = "TestTube";
            CreateTestTube();
        }

        //Parenting it to this instance (placemats position)
        Container.transform.parent = this.gameObject.transform;

        //Positioning Container
        PositionContainer();

        //Naming 
        Container.name = ContainerType;

    }

    private void PositionContainer()
    {
        Container.transform.localPosition = new Vector3(0f, 0.54f, 0f);
        Container.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
        if (ContainerType == "Beaker")
        {
            Container.transform.localScale = new Vector3(1500f, 1500f, 1500f);
            Container.transform.localPosition = new Vector3(0f, 0.54f, 0f);
            Container.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
            GameObject Solution = Container.transform.Find("Solution").gameObject;
            Solution.transform.parent = Container.transform;
            Solution.transform.localScale = new Vector3(1500f, 1500f, 1500f);
            Solution.transform.localPosition = new Vector3(0f, 0f, 0f);
            Solution.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
        else
        {
            Container.transform.localScale = new Vector3(700f, 700f, 700f);
            Container.transform.localPosition = new Vector3(0f, 0.1f, 0f);
            Container.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            //One unit 0.0001 is unit starting is -0.00068 add 0.0001 each unit. radius = 0.00013
            GameObject Solution = Container.transform.Find("Solution").gameObject;
            Solution.transform.parent = Container.transform;
            Solution.transform.localScale = new Vector3(0.00013f, 0f, 0.00013f);
            Solution.transform.localPosition = new Vector3(0f, 0f, -0.00068f);
            Solution.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
        }
        
    }

    private void CreateBeaker()
    {
        print(this.gameObject.name);
        //Creating clone of BeakerObject
        Container = Instantiate(GameObject.Find("BeakerObject"));
    }

    private void CreateTestTube()
    {
        print(this.gameObject.name);
        //Creating clone of TestTubeObject
        Container = Instantiate(GameObject.Find("TestTubeObject"));
        MainFlow.TestTube = Container;
    }

    private void AssignFunctionality()
    {
        //Assigning functionality to trigger collider
        GameObject Colliders = Container.transform.Find("Colliders").gameObject;
        GameObject Trigger = Colliders.transform.Find("Trigger").gameObject;
        Trigger.AddComponent<ContainerChemistry>();
        ContainerChemistry containerChemistry = Trigger.GetComponent<ContainerChemistry>();
        containerChemistry.PassEventHandler(eventHandler);
        containerChemistry.SetParent(Container);

    }

    public void PassEventHandler(EventHandler _eventHandler)
    {
        eventHandler = _eventHandler;
        //Assigning Functionality to the containers trigger collider
        AssignFunctionality();
    }

}
