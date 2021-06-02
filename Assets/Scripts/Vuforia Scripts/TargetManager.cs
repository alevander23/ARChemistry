using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System.Linq;

public class TargetManager : MonoBehaviour
{
    public string mDatabaseName = "";

    private List<TrackableBehaviour> mAllTargets = new List<TrackableBehaviour>();


    //Load Chemistry
    private ChemicalManager chemManager;

    private ReactionManager reactionManager;

    private void Awake()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        chemManager = new ChemicalManager();
        print(chemManager + "Instance of Chemical Manager");
        reactionManager = new ReactionManager();
    }

    private void OnDestroy()
    {
        VuforiaARController.Instance.UnregisterVuforiaStartedCallback(OnVuforiaStarted);
    }


    private void OnVuforiaStarted()
    {
        //Loads the vuforia database which are downloaded as a package containing all of the target images.
        LoadDatabase(mDatabaseName);

        //Creates a list of all the trackable images and assigns trackablebehaviours to them.
        mAllTargets = GetTargets();

        //Sets up the Attributes and functionality of the targets.
        SetupTargets(mAllTargets);
    }

    private void LoadDatabase(string setName)
    {

        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

        objectTracker.Stop();

        if (DataSet.Exists(setName))
        {
            DataSet dataSet = objectTracker.CreateDataSet();

            dataSet.Load(setName);
            objectTracker.ActivateDataSet(dataSet);
        }
        objectTracker.Start();
    }

    
    private List<TrackableBehaviour> GetTargets()
    {
        List<TrackableBehaviour> allTrackables = new List<TrackableBehaviour>();
        allTrackables = TrackerManager.Instance.GetStateManager().GetTrackableBehaviours().ToList();

        return allTrackables;
    }

    private void SetupTargets(List<TrackableBehaviour> allTargets)
    {
        foreach (TrackableBehaviour target in allTargets)
        {
            //Parent
            target.gameObject.transform.parent = transform;

            //Rename
            target.gameObject.name = target.TrackableName;

            //Add Functionality
            print(target.name + " Target Manager");
            target.gameObject.AddComponent<ObjectCreator>();
        }
    }

    //These are my methods which I have created to attempt and pass chemical manager through to all the other classes
    public ChemicalManager GetChemicalManager()
    {
        return chemManager;
    }

    public ReactionManager GetReactionManager()
    {
        return reactionManager;
    }
}
