using System;
using UnityEngine;

public class DamageReceiver : AbsKnockBack
{
    private Animator animator;
    private IKnockBack knockBackHandler; // Ensure this is assigned in your code
    public Transform target;
    public AbstractMovement Checkmove;


    void Start()
    {
        // Initialize the animator component
        animator = GetComponent<Animator>();

        // Subscribe to the action delegate
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collider's tag is "Player"
        if (collision.gameObject.CompareTag("Player"))
        {


            // Stop movement
            if (Checkmove != null)
            {
                Checkmove.StopMoving();
            }

            // Trigger the "Hurt" animation
            if (animator != null)
            {
                animator.SetTrigger("Hurt");
            }

            // Apply knockback
            if (knockBackHandler != null)
            {
                knockBackHandler.KnockBack(transform, target, 1f);
            }
        }
        else
        {
            // Ensure movement continues if not colliding with the Player
            if (Checkmove != null)
            {
                Checkmove.isMoving = true;
            }
        }
    }

    public void Hurt()
    {
        // Implement the behavior for when the object is hurt
        Debug.Log("Hurt method called");
    }
}