using System;
using UnityEngine;

public class Chemical 
{
    #region Attributes
    private string chemicalName = "";
    private UnityEngine.Color chemicalColor = new UnityEngine.Color(0f, 0f, 0f, 0f);
    #endregion

    #region Constructor
    public Chemical(string chemicalName, string chemicalColor, float chemicalOppacity)
    {
        this.chemicalName = chemicalName;
        string[] rgb = chemicalColor.Split(',');
        if (chemicalOppacity != 0)
        {
            this.chemicalColor = new UnityEngine.Color(float.Parse(rgb[0]) / 255, float.Parse((rgb[1])) / 255, float.Parse(rgb[2]) / 255, chemicalOppacity);
        }
        else
        {
            this.chemicalColor = Color.clear;
        }
    }
    #endregion

    #region Getter Methods
    public string ChemicalName { get => chemicalName; }
    public UnityEngine.Color ChemicalColor { get => chemicalColor; }
    #endregion

}
