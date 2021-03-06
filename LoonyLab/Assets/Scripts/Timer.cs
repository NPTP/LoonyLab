﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public float timeLeft = 20.0f;
    public Text timeDisplay;

    public GameObject cameraObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            int timeRounded = (int)timeLeft;

            int mins = timeRounded / 60;
            int secs = timeRounded % 60;

            if (secs > 9)
                timeDisplay.text = mins.ToString() + ":" + secs.ToString();
            else
                timeDisplay.text = mins.ToString() + ":0" + secs.ToString();
        }
        else
        {
            cameraObject.GetComponent<Level1>().EndLevel();
        }
    }
}
