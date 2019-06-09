using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameControl : MonoBehaviour
{
    public GameObject player;
    public GameObject Hand;
    public GameObject balancingScreen;
    public GameObject customer1;
    public GameObject customer2;

    public Text chem1;
    public Text chem2;
    public Text result;

    public Text chem1Total;
    public Text chem2Total;
    public Text resultTotal;
    public Text customer1Text;
    public Text customer2Text;

    public Text fix2;
    public Text fix3;
    public Text fix4;
    public Text fix5;

    public int threshold;

    private Chemical InHand; // Chemical player is currently holding. 


    private char sub_2 = (char)8322; // Subscript 2
    private char sub_3 = (char)8323; // Subscript 3


    private List<Chemical> chemicals = new List<Chemical>(); // List of chemicals player can pick up. 
    private List<string> orders = new List<string>(); // List of orders customers will make throughout level. 

    private Dictionary<Tuple<Chemical, Chemical>, Chemical> results = new Dictionary<Tuple<Chemical, Chemical>, Chemical>();

    private BalancingStation balanceStn = new BalancingStation();

    // Start is called before the first frame update
    void Start()
    {
        Chemical fe = new Chemical("Fe", 1, 0, false, Color.blue, "Fe");
        Chemical cl2 = new Chemical("Cl" + sub_2, 2, 0, false, Color.red, "Cl");
        Chemical o2 = new Chemical("O" + sub_2, 2, 0, false, Color.green, "O");
        Chemical h2 = new Chemical("H" + sub_2, 2, 0, false, Color.magenta, "H");

        results[Tuple.Create(fe, cl2)] = new Chemical("FeCl" + sub_3, 1, 3, true, Color.yellow, "FeCl");
        results[Tuple.Create(cl2, fe)] = new Chemical("FeCl" + sub_3, 3, 1, true, Color.yellow, "FeCl");
        results[Tuple.Create(fe, o2)] = new Chemical("Fe" + sub_2 + "O" + sub_3, 2, 3, true, Color.cyan, "FeO");
        results[Tuple.Create(o2, fe)] = new Chemical("Fe" + sub_2 + "O" + sub_3, 3, 2, true, Color.cyan, "FeO");
        results[Tuple.Create(o2, h2)] = new Chemical("H" + sub_2 + "O", 2, 1, true, Color.black, "HO");
        results[Tuple.Create(h2, o2)] = new Chemical("H" + sub_2 + "O", 1, 2, true, Color.black, "HO");


        chemicals.Add(fe);
        chemicals.Add(cl2);
        chemicals.Add(o2);
        chemicals.Add(h2);

        orders.Add("FeCl" + sub_3);
        orders.Add("Fe" + sub_2 + "O" + sub_3);
        orders.Add("FeCl" + sub_3);
        orders.Add("H" + sub_2 + "O");
        orders.Add("Fe" + sub_2 + "O" + sub_3);

        fix2.text = "Cl" + sub_2;
        fix3.text = "O" + sub_2;
        fix4.text = "H" + sub_2;
        fix5.text = "Fe" + sub_2 + "O" + sub_3;

        GenerateCustomers();
        InvokeRepeating("GenerateCustomers", 2.0f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateCustomers()
    {
        if (orders.Count != 0)
        {
            if (!customer1.activeSelf)
            {
                customer1.SetActive(true);
                customer1Text.text = orders[0];
                orders.RemoveAt(0);
            }
            if (!customer2.activeSelf && orders.Count != 0)
            {
                customer2.SetActive(true);
                customer2Text.text = orders[0];
                orders.RemoveAt(0);
            }
        }
    }


    public void ChemicalClick(int chemNum)
    {
        Chemical chem = chemicals[chemNum];
            if (!Hand.activeSelf)
            {
                Hand.SetActive(true);
                SpriteRenderer sr = Hand.GetComponent<SpriteRenderer>();
                InHand = chem;
                sr.color = InHand.Colour;
            }
        
    }


    public void TrashClick()
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;

        if (Math.Abs(player_y + 250) < threshold)
        {
            if (Math.Abs(player_x + 100) < threshold)
            {
                Hand.SetActive(false);
            }
        }
    }

    public void BalanceClick()
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;

        if (player_y < threshold && 6.5 < player_x && Hand.activeSelf)
        {

            balancingScreen.SetActive(true);
            Hand.SetActive(false);
            if (balanceStn.Reactant1 == null)
            {
                balanceStn.Reactant1 = InHand;
                balanceStn.QuantityR1 = 1;
                chem1.text = "1  " + InHand.Name;
                chem1Total.text = "Total: " + InHand.Subscript1.ToString() + " " + InHand.SingleName + " Molecules";

            }
            else
            {
                if (balanceStn.Reactant1 == InHand)
                {
                    balanceStn.QuantityR1++;
                    chem1.text = balanceStn.QuantityR1.ToString() + " " + InHand.Name;
                    chem1Total.text = "Total: " + (balanceStn.Reactant1.Subscript1 * balanceStn.QuantityR1).ToString() + " " + InHand.SingleName + " Molecules";
                    UpdateBalanced();
                }
                else
                {
                    if (balanceStn.Reactant2 == null)
                    {
                        balanceStn.Reactant2 = InHand;
                        balanceStn.QuantityR2 = 1;
                        chem2.text = "1  " + InHand.Name;
                        chem2Total.text = "Total: " + InHand.Subscript1 + " " + InHand.SingleName + " Molecules";

                        if (results.ContainsKey(Tuple.Create(balanceStn.Reactant1, balanceStn.Reactant2)))
                        {
                            balanceStn.Product = results[Tuple.Create(balanceStn.Reactant1, balanceStn.Reactant2)];
                            UpdateBalanced();
                        }
                        
                    }
                    else
                    {
                        if (balanceStn.Reactant2 == InHand)
                        {
                            balanceStn.QuantityR2++;
                            chem2.text = balanceStn.QuantityR2.ToString() + " " + InHand.Name;
                            chem2Total.text = "Total: " + (InHand.Subscript1 * balanceStn.QuantityR2).ToString() + " " + InHand.SingleName + " Molecules";
                            UpdateBalanced();
                        }
                        else
                        {
                            balancingScreen.SetActive(false);
                            Hand.SetActive(true);
                        }
                    }
                }
            }
        }

    }

    public void UpdateBalanced()
    {

        int num1 = (int)Math.Ceiling(balanceStn.QuantityR1 * balanceStn.Reactant1.Subscript1 / (float)balanceStn.Product.Subscript1);
        int num2 = (int)Math.Ceiling(balanceStn.QuantityR2 * balanceStn.Reactant2.Subscript1 / (float)balanceStn.Product.Subscript2);
        int goal = Math.Max(num1, num2);
        result.text = goal.ToString() + " " + balanceStn.Product.Name;
        int missing1 = balanceStn.Product.Subscript1 * goal - balanceStn.QuantityR1 * balanceStn.Reactant1.Subscript1;
        int missing2 = balanceStn.Product.Subscript2 * goal - balanceStn.QuantityR2 * balanceStn.Reactant2.Subscript1;

        if (missing1 == 0 && missing2 == 0)
        {
            // Balanced 

            resultTotal.text = "Balanced! You have finished the reaction for creating " + balanceStn.Product.Name + "!";
            Hand.SetActive(true);
            InHand = balanceStn.Product;

            SpriteRenderer sr = Hand.GetComponent<SpriteRenderer>();
            sr.color = InHand.Colour;
        }
        else
        {
            // Not balanced

            resultTotal.text = "Missing " + missing1.ToString() + " " + balanceStn.Reactant1.SingleName + " and " + missing2.ToString() + " " + balanceStn.Reactant2.SingleName;
        }
    }


    public void CustomerClick(int num)
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;
        string order = "";
        if (num == 0)
        {
            order = customer1Text.text;
        }
        else if (num == 1)
        {
            order = customer2Text.text;
        }

        if (player_x < -5.5 && player_y > -1)
        {
            if (order == InHand.Name)
            {
                if (num == 0)
                    customer1.SetActive(false);
                else if (num == 1)
                    customer2.SetActive(false);
                Hand.SetActive(false);

            }
        }

    }

    public void CloseScreen()
    {
        balancingScreen.SetActive(false);
        if (Hand.activeSelf)
            ClearScreen();
    }

    public void ClearScreen()
    {
        balanceStn.Reactant1 = null;
        balanceStn.Reactant2 = null;
        balanceStn.QuantityR1 = 0;
        balanceStn.QuantityR2 = 0;
        balanceStn.Product = null;

        chem1.text = "";
        chem2.text = "";
        result.text = "";
        chem1Total.text = "";
        chem2Total.text = "";
        resultTotal.text = "";
    }

}



public class Chemical
{
    public string Name;
    public string SingleName;
    public int Subscript1;
    public int Subscript2;
    public bool Product;
    public Color Colour;

    public Chemical(string name, int sub1, int sub2, bool product, Color colour, string singleName)
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



