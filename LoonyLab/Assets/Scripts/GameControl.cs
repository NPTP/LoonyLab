using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameControl : MonoBehaviour
{
    public GameObject player;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject balancingScreen;
    public GameObject customer1;

    public Text chem1;
    public Text chem2;
    public Text result;

    public InputField inChem1;
    public InputField inChem2;
    public InputField inResult;

    public int threshold;

    private bool balancing = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (balancing)
        {
            if (inChem1.text.Equals("2") && inChem2.text.Equals("3") && inResult.text.Equals("2"))
            {
                balancing = false;
                balancingScreen.SetActive(false);
                rightHand.SetActive(true);
                SpriteRenderer sr = rightHand.GetComponent<SpriteRenderer>();
                sr.color = Color.magenta;
            }
        } 
    }


    public void ChemicalClick(bool right)
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;

        if (Math.Abs(player_y - 250) < threshold)
        {
            if (right && Math.Abs(player_x + 175) < threshold)
                rightHand.SetActive(true);
            else if (!right && Math.Abs(player_x + 115) < threshold)
                leftHand.SetActive(true);
        }
    }

    public void TrashClick(bool right)
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;

        if (Math.Abs(player_y + 250) < threshold)
        {
            if (right && Math.Abs(player_x + 100) < threshold)
                rightHand.SetActive(false);
            else if (!right && Math.Abs(player_x - 70) < threshold)
                leftHand.SetActive(false);
        }
    }

    public void BalanceClick()
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;
        if (player_y < threshold && 6.5 < player_x && leftHand.activeSelf && rightHand.activeSelf)
        {

            balancingScreen.SetActive(true);
            leftHand.SetActive(false);
            rightHand.SetActive(false);

            balancing = true;

            chem1.text = "Fe";
            chem2.text = "Cl2";
            result.text = "FeCl3";

        }

        
    }

    public void CustomerClick()
    {
        float player_x = player.transform.position.x;
        float player_y = player.transform.position.y;

        if (!leftHand.activeSelf && rightHand.activeSelf && player_x < -5.5 && player_y > 1.5)
        {
            rightHand.SetActive(false);
            customer1.SetActive(false);
        }

    }

}
