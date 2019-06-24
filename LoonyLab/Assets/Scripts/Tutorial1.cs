using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial1 : MonoBehaviour
{
    private List<string> tutorial = new List<string>();
    private int tutorial_num = 0;

    public Text tutorial_text;

    // Start is called before the first frame update
    void Start()
    {
        tutorial.Add("Hello! Welcome to the Loony Lab! I'm glad you're here because we could definitely use some help with our opening day...");
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
        else
        {
            SceneManager.LoadScene("TutorialLab");
        }
    }
}


