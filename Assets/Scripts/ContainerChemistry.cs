using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ContainerChemistry : MonoBehaviour
{
    private EventHandler eventHandler = null;

    private GameObject ContainerObject = null;

    private GameObject SolutionObject = null;

    private MeshRenderer ContainerRenderer = null;

    private MeshRenderer SolutionRenderer = null;

    private List<Chemical> ContainerChemicals = new List<Chemical>();

    private ReactionManager reactionManager = null;

    private List<Chemical> chemicalList = null;

    private List<Reaction> reactionList = null;

    
    private int Volume = 0;

    private void Awake()
    {
        chemicalList = MainFlow.ChemicalList;
        reactionList = MainFlow.ReactionList;
       /* eventHandler = this.gameObject.GetComponent<EventHandler>();*/
    }

    public void SetParent(GameObject _ContainerObject)
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
        print("Hello");
        Destroy(other.gameObject);
        if (other.tag == "Chemical")
        {
            foreach (Chemical chemical in chemicalList)
            {
                if (chemical.ChemicalName == other.name)
                {
                    AddChemical(chemical);
                }
            }
            CheckForReaction();
            UpdateContainer();
        }
    }

    private void AddChemical(Chemical chemical)
    {
        ContainerChemicals.Add(chemical);
        Volume++;
        foreach (Chemical _chemical in ContainerChemicals)
        {
            print(_chemical.ChemicalName);
        }
    }
    private void CheckForReaction()
    {
        print("Entering Check for Reaction");
        foreach (Reaction reaction in reactionList)
        {
            if (ContainerChemicals.Contains(reaction.reactantA))
            {
                if (ContainerChemicals.Contains(reaction.reactantB))
                {
                    print("Both reactants present");
                    this.ContainerChemicals.Remove(reaction.reactantA);
                    this.ContainerChemicals.Remove(reaction.reactantB);
                    this.ContainerChemicals.Add(reaction.product);
                }
            }
        }
    }

    private void UpdateContainer()
    {
        this.SolutionColor();
        this.UpdateVolume();
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

        this.SolutionRenderer.material = Resources.Load<Material>("TransparentMaterial");
        Color solutionColor = new Color(red, green, blue, alpha);
        this.SolutionRenderer.material.EnableKeyword("_EMISSION");
        this.SolutionRenderer.material.SetColor("_EmissionColor", solutionColor);
        this.SolutionRenderer.material.renderQueue = 2450;
        this.SolutionRenderer.material.color = solutionColor;
        print("Alpha value : " + alpha);
    }

    private void UpdateVolume()
    {
        //0.05 starting height of volume - 0.1 is good scale for one unit 
        //new height is scale + starting height
        if (ContainerObject.name == "Beaker")
        {
            //this still needs to be fiddled with
            this.SolutionObject.transform.localScale = new Vector3(0.5624999f, 0.1f * Volume, 0.5624999f);
            this.SolutionObject.transform.localPosition = new Vector3(0f, (0.05f + (Volume * 0.1f)), 0f);
        }
        else
        {
            SolutionObject.transform.localScale = new Vector3(0.00013f, (0.0001f * Volume), 0.00013f);
            SolutionObject.transform.localPosition = new Vector3(0f, 0f, (-0.00068f + (0.0001f*Volume)));
        }
    }

    public void PassEventHandler(EventHandler _eventHandler)
    {
        this.eventHandler = _eventHandler;
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

