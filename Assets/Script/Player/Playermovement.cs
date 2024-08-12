using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private InputManager inputManager; // Thêm biến để lưu trữ tham chiếu đến InputManager
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public GameObject model;
    public PlayerStats playerStats;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] int maxJumpCount = 2;
    private int currentJumpCount;
    bool isGrounded;
    public   float horizontal = 0;
  
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Ngăn không cho rotation thay đổi
        animator = model.GetComponent<Animator>(); 
        CheckConditionError();
        currentJumpCount = maxJumpCount;
        inputManager.doOnJump += OnPress;
    }


    void Update()
    {
        Moving();
        Jump();
        FallCondition();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || currentJumpCount > 0)
            {
                if (rb != null)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    if (animator != null)
                    {
                        animator.SetTrigger("Jump");
                    }
                    Debug.Log("Jump");
                    currentJumpCount--;
                }
            }
        }
    }

    private void FallCondition()
    {
        if (rb.velocity.y < 0)
        {
            if (animator != null)
            {
                animator.SetBool("Fall", true);
            }
        }
    }

    private void CheckConditionError()
    {
        if (model != null)
        {
            spriteRenderer = model.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogWarning("Model không có SpriteRenderer.");
            }
        }
        else
        {
            Debug.LogError("Model không được gán vào đối tượng PlayerMovement.");
        }

        if (rb == null) Debug.LogError("Rigidbody2D không được gán vào đối tượng chính.");
        if (animator == null) Debug.LogError("Animator không được gán vào model.");
    }

    private void Moving()
    {
      

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            horizontal = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            horizontal = -1;
        }
        else
        {
            horizontal = 0;
        }

        if (rb != null)
        {
            rb.velocity = new Vector2(horizontal * playerStats.MoveSpeed, rb.velocity.y);
        }

        // Flip đối tượng cha, ảnh hưởng đến tất cả các đối tượng con
        if (horizontal != 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(horizontal), transform.localScale.y, transform.localScale.z);
        }

        bool isMoving = horizontal != 0;

        if (animator != null)
        {
            animator.SetBool("Run", isMoving);
        }
    }


    private void OnDestroy()
    {
        inputManager.doOnJump -= OnPress;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("Fall", false);
            Debug.Log("Grounded");
            isGrounded = true;
            animator.SetBool("Run", true);
            currentJumpCount = maxJumpCount;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("Fall", true);
            isGrounded = false;
            animator.SetBool("Run", false);
        }
    }

    public void OnPress(float dashForce)
    {
        animator.SetTrigger("Dash");
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal == 0)
        {
            horizontal = -1;
        }
        float dashMultiplier = 3.0f;
        float calculatedDashForce = dashForce * dashMultiplier;

        if (horizontal > 0)
        {
            rb.velocity = new Vector2(calculatedDashForce, rb.velocity.y);
        }
        else if (horizontal < 0)
        {
            rb.velocity = new Vector2(-calculatedDashForce, rb.velocity.y);
        }
    }


}
