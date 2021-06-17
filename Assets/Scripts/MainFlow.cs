using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFlow : MonoBehaviour
{
    //These static variables allow me to have certain attributes which are easily accessible.
    private string spawnQueue = "";

    private GameObject maintestTube = null;

    private ChemicalManager chemicalManager = null;

    private ReactionManager reactionManager = null;

    private List<Chemical> chemicalList = null;

    private List<Reaction> reactionList = null;

    private GameObject chemicals = null; 

    public string SpawnQueue { get => spawnQueue; set => spawnQueue = value; }
    public List<Chemical> ChemicalList { get => chemicalList; }
    public List<Reaction> ReactionList { get => reactionList; }
    public GameObject Chemicals { get => chemicals; }
    public ChemicalManager ChemicalManager { get => chemicalManager; }
    public GameObject MainTestTube { get => maintestTube; set => maintestTube = value; }

    public MainFlow()
    {
        print("is called");
        chemicals = new GameObject();
        chemicals.transform.parent = transform;
        chemicalManager = new ChemicalManager();
        chemicalList = chemicalManager.Chemicals;
        reactionManager = new ReactionManager(chemicalList);
        reactionList = reactionManager.Reactions;


        foreach (Reaction reaction in reactionList)
        {
            print(reaction.ReactantA.ChemicalName + " + " + reaction.ReactantB.ChemicalName + " = " + reaction.Product.ChemicalName);
        }
    }
    public void Main()
    {
        //Load targets and add Functionality
        gameObject.AddComponent<TargetManager>();

    }

    public MainFlow MainInstance { get => this; }


}
