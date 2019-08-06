using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticVars;


public class balstationAnimation : MonoBehaviour
{

    public Animator balstationAnimator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (balstationActive) {
            balstationAnimator.SetBool("balstationActive", true);
        }
        else {
            balstationAnimator.SetBool("balstationActive", false);
        }

        if (compoundMade) {
           StartCoroutine(doneAnimation());
           
        }
    }

    IEnumerator doneAnimation() {
        balstationAnimator.SetBool("balstationMadeCompound", true);
        yield return new WaitForSeconds(1f);
        balstationAnimator.SetBool("balstationMadeCompound", false);
        balstationAnimator.SetBool("balstationActive", false);
        balstationActive = false;
        compoundMade = false;
    }
}
