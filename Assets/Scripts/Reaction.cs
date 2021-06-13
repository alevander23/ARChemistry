using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction
{
    #region Attributes
    private Chemical ReactantA;
    private Chemical ReactantB;
    private Chemical Product;
    #endregion

    #region Constructor
    public Reaction(string ReactantA, string ReactantB, string Product, List<Chemical> chemicals)
    {
        foreach (Chemical chemical in chemicals)
        {
            if (chemical.ChemicalName == ReactantA)
            {
                this.ReactantA = chemical;
            }
            else if (chemical.ChemicalName == ReactantB)
            {
                this.ReactantB = chemical;
            }
            else if (chemical.ChemicalName == Product)
            {
                this.Product = chemical;
            }
        }
    }
    #endregion

    #region Getter Methods
    public Chemical reactantA { get => ReactantA; }
    public Chemical reactantB { get => ReactantB; }
    public Chemical product { get => Product; }
    #endregion

}
