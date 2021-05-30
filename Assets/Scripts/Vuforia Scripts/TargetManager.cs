using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System.Linq;

public class TargetManager : MonoBehaviour
{
    public string mDatabaseName = "";

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
        //Load database
        LoadDatabase(mDatabaseName);

        //Get trackable targets
        mAllTargets = GetTargets();

        //Set up targets
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
            if (target.gameObject.name == "Beaker")
            {
                print(target.gameObject.name + " Target Manager");
                target.gameObject.AddComponent<ObjectCreator>();
            }
            else if (target.gameObject.name == "Chemical")
            {
                print(target.gameObject.name + " Target Manager");
                target.gameObject.AddComponent<EventHandler>();
            }
            else if (target.gameObject.name == "AR_marker")
            {
                print(this.gameObject.name + " Target Manager");
                target.gameObject.AddComponent<EventHandler>();
            }
        }
    }
}
