using System;
using UnityEngine;
using System.Collections;

public class GoblinAttack : MonoBehaviour
{
    public GoblinMovement OnableToAttack;
    public Animator animator;
    private Collider2D attackCollider;
    private bool isAttacking = false;
    public float attackDelay = 1.0f;

    private void Awake()
    {
        OnableToAttack.NotifyAttack += NotifyAttack;
        OnableToAttack.NotifyAttack2 += NotifyAttack2;
    }

    private void Start()
    {
        // Khởi tạo animator nếu chưa được gán trong Awake
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component is missing in child objects.");
            }
        }

        // Khởi tạo attackCollider nếu chưa được gán
        attackCollider = GetComponent<Collider2D>();
        if (attackCollider == null)
        {
            Debug.LogError("Collider2D component is missing.");
        }
    }

    private void OnDestroy()
    {
        OnableToAttack.NotifyAttack -= NotifyAttack;
        OnableToAttack.NotifyAttack2 -= NotifyAttack2;
    }

    private void NotifyAttack()
    {
        StartCoroutine(AttackWithDelay());
        Debug.Log("Attack");
    }

    private void NotifyAttack2()
    {
        animator.SetTrigger("Attack2");
        Debug.Log("Attack2");
    }

    private void ToggleCollider(bool enable)
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = enable;
        }
    }

    private IEnumerator AttackWithDelay()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);
        ToggleCollider(true);
        yield return new WaitForSeconds(attackDelay - 0.1f);
        ToggleCollider(false);
        isAttacking = false;
        Debug.Log("Đã kích hoạt animator attack");
    }
}
