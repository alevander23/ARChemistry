using System;
using UnityEngine;

public class Chemical 
{
    #region Attributes
    private string chemicalName = "";
    private float[] chemicalColor = new float[3];
    private float chemicalOppacity = 0f;
    #endregion

    #region Constructor
    public Chemical(string chemicalName, string chemicalColor, float chemicalOppacity)
    {
        this.chemicalName = chemicalName;
        string[] rgb = chemicalColor.Split(',');
        this.chemicalColor[0] = float.Parse(rgb[0]);
        this.chemicalColor[1] = float.Parse(rgb[1]);
        this.chemicalColor[2] = float.Parse(rgb[2]);
        this.chemicalOppacity = chemicalOppacity;
    }
    #endregion

    #region Getter Methods
    public string ChemicalName { get => chemicalName; }
    public float[] ChemicalColor { get => chemicalColor; }
    public float ChemicalOppacity { get => chemicalOppacity; }
    #endregion

    public override string ToString()
    {
        return chemicalName + chemicalColor + chemicalOppacity;
    }
    
}
