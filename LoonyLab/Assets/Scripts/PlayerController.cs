using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    public bool balancing = false;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput * speed;

        // Set 4-way direction in this particular order to prioritize sideways walking animation

        animator.SetBool("movingUp", false);
        animator.SetBool("movingDown", false);
        animator.SetBool("movingRight", false);
        animator.SetBool("movingLeft", false);

        if (moveVelocity[1] > 0) {
            animator.SetBool("movingUp", true);
        }
        else if (moveVelocity[1] < 0) {
            animator.SetBool("movingDown", true);
        }
        else {
            animator.SetBool("movingUp", false);
            animator.SetBool("movingDown", false);
        }

        if (moveVelocity[0] > 0) {
            animator.SetBool("movingRight", true);
        }
        else if (moveVelocity[0] < 0) {
            animator.SetBool("movingLeft", true);
        }
        else {
            animator.SetBool("movingRight", false);
            animator.SetBool("movingLeft", false);
        }
    }

    private void FixedUpdate()
    {
        if (!balancing)
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
}
