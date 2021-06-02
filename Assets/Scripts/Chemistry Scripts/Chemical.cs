using System;
using UnityEngine;

public class Chemical 
{
    #region Attributes
    private string chemicalName;
    private int[] chemicalColor;
    private float chemicalOppacity;
    #endregion

    #region Constructor
    public Chemical(string chemicalName, string chemicalColor, float chemicalOppacity)
    {
        this.chemicalName = chemicalName;
        string[] rgb = chemicalColor.Split(',');
        this.chemicalColor[0] = Convert.ToInt32(rgb[0]);
        this.chemicalColor[1] = Convert.ToInt32(rgb[1]);
        this.chemicalColor[2] = Convert.ToInt32(rgb[2]);
        this.chemicalOppacity = chemicalOppacity;
    }
    #endregion

    #region Getter Methods
    public string ChemicalName { get => chemicalName; }
    public int[] ChemicalColor { get => chemicalColor; }
    public float ChemicalOppacity { get => chemicalOppacity; }
    #endregion

    public override string ToString()
    {
        return chemicalName + chemicalColor + chemicalOppacity;
    }
    
}
