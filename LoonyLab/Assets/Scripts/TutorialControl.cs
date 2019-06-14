using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class TutorialControl : MonoBehaviour
{
    public GameObject player; // Player object.
    public GameObject Hand; // Player's hand (inventory).
    public GameObject balancingScreen; // Screen that appears when balancing chemicals.
    public GameObject customer1; // First customer object. 
    public GameObject customer2; // Second customer object.

    public Text chem1; //Text used in balancing screen.
    public Text chem2;
    public Text result;
    public Text chem1Total;
    public Text chem2Total;
    public Text resultTotal;
    public Text customer1Text;
    public Text customer2Text;
    public Text tutorialText;

    public Text fix2; //Adding subscripts to element labels.

    public int threshold; //Distance player can be from objects in order to interact.

    private Chemical InHand; // Chemical player is currently holding. 


    private char sub_2 = (char)8322; // Subscript 2
    private char sub_3 = (char)8323; // Subscript 3


    private List<Chemical> chemicals = new List<Chemical>(); // List of chemicals player can pick up. 
    private List<string> orders = new List<string>(); // List of orders customers will make throughout level. 

    private Dictionary<Tuple<Chemical, Chemical>, Chemical> results = new Dictionary<Tuple<Chemical, Chemical>, Chemical>(); // Dictionary of possible reactions player can complete.

    private BalancingStation balanceStn = new BalancingStation(); // Balancing station object. Used to complete reactions.

    private bool balancing = false;
    private bool tutorial_balance = false;
    private bool tutorial_customer = false;
    private bool tutorial_finished = false;

    public GameObject customerArrow;
    public GameObject elementArrow;
    public GameObject balanceArrow;
    public GameObject giantText;
    public Text giantName;

    public GameObject nextButton; private int num = 0;
    private List<String> tutorialList= new List<String>();

    // Start is called before the first frame update
    void Start()
    {
        Chemical fe = new Chemical("Fe", 1, 0, false, Color.blue, "Fe");
        Chemical cl2 = new Chemical("Cl" + sub_2, 2, 0, false, Color.red, "Cl");

        chemicals.Add(fe);
        chemicals.Add(cl2);

        results[Tuple.Create(fe, cl2)] = new Chemical("FeCl" + sub_3, 1, 3, true, Color.yellow, "FeCl");

        orders.Add("FeCl" + sub_3);

        fix2.text = "Cl" + sub_2;

        tutorialList.Add("Here we are! This lab is where you're going to be making all of the chemicals and delivering the orders!");
        tutorialList.Add("And here's your first customer! Since this is your first day, I'll lead you through the steps to make his order.");
        tutorialList.Add("This customer is ordering FeCl" + sub_3 + "! I know those symbols can seem confusing, so let's break the chemical down.");
        tutorialList.Add("FeCl" + sub_3 + " can also be called Iron Chloride. This is because it is made up of two elements:");
        tutorialList.Add("Iron (Fe) and Chlorine (Cl). So when you combine them together, you create a compound called Iron Chloride.");
        tutorialList.Add("You may be wondering what the little 3 under the chlorine symbol means. That subscript tells you how many chlorine molecules are needed to make the compound.");
        tutorialList.Add("So in this case, to make one Iron Chloride molecule, you will need one Iron molecule and three Chlorine molecules. ");
        tutorialList.Add("This is where you come in! Let's get started by going over to the chemical cabinet and picking up an Iron molecule.");
        tutorialList.Add("Awesome job! Now that you have the element you need, let's bring it over to the balancing station.");
        tutorialList.Add("You can see that the Iron has been added to the station. Now let's go get some Chlorine and bring it over.");
        tutorialList.Add("You can see that the Iron has been added to the station. Now let's go get some Chlorine and bring it over.");
        tutorialList.Add("Nice work! Looking at the station you can see that you now have a total of 1 Iron molecule and 2 Chlorine molecules added.");
        tutorialList.Add("But the equation isn't balanced yet! Looking at the products side of the equation, you can see that one Iron Chloride compound requires 1 Fe molecule and 3 Cl molecules.");
        tutorialList.Add("In the 18th century, Antoine Lavoisier demonstrated a fundamental law of chemistry, the Law of Conservation of Mass (or LCM)");
        tutorialList.Add("The masses of all of the reacting substances must equal the masses\nof all the substances produced in a chemical reaction");
        tutorialList.Add("In a chemical reaction, nothing can be created or destroyed. All atoms present before a reaction starts must be present in the exact same amounts after it is over.");
        tutorialList.Add("The arrangement of the atoms may be different, but not their number.");
        tutorialList.Add("This means that if your product uses three Chlorine molecules, your reactants must also have three Chlorine molecules.");
        tutorialList.Add("It's up to you to balance the equation by determining how many groups of Iron and Chlorine molecules you need in your reactants.");
        tutorialList.Add("Since this is still your first day, I'll help you out. Let's start by getting one more Chlorine element and adding it to the station.");
        tutorialList.Add("Since this is still your first day, I'll help you out. Let's start by getting one more Chlorine element and adding it to the station.");
        tutorialList.Add("Awesome! Now you have a total of 4 Chlorine molecules. That's 1 too many to make only one Iron Chloride compound, so let's make two compounds instead!");
        tutorialList.Add("You're missing exactly 2 Chlorine molecules to have enough to make two Iron Chloride compounds. Go get one more Cl" + sub_2 + " molecule and bring it to the station.");
        tutorialList.Add("You're missing exactly 2 Chlorine molecules to have enough to make two Iron Chloride compounds. Go get one more Cl" + sub_2 + " molecule and bring it to the station.");
        tutorialList.Add("Perfect! Now we have the right number of Chlorine molecules, but are still missing something...");
        tutorialList.Add("If we're going to make two Iron Chloride compounds, that means we will need 2 Fe molecules, but right now we only have one.");
        tutorialList.Add("You're almost finished the reaction! Go get one more Fe element and bring it to the balancing station.");
        tutorialList.Add("You're almost finished the reaction! Go get one more Fe element and bring it to the balancing station.");
        tutorialList.Add("Nice going, you're a natural at this! Now you can take your finished compound and give it to the customer.");
        tutorialList.Add("You did some great science today! Now it's time for you to test your skills and try completing tomorrow's orders on your own.");

        GenerateCustomers();
        NextClick();
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
        }
    }


    public void ChemicalClick(int chemNum)
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;
        Chemical chem = chemicals[chemNum];

        if (player_y > 1 && player_x < 0 && !balancing)
        {
            if (!Hand.activeSelf)
            {
                if ((num == 8 && chemNum == 0) || (num == 20 && chemNum == 1) || (num == 10 && chemNum == 1) || (num == 23 && chemNum == 1) || (num == 27 && chemNum == 0))
                {
                    Hand.SetActive(true);
                    SpriteRenderer sr = Hand.GetComponent<SpriteRenderer>();
                    InHand = chem;
                    sr.color = InHand.Colour;
                    NextClick();
                }
            }
        }
    }


    public void BalanceClick()
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;

        if (player_y < threshold && 6.5 < player_x && Hand.activeSelf && tutorial_balance)
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
            
            NextClick();

        }
        balancingScreen.SetActive(true);
        balancing = true;
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
            string text1 = "Needed: " + (balanceStn.Product.Subscript1 * goal).ToString() + " " + balanceStn.Reactant1.SingleName;
            string text2 = " " + (balanceStn.Product.Subscript2 * goal).ToString() + " " + balanceStn.Reactant2.SingleName;

            resultTotal.text = text1 + text2 + " Missing " + missing1.ToString() + " " + balanceStn.Reactant1.SingleName + " and " + missing2.ToString() + " " + balanceStn.Reactant2.SingleName;
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

        if (player_x < -5.5 && player_y > -1 && tutorial_customer && !balancing)
        {
            if (order == InHand.Name)
            {
                if (num == 0)
                    customer1.SetActive(false);
                Hand.SetActive(false);
                NextClick();

            }
        }

    }

    public void NextClick()
    {
        if (num == 1)
        {
            customerArrow.SetActive(true);
        }
        if (num == 2)
        {
            giantText.SetActive(true);
            giantName.text = "FeCl" + sub_3;
        }
        if (num == 7)
        {
            giantText.SetActive(false);
            customerArrow.SetActive(false);
            elementArrow.SetActive(true);
            nextButton.SetActive(false);

        }
        if (num == 8)
        {
            elementArrow.SetActive(false);
            tutorial_balance = true;
            balanceArrow.SetActive(true);
        }
        if (num == 11)
        {
            nextButton.SetActive(true);
        }
        if (num == 19)
        {
            nextButton.SetActive(false);
            elementArrow.SetActive(true);
            balanceArrow.SetActive(false);
        }
        if (num == 21)
        {
            nextButton.SetActive(true);
            elementArrow.SetActive(false);
        }
        if (num == 22)
        {
            nextButton.SetActive(false);
        }
        if (num == 24)
        {
            nextButton.SetActive(true);
        }
        if (num == 26)
        {
            nextButton.SetActive(false);
        }
        if (num == 28)
        {
            tutorial_customer = true;
            customerArrow.SetActive(true);
        }
        if (num == 29)
        {
            customerArrow.SetActive(false);
            nextButton.SetActive(true);
        }
        if (num == 30)
        {
            SceneManager.LoadScene("Level1");
        }

        tutorialText.text = tutorialList[num];

        num++;
    }

    public void CloseScreen()
    {
        balancingScreen.SetActive(false);
        balancing = false;
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

