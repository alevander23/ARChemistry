using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ContainerChemistry : MonoBehaviour
{
    private EventHandler eventHandler = null;

    private GameObject ContainerObject = null;

    private GameObject SolutionObject = null;

   /* private CapsuleCollider ContainerTrigger = null;*/

    private MeshRenderer ContainerRenderer = null;

    private MeshRenderer SolutionRenderer = null;

    private List<Chemical> ContainerChemicals = new List<Chemical>();

    private List<Chemical> chemicalList = new List<Chemical>();

    private List<Reaction> reactionList = new List<Reaction>();

    private bool isColliding = false;

    private int Volume = 0;

    private void Awake()
    {
        chemicalList = MainFlow.MainInstance.ChemicalList;
        reactionList = MainFlow.ReactionList;
    }

    public void SetParent(GameObject _ContainerObject, GameObject Trigger)
    {
        ContainerObject = _ContainerObject;
        SolutionObject = ContainerObject.transform.Find("Solution").gameObject;
        print(SolutionObject.name + "  Set Parent");
        ContainerRenderer = ContainerObject.GetComponent<MeshRenderer>();
        SolutionRenderer = SolutionObject.GetComponent<MeshRenderer>();
        Hide();
    }

    private void OnDestroy()
    {
        eventHandler.OnTrackingFound -= Show;
        eventHandler.OnTrackingLost -= Hide;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("triggered");
        if (isColliding == false)
        {
            isColliding = true;
            if (other.tag == "Chemical")
            {
                /*ContainerTrigger.isTrigger = false;*/
                foreach (Chemical chemical in chemicalList)
                {
                    if (chemical.ChemicalName == other.name)
                    {
                        AddChemical(chemical);
                        Destroy(other.gameObject);
                    }
                }
                CheckForReaction();
                UpdateContainer();
            }
        }
    }

    private void Update()
    {
        isColliding = false;
    }

    private void AddChemical(Chemical chemical)
    {
        ContainerChemicals.Add(chemical);
        Volume++;
        foreach (Chemical chemical1 in ContainerChemicals)
        {
            print(chemical1.ChemicalName);
            print(chemical1.Num);
        }
    }
    private void CheckForReaction()
    {
        print("Entering Check for Reaction");
        if (ContainerChemicals.Count > 1)
        {
            print("proceeds");
            foreach (Reaction reaction in reactionList)
            {
                print(reaction.ReactantA.ChemicalName);
                print(reaction.ReactantA.Num);
                if (ContainerChemicals.Contains(reaction.ReactantA))
                {
                    if (ContainerChemicals.Contains(reaction.ReactantB))
                    {
                        print("Both reactants present");
                        ContainerChemicals.Remove(reaction.ReactantA);
                        print("removed A");
                        ContainerChemicals.Remove(reaction.ReactantB);
                        print("removed B");
                        ContainerChemicals.Add(reaction.Product);

                        break;
                    }
                }
                
            }
        }
    }

    private void UpdateContainer()
    {
        SolutionColor();
        UpdateVolume();
    }

    private void SolutionColor()
    {
        List<Color> colors = new List<Color>();

        float red = 0f;
        float green = 0f;
        float blue = 0f;
        float alpha = 0f;

        foreach (Chemical chemical in ContainerChemicals)
        {
            colors.Add(chemical.ChemicalColor);
        }

        foreach (Color color in colors)
        {
            red += color.r;
            green += color.g;
            blue += color.b;
            alpha += color.a;
        }

        red /= Volume;
        green /= Volume;
        blue /= Volume;
        alpha /= Volume;

        SolutionRenderer.material = Resources.Load<Material>("TransparentMaterial");
        Color solutionColor = new Color(red, green, blue, alpha);
        SolutionRenderer.material.EnableKeyword("_EMISSION");
        SolutionRenderer.material.SetColor("_EmissionColor", solutionColor);
        SolutionRenderer.material.renderQueue = 2450;
        SolutionRenderer.material.color = solutionColor;
        print("Alpha value : " + alpha);
    }

    private void UpdateVolume()
    {
        //0.05 starting height of volume - 0.1 is good scale for one unit 
        //new height is scale + starting height
        if (ContainerObject.name == "Beaker")
        {
            //this still needs to be fiddled with
            SolutionObject.transform.localScale = new Vector3(0.5624999f, 0.1f * Volume, 0.5624999f);
            SolutionObject.transform.localPosition = new Vector3(0f, (0.05f + (Volume * 0.1f)), 0f);
        }
        else
        {
            SolutionObject.transform.localScale = new Vector3(0.00013f, (0.0001f * Volume), 0.00013f);
            SolutionObject.transform.localPosition = new Vector3(0f, 0f, (-0.00068f + (0.0001f*Volume)));
        }
    }

    public void PassEventHandler(EventHandler _eventHandler)
    {
        eventHandler = _eventHandler;
        eventHandler.OnTrackingFound += Show;
        eventHandler.OnTrackingLost += Hide;
    }

    private void Show()
    {
        ContainerRenderer.enabled = true;
        SolutionRenderer.enabled = true;
    }

    private void Hide()
    {
        ContainerRenderer.enabled = false;
        SolutionRenderer.enabled = false;
    }
}

