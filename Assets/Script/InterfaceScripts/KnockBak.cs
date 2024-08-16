using UnityEngine;

public class EnemyKnockBack : MonoBehaviour, IKnockBack
{
    public void KnockBack(Transform transform, Transform target, float force)
    {
        // Logic knockback t?i ?�y
        Vector2 knockbackDirection = (transform.position - target.position).normalized;
        transform.position += (Vector3)knockbackDirection * force;
    }
}
