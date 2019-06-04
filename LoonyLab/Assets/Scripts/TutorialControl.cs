using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TutorialControl : MonoBehaviour
{
    public GameObject player;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject balancingScreen;
    public GameObject customer1;

    public Text chem1;
    public Text chem2;
    public Text result;
    public Text customer1Text;
    public Text tutorialText;

    public Text fix2;
    public Text fix3;
    public Text fix4;
    public Text fix5;

    public InputField inChem1;
    public InputField inChem2;
    public InputField inResult;

    public int threshold;
    private Dictionary<string, Color> chemicals = new Dictionary<string, Color>();
    private Dictionary<Tuple<string, string>, Tuple<string, int, int, int, Color>> results = new Dictionary<Tuple<string, string>, Tuple<string, int, int, int, Color>>();
    private string leftName = "X";
    private string rightName = "X";

    private bool balancing = false;

    private char sub_2 = (char)8322;
    private char sub_3 = (char)8323;
    private List<string> chemNames = new List<string>();
    private List<string> orders = new List<string>();

    private bool tutorial_items = false;
    private bool tutorial_balance = false;
    private bool tutorial_customer = false;

    private int num = 0;
    private List<String> tutorialList= new List<String>();

    // Start is called before the first frame update
    void Start()
    {

        chemicals["Fe"] = Color.blue;
        chemicals["Cl" + sub_2] = Color.red;


        results[Tuple.Create("Fe", "Cl" + sub_2)] = Tuple.Create("FeCl" + sub_3, 2, 3, 2, Color.magenta);
        results[Tuple.Create("Cl" + sub_2, "Fe")] = Tuple.Create("FeCl" + sub_3, 3, 2, 2, Color.magenta);


        chemNames.Add("Fe");
        chemNames.Add("Cl" + sub_2);

        orders.Add("FeCl" + sub_3);

        fix2.text = "Cl" + sub_2;

        tutorialList.Add("Here we are! This lab is where you're going to be making all of the chemicals and delivering the orders!");
        tutorialList.Add("And here's your first customer! Since this is your first day, I'll lead you through the steps to make his order.");
        tutorialList.Add("This customer is ordering FeCl" + sub_3 + "! I know those symbols can seem confusing, so let's break the chemical down.");
        tutorialList.Add("FeCl" + sub_3 + " can also be called Iron Chloride. This is because it is made up of two elements:");
        tutorialList.Add("Iron (Fe) and Chlorine (Cl). So when you combine them together, you create a compound called Iron Chloride.");
        tutorialList.Add("You may be wondering what the little 3 under the chlorine symbol means. That subscript tells you how many chlorine molecules are needed to make the compound.");
        tutorialList.Add("So in this case, to make one Iron Chloride molecule, you will need one Iron molecule and three Chlorine molecules. ");
        tutorialList.Add("This is where you come in! Let's get started by going over to the chemical cabinet and picking up some Fe and Cl molecules.");
        tutorialList.Add("Awesome job! Now that you have the elements you need, let's head on over to the balancing station and make our compound!");
        tutorialList.Add("Balancing our chemical equation is probably the trickiest part of working at the Loony Lab.");
        tutorialList.Add("Because of something called the Law of Conservation of Mass, the number of molecules on the reactants side of the equation has to be the same as the number of molecules on the product side.");
        tutorialList.Add("This means that if your product uses three Chlorine molecules, your reactants must also have three Chlorine molecules.");
        tutorialList.Add("It's up to you to balance the equation by determining how many groups of Iron and Chlorine molecules you need in your reactants,");
        tutorialList.Add("and how many groups of Iron Chloride you need in your products to make sure both sides are balanced.");
        tutorialList.Add("Right now you have a total of 1 Iron molecule and 2 Chlorine molecules in your reactants, but a total of 1 Iron molecule and 3 Chlorine molecules in your products.");
        tutorialList.Add("Change the coeffecients to make sure the equation is balanced!");
        tutorialList.Add("Nice going, you're a natural at this! Now you can take your finished compound and give it to the customer.");
        tutorialList.Add("You did some great science today! Now it's time for you to test your skills and try completing tomorrow's orders on your own.");

        GenerateCustomers();
        NextClick();
    }

    // Update is called once per frame
    void Update()
    {
       if (balancing)
        {
            if (CheckCorrect())
            {
                var tpl = Tuple.Create(leftName, rightName);
                balancing = false;
                balancingScreen.SetActive(false);
                rightHand.SetActive(true);
                rightName = results[tpl].Item1;
                SpriteRenderer sr = rightHand.GetComponent<SpriteRenderer>();
                sr.color = results[tpl].Item5;
                inChem1.text = "";
                inChem2.text = "";
                inResult.text = "";
            }
        } 
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
        }
    }


    public void ChemicalClick(int chemNum)
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;
        string chemName = chemNames[chemNum];

        if (Math.Abs(player_y - 250) < threshold && player_x < 0 && tutorial_items)
        {
            if (!rightHand.activeSelf)
            {
                rightHand.SetActive(true);
                SpriteRenderer sr = rightHand.GetComponent<SpriteRenderer>();
                rightName = chemName;
                sr.color = chemicals[chemName];
            }
            else if (!leftHand.activeSelf)
            {
                leftHand.SetActive(true);
                SpriteRenderer sr = leftHand.GetComponent<SpriteRenderer>();
                leftName = chemName;
                sr.color = chemicals[chemName];
            }
        }
    }

    public bool CheckCorrect()
    {
        string a = inChem1.text;
        string b = inChem2.text;
        string c = inResult.text;


        var tpl = Tuple.Create(leftName, rightName);

        return (a == results[tpl].Item2.ToString() && b == results[tpl].Item3.ToString() && c == results[tpl].Item4.ToString());

    }

    public void TrashClick(bool right)
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;

        if (Math.Abs(player_y + 250) < threshold)
        {
            if (right && Math.Abs(player_x + 100) < threshold)
            {
                rightHand.SetActive(false);
                rightName = "X";
            }
            else if (!right && Math.Abs(player_x - 70) < threshold)
            {
                leftHand.SetActive(false);
                leftName = "X";
            }
        }
    }

    public void BalanceClick()
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;

        if (player_y < threshold && 6.5 < player_x && leftHand.activeSelf && rightHand.activeSelf && tutorial_balance)
        {
            if (CheckChemicals())
            {
                balancingScreen.SetActive(true);
                leftHand.SetActive(false);
                rightHand.SetActive(false);

                balancing = true;

                var tpl = Tuple.Create(leftName, rightName);
                chem1.text = leftName;
                chem2.text = rightName;
                result.text = results[tpl].Item1;
            }

        }

    }

    public bool CheckChemicals()
    {
        return results.ContainsKey(Tuple.Create(leftName, rightName));
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

        if (player_x < -5.5 && player_y > -1 && tutorial_customer)
        {
            if (order == rightName)
            {
                if (num == 0)
                    customer1.SetActive(false);

                rightHand.SetActive(false);

            }
        }

    }

    public void NextClick()
    {
        tutorialText.text = tutorialList[num];
        num++;
    }

}

