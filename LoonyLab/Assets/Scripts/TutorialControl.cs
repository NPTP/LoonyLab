using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using static StaticVars;

public class TutorialControl : MonoBehaviour
{

    public GameObject player; // Player object.
    public GameObject Hand; // Player's hand (inventory).
    public GameObject balancingScreen; // Screen that appears when balancing chemicals.
    public GameObject customer1; // First customer object.
    public GameObject trashArrow;

    public GameObject CHover;
    public GameObject O2Hover;
    public GameObject BalanceHover;
    public GameObject CustomerHover;

    public List<Sprite> molecules;

    public Text chem1; //Text used in balancing screen.
    public Text chem2;
    public Text result;
    public Text chem1Total;
    public Text chem2Total;
    public Text resultTotal;
    public Text customer1Text;
    public Text customer2Text;
    public Text tutorialText;


    public List<GameObject> sec1;

    public List<GameObject> sec2;

    public List<GameObject> sec3;

    public Sprite c_sprite;
    public Sprite o2_sprite;

    public List<Sprite> co2_sprites;
   


    public Text fix2; //Adding subscripts to element labels.

    private Chemical InHand; // Chemical player is currently holding. 


    private char sub_2 = (char)8322; // Subscript 2
    private char sub_3 = (char)8323; // Subscript 3


    private List<Chemical> chemicals = new List<Chemical>(); // List of chemicals player can pick up. 
    private List<string> orders = new List<string>(); // List of orders customers will make throughout level. 

    private Dictionary<Tuple<Chemical, Chemical>, Chemical> results = new Dictionary<Tuple<Chemical, Chemical>, Chemical>(); // Dictionary of possible reactions player can complete.

    private BalancingStation balanceStn = new BalancingStation(); // Balancing station object. Used to complete reactions.

    public bool balancing = false;
    private bool tutorial_balance = false;
    private bool tutorial_customer = false;
    private bool space_next = true;

    public GameObject customerArrow;
    public GameObject CArrow;
    public GameObject O2Arrow;
    public GameObject balanceArrow;
    public GameObject giantText;
    public Text giantName;

    public GameObject nextButton;
    public GameObject WASDbutton;
    private int num = 0;
    private List<String> tutorialList= new List<String>();

    // Start is called before the first frame update
    void Start()
    {
        Chemical c = new Chemical("C", 1, 0, false, new List<Sprite> { c_sprite }, "C");
        Chemical o2 = new Chemical("O" + sub_2, 2, 0, false, new List<Sprite> { o2_sprite }, "O");

        chemicals.Add(c);
        chemicals.Add(o2);

        results[Tuple.Create(c, o2)] = new Chemical("CO" + sub_2, 1, 2, true, co2_sprites, "CO");

        orders.Add("CO" + sub_2);

        fix2.text = "O" + sub_2;

        tutorialList.Add("Welcome to the loony lab! This is where you're going to be making all of the chemicals and delivering the orders!");
        tutorialList.Add("And here's your first customer! Since this is your first day, I'll lead you through the steps to make her order.");
        tutorialList.Add("This customer is ordering CO" + sub_2 + "! It is made up of Carbon atoms and Oxygen atoms.");
        tutorialList.Add("This is where you come in! Let's get started by going over to the chemical cabinet and picking up a Carbon molecule.");
        tutorialList.Add("Awesome job! Now that you have the element you need, let's bring it over to the balancing station.");
        tutorialList.Add("You can see that the Carbon has been added to the station. Now let's go get some Oxygen and bring it over.");
        tutorialList.Add("You can see that the Carbon has been added to the station. Now let's go get some Oxygen and bring it over.");
        tutorialList.Add("Nice going, you're a natural at this! Now you can take your finished compound and give it to the customer.");
        tutorialList.Add("Awesome work! Going forward, if you ever pick up the wrong element, you can throw it away over here.");
        tutorialList.Add("You did some great science today! Now it's time for you to test your skills and try completing tomorrow's orders on your own.");
        tutorialList.Add("");

        NextClick();
        player.GetComponent<PlayerController>().balancing = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (space_next)
            {
                NextClick();
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
                if (CheckCustomer())
                    CustomerClick(0);
            }
        }
        if (CheckBalance() && Hand.activeSelf && !InHand.Product)
            BalanceHover.SetActive(true);
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

        if (CheckCustomer() && InHand.Product && Hand.activeSelf)
            CustomerHover.SetActive(true);
        else
            CustomerHover.SetActive(false);
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

        if (!balancing)
        {
            if (!Hand.activeSelf)
            {
                if ((chemNum == 0 && num == 4) || (chemNum == 1 && num == 6))
                {
                    chemNumPublic = chemNum;
                    chemClickEvent = true;
                    FindObjectOfType<AudioManager>().Play("cabinetUse");
                    Hand.SetActive(true);
                    SpriteRenderer sr = Hand.GetComponent<SpriteRenderer>();
                    InHand = chem;
                    sr.sprite = InHand.Colour[0];
                    NextClick();
                }
            }
        }
    }


    public void BalanceClick()
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;

        if (Hand.activeSelf && tutorial_balance)
        {
            if (!InHand.Product)
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
                    if (balanceStn.Reactant1 == InHand && CheckAddItem())
                    {
                        balanceStn.QuantityR1++;
                        chem1.text = balanceStn.QuantityR1.ToString() + " " + InHand.Name;
                        chem1Total.text = "Total: " + (balanceStn.Reactant1.Subscript1 * balanceStn.QuantityR1).ToString() + " " + InHand.SingleName + " Atoms";
                        if (balanceStn.Product != null && balanceStn.Reactant2 != null)
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
                            if (balanceStn.Reactant2 == InHand && CheckAddItem())
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
                }

                NextClick();

            }
        }
        DisplayAtomImages();
        FindObjectOfType<AudioManager>().Play("viewBalStation");
        balancingScreen.SetActive(true);
        balancing = true;
        player.GetComponent<PlayerController>().balancing = true;
    }

    public void UpdateBalanced()
    {

        int num1 = (int)Math.Ceiling(balanceStn.QuantityR1 * balanceStn.Reactant1.Subscript1 / (float)balanceStn.Product.Subscript1);
        int num2 = (int)Math.Ceiling(balanceStn.QuantityR2 * balanceStn.Reactant2.Subscript1 / (float)balanceStn.Product.Subscript2);
        int goal = Math.Max(num1, num2);
        result.text = "_ " + balanceStn.Product.Name;
        int missing1 = balanceStn.Product.Subscript1 * goal - balanceStn.QuantityR1 * balanceStn.Reactant1.Subscript1;
        int missing2 = balanceStn.Product.Subscript2 * goal - balanceStn.QuantityR2 * balanceStn.Reactant2.Subscript1;

        if (missing1 == 0 && missing2 == 0)
        {
            // Balanced 
            result.text = goal.ToString() + " " + balanceStn.Product.Name;
            resultTotal.text = "Balanced! You have finished the reaction for creating " + balanceStn.Product.Name + "!";
            Hand.SetActive(true);
            InHand = balanceStn.Product;

            SpriteRenderer sr = Hand.GetComponent<SpriteRenderer>();
            sr.sprite = molecules[7];

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


    public void CustomerClick(int num)
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;
        string order = "";
        if (num == 0)
        {
            order = customer1Text.text;
        }

        if (tutorial_customer && !balancing && player_x < -2.5)
        {
            if (order == InHand.Name)
            {
                if (num == 0)
                    customer1.SetActive(false);
                Hand.SetActive(false);
                deliveryLightsOn = false;
                conveyorBeltEvent = true;
                FindObjectOfType<AudioManager>().Play("deliverCompound");
                NextClick();
                customer1Text.text = "";
            }
        }

    }

    public void NextClick()
    {
        if (num == 1)
        {
            customerArrow.SetActive(true);
            GenerateCustomers();
        }
        if (num == 2)
        {
            giantText.SetActive(true);
            giantName.text = "CO" + sub_2;
        }
        if (num == 3)
        {
            giantText.SetActive(false);
            customerArrow.SetActive(false);
            CArrow.SetActive(true);
            nextButton.SetActive(false);
            WASDbutton.SetActive(true);
            space_next = false;
            player.GetComponent<PlayerController>().balancing = false;

        }
        if (num == 4)
        {
            CArrow.SetActive(false);
            tutorial_balance = true;
            balanceArrow.SetActive(true);
        }
        if (num == 5)
        {
            balanceArrow.SetActive(false);
            O2Arrow.SetActive(true);
            nextButton.SetActive(true);
            WASDbutton.SetActive(false);
        }
        if (num == 6)
        {
            O2Arrow.SetActive(false);
            balanceArrow.SetActive(true);
            nextButton.SetActive(false);
            WASDbutton.SetActive(true);
        }
        if (num == 7)
        {
            tutorial_customer = true;
            customerArrow.SetActive(true);
            balanceArrow.SetActive(false);
            nextButton.SetActive(true);
            WASDbutton.SetActive(false);
        }
        if(num == 8)
        {
            customerArrow.SetActive(false);
            nextButton.SetActive(true);
            WASDbutton.SetActive(false);
            space_next = true;
            trashArrow.SetActive(true);
        }
        if(num == 9)
        {
            trashArrow.SetActive(false);
        }
        if (num == 10)
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
        player.GetComponent<PlayerController>().balancing = false;
        nextButton.SetActive(false);
        WASDbutton.SetActive(true);
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

        ResetAtomImages();
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

    public void DisplayAtomImages()
    {
        if (balanceStn.Reactant1 != null)
        {
            for (int i = 0; i < balanceStn.QuantityR1; i++)
            {
                sec1[i].SetActive(true);
                Image sr = sec1[i].GetComponent<Image>();
                sr.sprite = balanceStn.Reactant1.Colour[0];

            }
        }

        if (balanceStn.Reactant2 != null)
        {
            for (int i = 0; i < balanceStn.QuantityR2; i++)
            {
                sec2[i].SetActive(true);
                Image sr = sec2[i].GetComponent<Image>();
                sr.sprite = balanceStn.Reactant2.Colour[0];

            }
        }



        if (results.ContainsKey(Tuple.Create(balanceStn.Reactant1, balanceStn.Reactant2)))
        {
            int num1 = (int)Math.Ceiling(balanceStn.QuantityR1 * balanceStn.Reactant1.Subscript1 / (float)balanceStn.Product.Subscript1);
            int num2 = (int)Math.Ceiling(balanceStn.QuantityR2 * balanceStn.Reactant2.Subscript1 / (float)balanceStn.Product.Subscript2);
            int total_needed = Math.Max(num1, num2);

            int remaining_r1 = balanceStn.QuantityR1 * balanceStn.Reactant1.Subscript1;
            int remaining_r2 = balanceStn.QuantityR2 * balanceStn.Reactant2.Subscript1;

            for (int i = 0; i < total_needed; i++)
            {
                sec3[i].SetActive(true);
                Image sr2 = sec3[i].GetComponent<Image>();
                if (remaining_r1 >= balanceStn.Product.Subscript1 && remaining_r2 >= balanceStn.Product.Subscript2)
                {
                    sr2.sprite = balanceStn.Product.Colour[balanceStn.Product.Colour.Count - 1];
                    remaining_r1 -= balanceStn.Product.Subscript1;
                    remaining_r2 -= balanceStn.Product.Subscript2;
                }
                else if (remaining_r1 >= balanceStn.Product.Subscript1)
                {
                    int partial_sprite = (balanceStn.Product.Subscript2 + 1) * balanceStn.Product.Subscript1 + remaining_r2;
                    sr2.sprite = balanceStn.Product.Colour[partial_sprite];
                    remaining_r1 -= balanceStn.Product.Subscript1;
                    remaining_r2 = 0;
                }
                else if (remaining_r2 >= balanceStn.Product.Subscript2)
                {
                    int partial_sprite = remaining_r1 * (balanceStn.Product.Subscript2 + 1) + balanceStn.Product.Subscript2;
                    sr2.sprite = balanceStn.Product.Colour[partial_sprite];
                    remaining_r2 -= balanceStn.Product.Subscript2;
                    remaining_r1 = 0;
                }
                else
                {
                    int partial_sprite = remaining_r1 * (balanceStn.Product.Subscript2 + 1) + remaining_r2;
                    sr2.sprite = balanceStn.Product.Colour[partial_sprite];
                }

            }



        }

    }

    public void ResetAtomImages()
    {
        for (int i = 0; i < 4; i++)
        {
            sec1[i].SetActive(false);
            sec2[i].SetActive(false);
            sec3[i].SetActive(false);
        }
    }

    public bool CheckAddItem()
    {
        if (InHand == balanceStn.Reactant1 && balanceStn.Reactant2 != null)
        {
            int num1 = (int)Math.Ceiling((balanceStn.QuantityR1 + 1) * balanceStn.Reactant1.Subscript1 / (float)balanceStn.Product.Subscript1);
            int num2 = (int)Math.Ceiling(balanceStn.QuantityR2 * balanceStn.Reactant2.Subscript1 / (float)balanceStn.Product.Subscript2);
            int goal = Math.Max(num1, num2);

            return (balanceStn.QuantityR1 < 4 && goal < 5);
        }
        else if (InHand == balanceStn.Reactant2)
        {
            int num1 = (int)Math.Ceiling(balanceStn.QuantityR1 * balanceStn.Reactant1.Subscript1 / (float)balanceStn.Product.Subscript1);
            int num2 = (int)Math.Ceiling((balanceStn.QuantityR2 + 1) * balanceStn.Reactant2.Subscript1 / (float)balanceStn.Product.Subscript2);
            int goal = Math.Max(num1, num2);

            return (balanceStn.QuantityR2 < 4 && goal < 5);
        }
        return true;

    }


}

