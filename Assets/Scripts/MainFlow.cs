using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFlow : MonoBehaviour
{
    private static string spawnQueue = "";

    private static GameObject testTube = null;

    private static ChemicalManager chemicalManager;

    private static ReactionManager reactionManager;

    private static List<Chemical> chemicalList;

    private static List<Reaction> reactionList;

    private static GameObject chemicals = null; 
    public static string SpawnQueue { get => spawnQueue; set => spawnQueue = value; }
    public static List<Chemical> ChemicalList { get => chemicalList; }
    public static List<Reaction> ReactionList { get => reactionList; }
    public static GameObject Chemicals { get => chemicals; }
    public static ChemicalManager ChemicalManager { get => chemicalManager; }
    public static GameObject TestTube { get => testTube; set => testTube = value; }

    public void Main()
    {
        chemicals = new GameObject();
        chemicals.transform.parent = transform;
        chemicalManager = new ChemicalManager();
        chemicalList = chemicalManager.Chemicals;
        reactionManager = new ReactionManager();
        reactionList = reactionManager.Reactions;

        //Load targets and add Functionality
        this.gameObject.AddComponent<TargetManager>();
    }


}
