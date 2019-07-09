using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Chemical
{
    public string Name;
    public string SingleName;
    public int Subscript1;
    public int Subscript2;
    public bool Product;
    public Sprite Colour;

    public Chemical(string name, int sub1, int sub2, bool product, Sprite colour, string singleName)
    {
        Name = name;
        Subscript1 = sub1;
        Subscript2 = sub2;
        Product = product;
        Colour = colour;
        SingleName = singleName;
    }
}

public class BalancingStation
{
    public Chemical Reactant1;
    public Chemical Reactant2;

    public int QuantityR1;
    public int QuantityR2;

    public Chemical Product;
}



