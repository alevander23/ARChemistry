using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemistryLogic : MainFlow
{
    #region Chemistry Methods
    protected Dictionary<Chemical, float> CheckForReaction(Dictionary<Chemical, float> ContainerChemicals)
    {
        if (ContainerChemicals.Count > 1)
        {
            /*foreach(Reaction reaction1 in ReactionManager.Reactions)
            {
                print(reaction1.ReactantA.ChemicalName + " + " + reaction1.ReactantB.ChemicalName + " = " + reaction1.Product.ChemicalName);
            }*/
            foreach (Reaction reaction in ReactionManager.Reactions)
            {
                if (ContainerChemicals.ContainsKey(reaction.ReactantA))
                {
                    if (ContainerChemicals.ContainsKey(reaction.ReactantB))
                    {
                        //Adjust volume of each chemical, and add reactant with appropriate volume

                        //If reactant A is more plentiful than reactant B, remove all reactant B. Substract reactant B from reaction A volume.
                        if (ContainerChemicals[reaction.ReactantA] > ContainerChemicals[reaction.ReactantB])
                        {
                            ContainerChemicals[reaction.ReactantA] -= ContainerChemicals[reaction.ReactantB];
                            if (ContainerChemicals.ContainsKey(reaction.Product))
                            {
                                ContainerChemicals[reaction.Product] += 2 * ContainerChemicals[reaction.ReactantB];
                            }
                            else
                            {
                                ContainerChemicals.Add(reaction.Product, 2 * ContainerChemicals[reaction.ReactantB]);
                            }
                            ContainerChemicals.Remove(reaction.ReactantB);
                        }
                        //If reactant B and A volume is equal. Then remove all of both reactants Volume / remove from dictionary
                        else if (ContainerChemicals[reaction.ReactantA] == ContainerChemicals[reaction.ReactantB])
                        {
                            if (ContainerChemicals.ContainsKey(reaction.Product))
                            {
                                ContainerChemicals[reaction.Product] += 2 * ContainerChemicals[reaction.ReactantB];
                            }
                            else
                            {
                                ContainerChemicals.Add(reaction.Product, 2 * ContainerChemicals[reaction.ReactantB]);
                            }
                            ContainerChemicals.Remove(reaction.ReactantA);
                            ContainerChemicals.Remove(reaction.ReactantB);

                        }
                        // If reactant B is more plentiful do opposite of first if statement.
                        else
                        {
                            if (ContainerChemicals.ContainsKey(reaction.Product))
                            {
                                ContainerChemicals[reaction.Product] += 2 * ContainerChemicals[reaction.ReactantA];
                            }
                            else
                            {
                                ContainerChemicals.Add(reaction.Product, 2 * ContainerChemicals[reaction.ReactantA]);
                            }
                            ContainerChemicals[reaction.ReactantB] -= ContainerChemicals[reaction.ReactantA];
                            ContainerChemicals.Remove(reaction.ReactantA);
                        }
                    }
                }
            }

            /*print("Container Chemicals:");

            foreach (Chemical chemical in ContainerChemicals.Keys)
            {
                print(chemical.ChemicalName);
            }*/

            return ContainerChemicals;
        }
        else
        {
            return ContainerChemicals;
        }
    }

    /*protected void SolutionColor(Dictionary<Chemical, float> ContainerChemicals, MeshRenderer SolutionRenderer)
    {
        List<Color> colors = new List<Color>();
        SolutionRenderer.material = Resources.Load<Material>("TransparentMaterial");
        SolutionRenderer.material.EnableKeyword("_EMISSION");

        //Effective Volume for mixing Oppacity
        float Volume = ContainerChemicals.Count;

        //Effective Volume for mixing Color
        float ColorVolume = 0;

        //Counter for Opaque contents
        float OpaqueCounter = 0;

        //Counter for Transparent contents
        float TransparentCounter = 0;

        float red = 0f;
        float green = 0f;
        float blue = 0f;
        float alpha = 0f;
        
        foreach (Chemical chemical in ContainerChemicals.Keys)
        {
            colors.Add(chemical.ChemicalColor);
            if (chemical.ChemicalColor.a == 1.0f)
            {
                OpaqueCounter += ContainerChemicals[chemical];
            }
            else
            {
                TransparentCounter += ContainerChemicals[chemical];
            }
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
        }

        Color solutionColor = new Color(red, blue, green, alpha);

        if (solutionColor != Color.clear)
        {
            *//*print("entering color");*//*
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
            print("trans colorless");
            //Transparent Colorless Solutions
            solutionColor = new Color(1f, 1f, 1f, 0.01f);
            SolutionRenderer.material.SetColor("_EmissionColor", new Vector4(1.0f, 1.0f, 1.0f, 0f) * 0.25f);
        }

        SolutionRenderer.material.renderQueue = 2450;
        SolutionRenderer.material.color = solutionColor;
        
    }*/


    /*protected void UpdateVolume(GameObject ContainerObject, GameObject SolutionObject, float Volume)
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

        *//*return SolutionObject;*//*
    }*/
    #endregion
}
