using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System.Linq;
using UnityEngine.Events;

public class EventHandler : MonoBehaviour, ITrackableEventHandler
{
    public UnityAction OnTrackingFound;
    public UnityAction OnTrackingLost;
    public UnityAction OnSetupCalled;

    private TrackableBehaviour mTrackableBehaviour = null;



    private readonly List<TrackableBehaviour.Status> mTrackingFound = new List<TrackableBehaviour.Status>()
    {
        TrackableBehaviour.Status.DETECTED,
        TrackableBehaviour.Status.TRACKED,

        /*//Device positioning
        TrackableBehaviour.Status.EXTENDED_TRACKED*/

    };

    private readonly List<TrackableBehaviour.Status> mTrackingLost = new List<TrackableBehaviour.Status>()
    {
        TrackableBehaviour.Status.TRACKED,
        TrackableBehaviour.Status.NO_POSE
    };

    private void Awake()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        mTrackableBehaviour.RegisterTrackableEventHandler(this);

    }

    private void OnDestroy()
    {
        mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        //If tracking found
        foreach(TrackableBehaviour.Status trackedStatus in mTrackingFound)
        {
            if (newStatus == trackedStatus)
            {
                if (OnTrackingFound != null)
                    print('2');
                    OnTrackingFound();

                print("Tracking Found");

                return;
            }
        }
        //If tracking lost
        foreach (TrackableBehaviour.Status trackedStatus in mTrackingLost)
        {
            if (newStatus == trackedStatus)
            {
                if (OnTrackingLost != null)
                    OnTrackingLost();

                print("Tracking Lost");

                return;
            }
        }
    }
}
