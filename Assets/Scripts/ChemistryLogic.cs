using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemistryLogic : MainFlow
{
    #region Chemistry Methods
    protected List<Chemical> CheckForReaction(List<Chemical> ContainerChemicals)
    {
        print("Entering Check for Reaction");
        if (ContainerChemicals.Count > 1)
        {
            /*foreach(Reaction reaction1 in ReactionManager.Reactions)
            {
                print(reaction1.ReactantA.ChemicalName + " + " + reaction1.ReactantB.ChemicalName + " = " + reaction1.Product.ChemicalName);
            }*/
            print(ReactionManager.Reactions.Count);
            print("proceeds");
            foreach (Reaction reaction in ReactionManager.Reactions)
            {
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
                        print("added C");
                        print(ReactionManager.Reactions.Count + " after removal count");
                    }
                }
            }

            print("Container Chemicals:");

            foreach (Chemical chemical in ContainerChemicals)
            {
                print(chemical.ChemicalName);
            }

            return ContainerChemicals;
        }
        else
        {
            return ContainerChemicals;
        }
    }

    protected void SolutionColor(List<Chemical> ContainerChemicals, MeshRenderer SolutionRenderer)
    {
        List<Color> colors = new List<Color>();
        SolutionRenderer.material = Resources.Load<Material>("TransparentMaterial");
        SolutionRenderer.material.EnableKeyword("_EMISSION");
        //Effective Volume for mixing Oppacity
        int Volume = ContainerChemicals.Count;

        //Effective Volume for mixing Color
        int ColorVolume = 0;

        //Counter for Opaque contents
        int OpaqueCounter = 0;
        //Counter for Transparent contents
        int TransparentCounter = 0;

        float red = 0f;
        float green = 0f;
        float blue = 0f;
        float alpha = 0f;
        
        foreach (Chemical chemical in ContainerChemicals)
        {
            colors.Add(chemical.ChemicalColor);
            print(chemical.ChemicalName);
        }

        foreach (Color color in colors)
        {
            if (color != Color.clear)
            {
                ColorVolume++;
            }
            red += color.r;
            green += color.g;
            blue += color.b;
            alpha += color.a;
            if (color.a == 1.0f)
            {
                OpaqueCounter++;
            }
            else
            {
                TransparentCounter++;
            }
        }

        print(alpha);
        Color solutionColor = new Color(red, blue, green, alpha);

        if (solutionColor != Color.clear)
        {
            print("entering color");
            red /= ColorVolume;
            green /= ColorVolume;
            blue /= ColorVolume;
            alpha /= Volume;
            if (alpha <= 0.01)
            {
                //Transparent Colored Solutions
                solutionColor = new Color(red, green, blue, alpha);
                SolutionRenderer.material.SetColor("_EmissionColor", solutionColor);
            }
            else
            {
                //Opaque Solutions
                alpha = 1 - (0.05f * TransparentCounter);
                solutionColor = new Color(red, green, blue, alpha);
            }
        }
        else
        {
            //Transparent Colorless Solutions
            solutionColor = new Color(1f, 1f, 1f, 0f);
            SolutionRenderer.material.SetColor("_EmissionColor", new Vector4(1.0f, 1.0f, 1.0f, 0f) * 0.25f);
        }

        SolutionRenderer.material.renderQueue = 2450;
        SolutionRenderer.material.color = solutionColor;
        
    }


    protected void UpdateVolume(GameObject ContainerObject, GameObject SolutionObject, int Volume)
    {
        print("update Volume");
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
            SolutionObject.transform.localPosition = new Vector3(0f, 0f, (-0.00068f + (0.0001f * Volume)));
        }

        /*return SolutionObject;*/
    }
    #endregion
}
