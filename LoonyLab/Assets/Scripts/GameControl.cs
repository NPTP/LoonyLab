using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameControl : MonoBehaviour
{
    public GameObject player;
    public GameObject leftHand;
    public GameObject rightHand;
    public int threshold;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
