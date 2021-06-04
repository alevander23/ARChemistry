using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[RequireComponent(typeof(Chemical))]*/
public class ChemicalManager : MonoBehaviour
{
    /*private Chemical chemical = null;*/
    private List<Chemical> chemicals = new List<Chemical>();
    private List<string> chemicalNames = new List<string>();
    private List<string> ChemicalIndexList = new List<string>();
    public List<Chemical> Chemicals { get => chemicals; }
    public List<string> ChemicalNames { get => chemicalNames; }
    public List<string> chemicalIndexList { get => ChemicalIndexList; }

    void Awake()
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
            string[] chemical = line.Split('#');
            float oppacity = float.Parse(chemical[2]);
            this.chemicals.Add(new Chemical(chemical[0], chemical[1], oppacity));
            this.chemicalNames.Add(chemical[0]);
            this.ChemicalIndexList.Add(Convert.ToString(counter));
            counter++;
        }

    }



}
