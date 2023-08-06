using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 3.4f;
    public float jumpHeight = 3.4f;
    public float gravityScale = 6.5f;

    bool facingRight = true;
    float moveDirection = 0;
    float airHorizontalVel = 0;
    bool isGrounded = false;
    Rigidbody2D r2d;
    BoxCollider2D mainCollider;
    Animator animator;

    // Start is called before the first frame update
    void Start() {
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<BoxCollider2D>();
        r2d.freezeRotation = true;
        r2d.gravityScale = gravityScale;
        facingRight = transform.localScale.x > 0;
        animator = GetComponent<Animator>();
    }

    void Update() {
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && isGrounded) {
            moveDirection = Input.GetKey(KeyCode.A) ? -1 : 1;
        } else if (isGrounded || r2d.velocity.magnitude < 0.01f) {
            moveDirection = 0;
        }
        if (moveDirection != 0) {
            if (moveDirection > 0 && !facingRight) {
                facingRight = true;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            if (moveDirection < 0 && facingRight) {
                facingRight = false;
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        if (isGrounded) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                animator.ResetTrigger("goIdle");
                animator.SetTrigger("charge");
            } else if (Input.GetKeyUp(KeyCode.Space)) {
                r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
            } else {
                airHorizontalVel = 0;
                animator.ResetTrigger("letGo");
                animator.SetTrigger("goIdle");
            }
        } else {
            airHorizontalVel = moveDirection;
            animator.ResetTrigger("charge");
            animator.ResetTrigger("goIdle");
            animator.SetTrigger("letGo");
        }
        Debug.Log(r2d.velocity.magnitude);
        // if (r2d.velocity.magnitude < 0.01f) {

        // }
    }

    void FixedUpdate() {
        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);

        isGrounded = false;
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != mainCollider)
                {
                    isGrounded = true;
                    break;
                }
            }
        }
        Debug.Log(airHorizontalVel);
        r2d.velocity = new Vector2((airHorizontalVel) * maxSpeed, r2d.velocity.y);

        Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(0, colliderRadius, 0), isGrounded ? Color.green : Color.red);
        Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(colliderRadius, 0, 0), isGrounded ? Color.green : Color.red);
    }
}
