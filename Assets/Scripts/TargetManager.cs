using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System.Linq;

public class TargetManager : MainFlow
{
    public string mDatabaseName = "Beaker";

    private List<TrackableBehaviour> mAllTargets = new List<TrackableBehaviour>();

    private void Awake()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
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
            target.gameObject.transform.parent = transform;

            //Rename
            target.gameObject.name = target.TrackableName;

            //Add Functionality
            target.gameObject.AddComponent<TargetFunctionality>();
            
        }
    }

}
