using System.Collections;
using UnityEngine;

public class BODAttack : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // Tham chiếu đến người chơi
    public AbstractMovement OnAttack;
    public Animator animator;
    private Collider2D attackCollider; // Biến để lưu trữ Collider2D
    public Rigidbody2D rb;
    private void Awake()
    {
        if (OnAttack != null)
        {
            OnAttack.OnableToAttack += OnableToAttack;
        }
        else
        {
            Debug.LogWarning("OnAttack không được gán.");
        }

        // Lấy Collider2D từ đối tượng này
        attackCollider = GetComponent<Collider2D>();
        if (attackCollider == null)
        {
            Debug.LogError("Collider2D không được gán vào đối tượng này.");
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }


    private void OnDestroy()
    {
        if (OnAttack != null)
        {
            OnAttack.OnableToAttack -= OnableToAttack;
        }
    }

    private void OnableToAttack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
            Debug.Log("ĐÁNH");

            // Bật Collider2D
            ToggleCollider(true);
            // Tắt Collider2D sau một khoảng thời gian
            StartCoroutine(DisableColliderAfterDelay(0.5f)); // Điều chỉnh thời gian theo nhu cầu
        }
        else
        {
            Debug.LogWarning("Animator chưa được gán.");
        }
    }

    // Phương thức để bật/tắt Collider2D
    private void ToggleCollider(bool enable)
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = enable;
        }
    }

    // Coroutine để tắt Collider2D sau một khoảng thời gian
    private IEnumerator DisableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ToggleCollider(false);
    }
}
