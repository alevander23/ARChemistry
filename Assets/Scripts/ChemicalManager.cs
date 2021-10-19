using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalManager
{
    private readonly List<Chemical> chemicals = new List<Chemical>();
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
            chemicals.Add(new Chemical(chemical[0], chemical[1], oppacity));
        }

    }


}
