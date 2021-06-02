using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[RequireComponent(typeof(Chemical))]*/
public class ChemicalManager 
{
    /*private Chemical chemical = null;*/
    private List<Chemical> chemicals = new List<Chemical>();
    private List<string> chemicalNames = new List<string>();
    private List<string> ChemicalIndexList = new List<string>();
    public List<Chemical> Chemicals { get => chemicals; }
    public List<string> ChemicalNames { get => chemicalNames; }
    public List<string> chemicalIndexList { get => ChemicalIndexList; }

    void Start()
    {
        /*chemical = GetComponent<Chemical>();*/
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

        int counter = 0;
        foreach (string line in lines)
        {
            string[] chemicalString = line.Split('#');

            float oppacity = float.Parse(chemicalString[2]);
            /*this.gameObject.AddComponent<Chemical>();*/
            chemicals.Add(new Chemical(chemicalString[0], chemicalString[1], oppacity));
            chemicalNames.Add(chemicalString[0]);
            ChemicalIndexList.Add(Convert.ToString(counter));
            counter++;
        }

    }

}
