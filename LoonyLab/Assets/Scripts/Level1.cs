using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using static StaticVars;

public class Level1 : MonoBehaviour
{

    public GameObject player; // Player object.
    public GameObject Hand; // Player's hand (inventory).
    public GameObject balancingScreen; // Screen that appears when balancing chemicals.
    public GameObject customer1; // First customer object. 
    public GameObject endScreen; // End of level screen.
    public GameObject tutorial;
    public GameObject nextButton;

    public Text chem1; //Text used in balancing screen.
    public Text chem2;
    public Text result;
    public Text chem1Total;
    public Text chem2Total;
    public Text resultTotal;
    public Text customer1Text;
    public Text tutorialText;
    public Text balanceHoverText;
    public Text ordersDone;

    public GameObject CHover;
    public GameObject O2Hover;
    public GameObject H2Hover;
    public GameObject BalanceHover;
    public GameObject CustomerHover;
    public GameObject TrashHover;

    public Text fix2; //Adding subscripts to element labels.
    public Text fix3;

    private Chemical InHand; // Chemical player is currently holding. 

    private List<string> TutorialList = new List<string>();
    public List<Sprite> molecules;

    private bool balancing = false;
    private bool tutorial_on = false;
    private bool tutorial_finished = false;

    private int num = 0;
    private int ordersCompleted = 0;


    private char sub_2 = (char)8322; // Subscript 2
    private char sub_3 = (char)8323; // Subscript 3


    private List<Chemical> chemicals = new List<Chemical>(); // List of chemicals player can pick up. 
    private List<string> orders = new List<string>(); // List of orders customers will make throughout level. 

    private Dictionary<Tuple<Chemical, Chemical>, Chemical> results = new Dictionary<Tuple<Chemical, Chemical>, Chemical>(); // Dictionary of possible reactions player can complete.

    private BalancingStation balanceStn = new BalancingStation(); // Balancing station object. Used to complete reactions.

    // Start is called before the first frame update
    void Start()
    {
        // Create chemicals used in the level.

        Chemical c = new Chemical("C", 1, 0, false, molecules[0], "C");
        Chemical o2 = new Chemical("O" + sub_2, 2, 0, false, molecules[5], "O");
        Chemical h2 = new Chemical("H" + sub_2, 2, 0, false, molecules[2], "H");

        // Load possible reactions into dictionary.

        results[Tuple.Create(c, o2)] = new Chemical("CO" + sub_2, 1, 2, true, molecules[7], "CO");
        results[Tuple.Create(o2, c)] = new Chemical("CO" + sub_2, 2, 1, true, molecules[7], "CO");
        results[Tuple.Create(h2, o2)] = new Chemical("H" + sub_2 + "O", 2, 1, true, molecules[7], "HO");
        results[Tuple.Create(o2, h2)] = new Chemical("H" + sub_2 + "O", 1, 2, true, molecules[7], "HO");

        // Add chemicals to list.

        chemicals.Add(c);
        chemicals.Add(o2);
        chemicals.Add(h2);

        // Load orders for the level.

        orders.Add("CO" + sub_2);
        orders.Add("H" + sub_2 + "O");
        orders.Add("CO" + sub_2);

        // Fix subscripts.

        fix2.text = "O" + sub_2;
        fix3.text = "H" + sub_2;

        InHand = c;

        // Load customers for the level and have new ones appear as player completes orders.

        GenerateCustomers();
        InvokeRepeating("GenerateCustomers", 2.0f, 2.0f);

        TutorialList.Add("The left side of the equation shows the total # of atoms you've added to the station. The right side shows how many more you need for the equation to be balanced.");
        TutorialList.Add("4 Hydrogen atoms are needed, but you only have added 2! Go pick up another Hydrogen molecule and add it to the station!");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (tutorial_on && nextButton.activeSelf)
            {
                NextClick();
            }
            else if (endScreen.activeSelf)
            {
                NextLevel();
            }
            else if (balancing)
            {
                CloseScreen();
            }
            else
            {
                if (CheckBalance())
                    BalanceClick();
                if (CheckFe())
                    ChemicalClick(0);
                if (CheckCl2())
                    ChemicalClick(1);
                if (CheckO2())
                    ChemicalClick(2);
                if (CheckCustomer())
                    CustomerClick();
                if (CheckTrash())
                    TrashClick();
            }
        }
        else if (Input.GetKeyDown(KeyCode.R) && balancing)
        {
            ClearScreen();
        }

        if (CheckBalance() && Hand.activeSelf && !InHand.Product)
        {
            BalanceHover.SetActive(true);
            balanceHoverText.text = "Press SPACE to add item to balancing station.";
        }
        else if (CheckBalance())
        {
            BalanceHover.SetActive(true);
            balanceHoverText.text = "Press SPACE to view items in balancing station.";
        }
        else
            BalanceHover.SetActive(false);


        if (CheckFe() && !Hand.activeSelf)
            CHover.SetActive(true);
        else
            CHover.SetActive(false);

        if (CheckCl2() && !Hand.activeSelf)
            O2Hover.SetActive(true);
        else
            O2Hover.SetActive(false);

        if (CheckCustomer() && Hand.activeSelf)
        { 
            if (InHand.Product)
                CustomerHover.SetActive(true);
        }
        else
            CustomerHover.SetActive(false);
        if (CheckO2() && !Hand.activeSelf)
            H2Hover.SetActive(true);
        else
            H2Hover.SetActive(false);
        if (CheckTrash() && Hand.activeSelf)
            TrashHover.SetActive(true);
        else
            TrashHover.SetActive(false);
    }

    public void GenerateCustomers()
    {
        // Load new customers if there are empty spaces available.

        if (orders.Count != 0) // Check if there are still orders left to complete. 
        {
            if (!customer1.activeSelf) // Check if customer 1 space is available.
            {
                customer1.SetActive(true);
                customer1Text.text = orders[0]; // Load next order in line. 
                orders.RemoveAt(0); // Remove loaded order from list. 
            }
        }
    }


    public void ChemicalClick(int chemNum)
    {
        // Allow player to pick up selected chemical if player is not already holding something. 

        Chemical chem = chemicals[chemNum];
            if ((!Hand.activeSelf && !tutorial.activeSelf) || (chemNum == 2 && num == 2))
            {
                chemNumPublic = chemNum;
                chemClickEvent = true;
                FindObjectOfType<AudioManager>().Play("cabinetUse");
                Hand.SetActive(true);
                SpriteRenderer sr = Hand.GetComponent<SpriteRenderer>();
                InHand = chem;
                sr.sprite = InHand.Colour;
            }
        
    }


    public void TrashClick()
    { 
        // Allow player to throwout whatever chemical they are currently holding. 

        Hand.SetActive(false);
        InHand = chemicals[0];
        FindObjectOfType<AudioManager>().Play("trashUse");
        trashClickEvent = true;
            
        
    }

    public void BalanceClick()
    { 

        if (Hand.activeSelf && !InHand.Product && (balanceStn.Reactant1 == null || balanceStn.Reactant2 == null || InHand == balanceStn.Reactant1 || InHand == balanceStn.Reactant2))
        {
            balancingScreen.SetActive(true);
            Hand.SetActive(false);
            FindObjectOfType<AudioManager>().Play("addToBalStation");

            if (balanceStn.Reactant1 == null)
            {
                balanceStn.Reactant1 = InHand;
                balanceStn.QuantityR1 = 1;
                chem1.text = "1  " + InHand.Name;
                chem1Total.text = "Total: " + InHand.Subscript1.ToString() + " " + InHand.SingleName + " Atoms";

            }
            else
            {
                if (balanceStn.Reactant1 == InHand)
                {
                    balanceStn.QuantityR1++;
                    chem1.text = balanceStn.QuantityR1.ToString() + " " + InHand.Name;
                    chem1Total.text = "Total: " + (balanceStn.Reactant1.Subscript1 * balanceStn.QuantityR1).ToString() + " " + InHand.SingleName + " Atoms";
                    UpdateBalanced();
                }
                else
                {
                    if (balanceStn.Reactant2 == null)
                    {
                        balanceStn.Reactant2 = InHand;
                        balanceStn.QuantityR2 = 1;
                        chem2.text = "1  " + InHand.Name;
                        chem2Total.text = "Total: " + InHand.Subscript1 + " " + InHand.SingleName + " Atoms";

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
                            chem2Total.text = "Total: " + (InHand.Subscript1 * balanceStn.QuantityR2).ToString() + " " + InHand.SingleName + " Atoms";
                            UpdateBalanced();
                        }
                        else
                        {
                            balancingScreen.SetActive(false);
                            Hand.SetActive(true);
                        }
                    }
                }
                if (balanceStn.Reactant1 != null && balanceStn.Reactant2 != null)
                {
                    if (!results.ContainsKey(Tuple.Create(balanceStn.Reactant1, balanceStn.Reactant2)))
                    {
                        result.text = "Wrong ingredients!";
                        resultTotal.text = "Reset balancing station and try again!";
                        FindObjectOfType<AudioManager>().Play("incorrectIngredient");
                    }
                }
            }
        }
        if ((balanceStn.Reactant1 == chemicals[2] && balanceStn.Reactant2 == chemicals[1] || balanceStn.Reactant1 == chemicals[1] && balanceStn.Reactant2 == chemicals[2]) && !tutorial_finished && orders.Count < 2)
        {
            tutorial_on = true;
            tutorial.SetActive(true);

        }
        FindObjectOfType<AudioManager>().Play("viewBalStation");
        balancingScreen.SetActive(true);
        player.GetComponent<PlayerController>().balancing = true;
        balancing = true;
        tutorialText.text = TutorialList[0];
        if (num == 2)
        {
            tutorial.SetActive(false);
            tutorial_on = false;
        }
        num = 1;
    }

    public void NextClick()
    {
        if (num == 1)
        {
            nextButton.SetActive(false);
            tutorial_on = false;
            tutorial_finished = true;
        }
        tutorialText.text = TutorialList[num];
        num++;
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
            sr.sprite = InHand.Colour;
            FindObjectOfType<AudioManager>().Play("successBalance");
            deliveryLightsOn = true;
        }
        else
        {
            // Not balanced
            string text1 = "Needed: " + (balanceStn.Product.Subscript1 * goal).ToString() + " " + balanceStn.Reactant1.SingleName;
            string text2 = " " + (balanceStn.Product.Subscript2 * goal).ToString() + " " + balanceStn.Reactant2.SingleName;

            resultTotal.text = text1 + text2 + " Missing " + missing1.ToString() + " " + balanceStn.Reactant1.SingleName + " and " + missing2.ToString() + " " + balanceStn.Reactant2.SingleName;
        }
    }


    public void CustomerClick()
    {

        string order = customer1Text.text;

        if (order == InHand.Name) {
                customer1.SetActive(false);
            customer1Text.text = "";
                ordersCompleted++;
                ordersDone.text = ordersCompleted.ToString() + "/3";

                Hand.SetActive(false);
                if (orders.Count == 0) {
                    EndLevel();
                }
                deliveryLightsOn = false;
                conveyorBeltEvent = true;
                FindObjectOfType<AudioManager>().Play("deliverCompound");
            }
    }

    public void EndLevel()
    {
        chemNumPublic = 99;
        chemClickEvent = false;
        trashClickEvent = false;
        deliveryLightsOn = false;
        conveyorBeltEvent = false;
        FindObjectOfType<AudioManager>().Play("winLevel");
        endScreen.SetActive(true);
        player.GetComponent<PlayerController>().balancing = true;
    }

    public void CloseScreen()
    {
        balancingScreen.SetActive(false);
        balancing = false;
        player.GetComponent<PlayerController>().balancing = false;
        if (Hand.activeSelf)
            ClearScreen();
    }

    public void ClearScreen()
    {
        FindObjectOfType<AudioManager>().Play("clearBalStation");
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

    public void NextLevel()
    {
        SceneManager.LoadScene("Level2");
    }

    public bool CheckBalance()
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;

        if (player_y > -1.13 && player_y < 1.55)
        {
            if (player_x > 3.86)
                return true;
        }
        return false;
    }

    public bool CheckCustomer()
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;

        if (player_y > -0.8 && player_y < 0.6)
        {
            if (player_x < -3.26)
                return true;
        }
        return false;
    }

    public bool CheckFe()
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;

        if (player_y > 0.82)
        {
            if (player_x > 2.1 && player_x < 2.9)
                return true;
        }
        return false;
    }

    public bool CheckCl2()
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;

        if (player_y < -1.2)
        {
            if (player_x > 2.1 && player_x < 2.9)
                return true;
        }
        return false;
    }

    public bool CheckO2()
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;

        if (player_y < -1.2)
        {
            if (player_x > -1.91 && player_x < -0.89)
                return true;
        }
        return false;
    }

    public bool CheckTrash()
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;

        if (player_y > 0.82)
        {
            if (player_x > -0.91 && player_x < -0.21)
                return true;
        }
        return false;
    }

}







