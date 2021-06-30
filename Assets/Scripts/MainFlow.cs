using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFlow : MonoBehaviour
{
    #region Static Attributes
    //These static variables allow me to have certain attributes which are easily accessible.
    private static string spawnQueue = "TestTube";

    private static List<GameObject> testTubes;

    private static List<GameObject> beakers;

    private static ChemicalManager chemicalManager;

    private static ReactionManager reactionManager;
    #endregion

    #region Static Getter/Setter Methods
    /*  private static Dictionary<GameObject, EventHandler> instanceEventHandlers = new Dictionary<GameObject, EventHandler>();
  */
    protected static string SpawnQueue { get => spawnQueue; set => spawnQueue = value; }
    protected static List<GameObject> TestTubes { get => testTubes; set => testTubes = value; }
    protected static List<GameObject> Beakers { get => beakers; set => beakers = value; }
    protected static ChemicalManager ChemicalManager { get => chemicalManager; }
    protected static ReactionManager ReactionManager { get => reactionManager; }
    /*protected static Dictionary<GameObject, EventHandler> InstanceEventHandlers { get => instanceEventHandlers; set => instanceEventHandlers = value; }
*/
    #endregion

    #region MonoBehaviours
    private void Awake()
    {
        testTubes = new List<GameObject>();
        beakers = new List<GameObject>();
        chemicalManager = new ChemicalManager();
        reactionManager = new ReactionManager(ChemicalManager.Chemicals);
        gameObject.AddComponent<TargetManager>();
    }
    #endregion
}
