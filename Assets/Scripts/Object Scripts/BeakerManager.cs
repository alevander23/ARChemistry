using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeakerManager : MonoBehaviour
{
    #region Attributes

    private ChemicalManager chemicalManager;

    private ReactionManager reactionManager;

    private List<Chemical> BeakerChemicals = new List<Chemical>();

    private List<string> BeakerChemicalsChecker = new List<string>();
   
    private int Volume;
    #endregion

    #region Monobehaviour API

    private void Awake()
    {
        chemicalManager = GameObject.Find("TargetManager").GetComponent<ChemicalManager>();
        reactionManager = GameObject.Find("TargetManager").GetComponent<ReactionManager>();
        Volume = 0;

    }

    private void OnTriggerEnter(Collider other)
    {
        //Deletes collider object
        string pillName = other.name;
        Destroy(other);
        GameObject pill = GameObject.Find(pillName);
        Destroy(pill);

        print(other.name + " Triggered");
        if (other.name == "Beaker" || other.name == "Test_Tube")
        {

        }
        else
        {
            //Adds new chemical to Beaker
            AddChemical(other);

            foreach (Chemical chemical in BeakerChemicals)
            {
                print(chemical.ChemicalName);
            }

            //If Beaker has multiple chemicals check to see if reaction is possible
            if (BeakerChemicals.Count > 1)
            {
                CheckReaction();
            }
            UpdateBeaker();

        }
    }
    #endregion

    #region Chemistry Functions
    private void AddChemical(Collider other)
    {
        foreach (Chemical chemical in chemicalManager.Chemicals)
        {
            if (chemical.ChemicalName == other.name)
            {
                print(chemical.ChemicalName + "Add Chemical");
                BeakerChemicalsChecker.Add(chemical.ChemicalName);
                BeakerChemicals.Add(chemical);
                print(Convert.ToString(Volume));
                Volume++;
                print(Convert.ToString(Volume));
            }
        }
    }

    private void CheckReaction()
    {
        foreach (Reaction reaction in reactionManager.Reactions)
            //If reaction is a match
            if (BeakerChemicalsChecker.Contains(reaction.reactantA) && BeakerChemicalsChecker.Contains(reaction.reactantB))
            {
                //Remove reactants from Beaker
                foreach (Chemical chemical in BeakerChemicals)
                {
                    if (chemical.ChemicalName == reaction.reactantA)
                    {
                        BeakerChemicals.Remove(chemical);
                        break;
                    }
                }
                foreach (Chemical chemical in BeakerChemicals)
                {
                    if (chemical.ChemicalName == reaction.reactantB)
                    {
                        BeakerChemicals.Remove(chemical);
                        break;
                    }
                }
                //Add the product to the Beaker
                foreach (Chemical chemical in chemicalManager.Chemicals)
                {
                    if (chemical.ChemicalName == reaction.product)
                    {
                        BeakerChemicalsChecker.Add(reaction.product);
                        BeakerChemicals.Add(chemical);
                        //break isnt neccesary as every chemical in chemicals is unique
                    }
                }
            }
    }
    #endregion

    #region Unity Beaker
    private void UpdateBeaker()
    {
        //Referencing beaker and contents GameObjects
        GameObject Beaker = GameObject.Find("BeakerObject");
        GameObject Cylinder = GameObject.Find("Volume");
        MeshRenderer CylinderRenderer = Cylinder.GetComponent<MeshRenderer>();

        //Working out what color the solution will be
        Color solutionColor = GetColor();
        CylinderRenderer.material.color = solutionColor;

        //Updating volume of solution 
        // 0.0001 is the scale for 1 unit of change and 0.00025 is the initial offset when unit = 1
        ChangeVolume(Cylinder);
        
    }

    private Color GetColor()
    {
        //Color mixer
        List<Color> colors = new List<Color>();
        Color colorless = new Color(253, 253, 253, 128);
        float red = 0;
        float green = 0;
        float blue = 0;
        float alpha = 0;

        foreach (Chemical chemical in BeakerChemicals)
        {
            Color chemicalColor = new Color(chemical.ChemicalColor[0], 
                                            chemical.ChemicalColor[1], 
                                            chemical.ChemicalColor[2], 
                                            chemical.ChemicalOppacity);
            if (chemicalColor != colorless)
            {
                colors.Add(chemicalColor);
            }
            
        }

        foreach(Color color in colors)
        {
            red += color.r;
            green += color.g;
            blue += color.b;
            alpha += color.a;
        }

        //Blending the colors of all chemicals in solution. 
        print(colors.Count);
        print(red);
        print(green);
        print(blue);
        red /= colors.Count;
        blue /= colors.Count;
        green /= colors.Count;
        alpha /= colors.Count;

        print(red);
        print(green);
        print(blue);
        Color SolutionColor = new Color(red, blue, green, alpha);
        return SolutionColor;
    }

    private void ChangeVolume(GameObject Cylinder)
        //0.05 starting height of volume - 0.1 is good scale for one unit new height is scale + starting height
    {
        Cylinder.transform.localScale = new Vector3(0.5624999f, 0.1f*Volume, 0.5624999f);
        print("Volume = " + Convert.ToString(Volume));
        Cylinder.transform.localPosition = new Vector3(0f, (0.05f + (Volume * 0.1f)), 0f);
    }
    #endregion

}
