using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticVars;

public class deliveryLightScript : MonoBehaviour
{

    public Animator deliveryLightsAnimator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (deliveryLightsOn) {
            deliveryLightsAnimator.SetBool("lightsOn", true);
        }
        else {
            deliveryLightsAnimator.SetBool("lightsOn", false);
        }
    }
}
