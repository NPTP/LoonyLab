using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial1 : MonoBehaviour
{
    private List<string> tutorial = new List<string>();
    private int tutorial_num = 0;

    public Text tutorial_text;

    // Start is called before the first frame update
    void Start()
    {
        tutorial.Add("Hello! Welcome to the Loony Lab! I'm glad you're here because we could definitely use some help with our opening day...");
        tutorial.Add("We've purchased all the different basic elements from the periodic table we need, but we've gotten all sorts of orders for chemicals that we'll have to make ourselves!");
        tutorial.Add("That's where you come in. We're going to need your help with balancing all of these chemicals and finishing up all the orders in time.");
        tutorial.Add("These scientists aren't very patient people...");
        tutorial.Add("So follow me and I'll show you around the lab!");
        ButtonPress();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPress()
    {
        if (tutorial_num < tutorial.Count)
        {
            tutorial_text.text = tutorial[tutorial_num];
            tutorial_num++;
        }
    }
}
