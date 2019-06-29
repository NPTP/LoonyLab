using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticVars;

public class trashScript : MonoBehaviour
{

    public Animator trashAnimator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (trashClickEvent) {
            StartCoroutine(UseTrash());
        }
    }

    IEnumerator UseTrash() {
        trashAnimator.SetBool("trashActive", true);
        yield return new WaitForSeconds(0.5f);
        trashAnimator.SetBool("trashActive", false);
        trashClickEvent = false;
        deliveryLightsOn = false;
    }
}
