using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 3f;
    public float jumpHeight = 0f;
    public float gravityScale = 3f;
    public Transform indicator;

    bool facingRight = true;
    float moveDirection = 1;
    float airHorizontalVel = 0;
    bool isGrounded = false;
    Rigidbody2D r2d;
    Animator animator;

    // Start is called before the first frame update
    void Start() {
        r2d = GetComponent<Rigidbody2D>();
        r2d.freezeRotation = true;
        r2d.gravityScale = gravityScale;
        facingRight = transform.localScale.x > 0;
        animator = GetComponent<Animator>();
    }

    void Update() {
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && isGrounded) {
            moveDirection = Input.GetKey(KeyCode.A) ? -1 : 1;
        }
        if (moveDirection > 0 && !facingRight) {
            facingRight = true;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (moveDirection < 0 && facingRight) {
            facingRight = false;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (isGrounded) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                animator.ResetTrigger("goIdle");
                animator.SetTrigger("charge");
                jumpHeight = 0;
            } else if (Input.GetKeyUp(KeyCode.Space)) {
                r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
            } else {
                airHorizontalVel = 0;
                animator.ResetTrigger("letGo");
                animator.SetTrigger("goIdle");
            }
            if (Input.GetKey(KeyCode.Space)) {
                CycleJumpHeight();
            }
        } else {
            airHorizontalVel = moveDirection;
            animator.ResetTrigger("charge");
            animator.ResetTrigger("goIdle");
            animator.SetTrigger("letGo");
        }
        indicator.localScale = new Vector3(jumpHeight / 16, 1, 1);
    }

    void CycleJumpHeight() {
        if (jumpHeight <= 16 && jumpHeight >= 0) {
            jumpHeight += 7f * Time.deltaTime;
        } else if (jumpHeight >= 16) {
            jumpHeight = 0;
        } else if (jumpHeight < 0) {
            jumpHeight = 16;
        }
    }

    void FixedUpdate() {
        // thank god I got to purge that confusing ass code
        r2d.velocity = new Vector2(airHorizontalVel * maxSpeed, r2d.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        isGrounded = true;
    }
    void OnTriggerExit2D(Collider2D collision) {
        isGrounded = false;
    }
}
