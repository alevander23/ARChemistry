using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventHandler))]
public class ObjectCreator : MonoBehaviour
{
    private EventHandler eventHandler = null;
    //This can be scaled up to essentially unlimited by putting a cap of 10 as you wouldnt ever really be able to use more than that anyways. 
    //Logic can be added later when I am ready to implement this. 
    private MeshRenderer[] renderers = new MeshRenderer[3];

    private void Awake()
    {
        Setup();

        eventHandler = GetComponent<EventHandler>();
        eventHandler.OnTrackingFound += Show;
        eventHandler.OnTrackingLost += Hide;
    }

    private void OnDestroy()
    {
        eventHandler.OnTrackingFound -= Show;
        eventHandler.OnTrackingLost -= Hide;
    }

    private void Setup()
    {
        print(this.gameObject.name + "Setup");
        if(this.gameObject.name == "Beaker")
        {
            GameObject Beaker = GameObject.Find("Beaker");
            Beaker.transform.parent = transform;
            /*Beaker.transform.localEulerAngles = new Vector3(0, 0, 0);*/

            MeshRenderer BeakerRenderer = Beaker.GetComponent<MeshRenderer>();
            GameObject Volume = Beaker.transform.GetChild(0).gameObject;
            Volume.transform.parent = transform;
            MeshRenderer VolumeRenderer = Volume.GetComponent<MeshRenderer>();
            renderers[0] = BeakerRenderer;
            renderers[1] = VolumeRenderer;

            Hide();
        }
    }

    private void Show()
    {
        print(this.gameObject.name + " Show Method");
        if(this.gameObject.name == "Beaker")
        {
            print(this.gameObject.name + " Shown");
            
            renderers[0].enabled = true;
            renderers[1].enabled = true;
        }
    }

    private void Hide()
    {
        if(this.gameObject.name == "Beaker")
        { 
            print(this.gameObject.name + " Hiden");
            renderers[0].enabled = false;
            renderers[1].enabled = false;
        }
    }
}
