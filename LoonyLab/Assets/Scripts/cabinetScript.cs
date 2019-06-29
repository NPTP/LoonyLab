using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticVars;

public class cabinetScript : MonoBehaviour
{

    public Animator cabinetAnimator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (chemClickEvent) {
            StartCoroutine(OpenCabinet());
        }
    }

    IEnumerator OpenCabinet() {
        cabinetAnimator.SetInteger("moleculeNum", chemNumPublic);
        cabinetAnimator.SetBool("cabinetActive", true);
        yield return new WaitForSeconds(0.5f);
        cabinetAnimator.SetBool("cabinetActive", false);
        chemClickEvent = false;
    }
}
