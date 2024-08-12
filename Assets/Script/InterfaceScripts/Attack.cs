using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBehavior
{
    Animator animator;
    public virtual void Attack()
    {
        animator.SetTrigger("Attack");
    }
}
