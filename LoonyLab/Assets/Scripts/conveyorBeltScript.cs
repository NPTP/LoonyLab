using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticVars;

public class conveyorBeltScript : MonoBehaviour
{

    public Animator conveyorBeltAnimator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (conveyorBeltEvent) {
            StartCoroutine(UseConveyorBelt());
        }
    }

    IEnumerator UseConveyorBelt() {
        conveyorBeltAnimator.SetBool("conveyorBeltActive", true);
        yield return new WaitForSeconds(0.5f);
        conveyorBeltAnimator.SetBool("conveyorBeltActive", false);
        conveyorBeltEvent = false;
    }
}
