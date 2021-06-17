using System;
using UnityEngine;

public class Chemical 
{
    #region Attributes
    private readonly string chemicalName = "";
    private readonly UnityEngine.Color chemicalColor = new UnityEngine.Color(0f, 0f, 0f, 0f);
    private readonly int num = 0;
    #endregion

    #region Constructor
    public Chemical(string chemicalName, string chemicalColor, float chemicalOppacity)
    {
        System.Random random = new System.Random();
        num = random.Next();
        this.chemicalName = chemicalName;
        string[] rgb = chemicalColor.Split(',');
        this.chemicalColor = new UnityEngine.Color(float.Parse(rgb[0])/255, float.Parse((rgb[1]))/255, float.Parse(rgb[2])/255, chemicalOppacity);
    }
    #endregion

    #region Getter Methods
    public string ChemicalName { get => chemicalName; }
    public UnityEngine.Color ChemicalColor { get => chemicalColor; }
    public int Num { get => num; }
    #endregion

}
