using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction 
{
    private Chemical reactantA = null;
    private Chemical reactantB = null;
    private Chemical product = null;

    #region Constructor
    public Reaction(string _ReactantA, string _ReactantB, string _Product, List<Chemical> chemicals)
    {
        foreach (Chemical chemical in chemicals)
        {
            if (chemical.ChemicalName.Equals(_ReactantA))
            {
                reactantA = chemical;
            }
            else if (chemical.ChemicalName.Equals(_ReactantB))
            {
                reactantB = chemical;
            }
            else if (chemical.ChemicalName.Equals(_Product))
            {
                product = chemical;
            }
        }

    }
    #endregion

    #region Getter Methods
    public Chemical ReactantA { get => reactantA; }
    public Chemical ReactantB { get => reactantB; }
    public Chemical Product { get => product; }
    #endregion

}
