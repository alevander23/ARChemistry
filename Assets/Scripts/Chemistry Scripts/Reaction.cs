using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction 
{
    #region Attributes
    private string ReactantA;
    private string ReactantB;
    private string Product;
    #endregion

    #region Constructor
    public Reaction(string ReactantA, string ReactantB, string Product)
    {
        this.ReactantA = ReactantA;
        this.ReactantB = ReactantB;
        this.Product = Product;
    }
    #endregion

    #region Getter Methods
    public string reactantA { get => ReactantA; }
    public string reactantB { get => ReactantB; }
    public string product { get => Product; }
    #endregion

}
