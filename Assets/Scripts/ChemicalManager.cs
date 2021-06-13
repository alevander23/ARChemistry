using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalManager
{
    private List<Chemical> chemicals = new List<Chemical>();
    private List<string> chemicalNames = new List<string>();
    public List<Chemical> Chemicals { get => chemicals; }
    /*public List<string> ChemicalNames { get => chemicalNames; }*/

    public ChemicalManager()
    {
        TextAsset chemicalsAsset = (TextAsset)Resources.Load("Chemicals", typeof(TextAsset));
        string chemicalsText = chemicalsAsset.text;
        List<string> lines = new List<string>();

        //Seperating text line by line
        string bufferText = "";
        foreach (char _ in chemicalsText)
        {
            if (_ == '\n')
            {
                lines.Add(bufferText);
                bufferText = "";
            }
            else
            {
                bufferText += _.ToString();
            }
        }
        lines.Add(bufferText);
 
        foreach (string line in lines)
        {
            string[] chemical = line.Split('#');
            float oppacity = float.Parse(chemical[2]);
            this.chemicals.Add(new Chemical(chemical[0], chemical[1], oppacity));
            this.chemicalNames.Add(chemical[0]);
        }

    }

    public string GetNameOfChemicalIndex(int Index)
    {
        return chemicalNames[Index];
    }



}
