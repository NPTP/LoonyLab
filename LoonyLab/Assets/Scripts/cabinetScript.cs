using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TutorialControl;

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
        cabinetAnimator.SetBool("cabinetActive", true);
        cabinetAnimator.SetInteger("moleculeNum", chemNumPublic);
        yield return new WaitForSeconds(1);
        cabinetAnimator.SetBool("cabinetActive", false);
    }
}
