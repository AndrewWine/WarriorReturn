using System;
using TMPro;
using UnityEngine;

public class GoblinMovement : MonoBehaviour
{
    [SerializeField] protected float radius = 3.0f; // Bán kính kiểm tra
    public float Radius { get { return radius; } set { radius = value; } }
    public MobStats Stats;
    [SerializeField] protected float raycastDistance = 5.0f; // Khoảng cách raycast
    public float RaycastDistance { get { return raycastDistance; } set { raycastDistance = value; } }
    [SerializeField] protected float raycastAngle = -30f; // Góc chiếu raycast
    public float RaycastAngle { get { return raycastAngle; } set { raycastAngle = value; } }
    public LayerMask groundLayer; // Layer mask cho Ground
    public Action NotifyAttack;
    public Action NotifyAttack2;
    private Transform playerTransform;
    private bool isPlayerInRange = false;
    private bool movingRight = true; // Biến trạng thái theo dõi hướng di chuyển
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb;
    public float distanceToPlayer;
    public GoblinHealth SendDefeated;
    public bool OnableToMove;
    public bool RepeatAtk;
    public int CountAtk;

    private bool hasAttacked; // Biến trạng thái để theo dõi việc gọi sự kiện

    private void Awake()
    {
        CountAtk = 0; // Khởi tạo CountAtk là 0
        OnableToMove = true;
        spriteRenderer = GetComponent<SpriteRenderer>(); // Khởi tạo spriteRenderer
        animator = GetComponent<Animator>(); // Khởi tạo animator
        rb = GetComponent<Rigidbody2D>(); // Khởi tạo Rigidbody2D

        if (SendDefeated != null)
        {
            SendDefeated.isDeath += isDeath;
        }
        else
        {
            Debug.LogError("SendDefeated is not assigned.");
        }

        if (Stats == null)
        {
            Debug.LogError("MobStats is not assigned.");
        }
    }

    private void Update()
    {
        CheckPlayerInRange();
        if (isPlayerInRange)
        {
            MoveTowardsPlayer();
            RepeatAtk = true;
        }
        else
        {
            MoveAndCheckGround();
            RepeatAtk = false;
        }

        if (playerTransform != null && playerTransform.CompareTag("Player"))
        {
            distanceToPlayer = Vector2.Distance(transform.position, (Vector2)playerTransform.position);
        }
        else
        {
            // Xử lý trường hợp khi playerTransform không phải là người chơi, nếu cần
        }
    }

    private void CheckPlayerInRange()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, Radius, LayerMask.GetMask("Player"));
        if (collider != null)
        {
            isPlayerInRange = true;
            playerTransform = collider.transform;
        }
        else
        {
            isPlayerInRange = false;
            playerTransform = null;
        }
    }

    private void MoveTowardsPlayer()
    {
        if (OnableToMove)
        {
            if (distanceToPlayer > 4.0f)
            {
                animator?.SetBool("Run", true); // Sử dụng toán tử null-conditional

                // Tính toán hướng di chuyển
                Vector2 direction = (playerTransform.position - transform.position).normalized;

                // Di chuyển về phía người chơi mà không thay đổi rotation và giữ tọa độ y không đổi
                Vector2 newPosition = (Vector2)transform.position + direction * (Stats?.MoveSpeed ?? 0) * Time.deltaTime;
                rb?.MovePosition(new Vector2(newPosition.x, transform.position.y));

                // Flip sprite dựa trên hướng di chuyển
                if (direction.x > 0 && !movingRight) // Đang di chuyển sang phải mà chưa flip
                {
                    Flip();
                }
                else if (direction.x < 0 && movingRight) // Đang di chuyển sang trái mà đã flip
                {
                    Flip();
                }

                // Reset trạng thái đã tấn công
                hasAttacked = false;
            }
            else if (distanceToPlayer <= 4.0f && RepeatAtk && !hasAttacked)
            {
                if (CountAtk < 3)
                {
                    NotifyAttack?.Invoke(); // Gọi sự kiện NotifyAttack
                    CountAtk++;
                    Debug.Log("Đã gọi Atk " + CountAtk);
                }
                else if (CountAtk == 3)
                {
                    NotifyAttack2?.Invoke(); // Gọi sự kiện NotifyAttack2
                    Debug.Log("Đã gọi Atk2");
                    CountAtk = 0; // Reset CountAtk sau khi gọi NotifyAttack2
                }

                // Đánh dấu đã tấn công
                hasAttacked = true;
            }
        }
        else
        {
            Debug.Log("isDeath");
        }
    }

    private void Flip()
    {
        movingRight = !movingRight; // Đảo ngược hướng di chuyển
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX; // Thực hiện flip sprite
        }
    }

    private void MoveAndCheckGround()
    {
        if (OnableToMove)
        {
            animator?.SetBool("Run", true);
            // Cập nhật hướng raycast dựa trên hướng di chuyển của nhân vật
            Vector2 raycastDirection = movingRight ? Quaternion.Euler(0, 0, RaycastAngle) * Vector2.down : Quaternion.Euler(0, 0, -RaycastAngle) * Vector2.down;

            // Thực hiện raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, RaycastDistance, groundLayer);
            Debug.DrawRay(transform.position, raycastDirection * RaycastDistance, Color.red);

            Vector2 moveDirection = movingRight ? Vector2.right : Vector2.left;

            // Di chuyển về phía moveDirection mà không thay đổi rotation và giữ tọa độ y không đổi
            Vector2 newPosition = (Vector2)transform.position + moveDirection * (Stats?.MoveSpeed ?? 0) * Time.deltaTime;
            rb?.MovePosition(new Vector2(newPosition.x, transform.position.y));
            Debug.Log("Tia đã va chạm với: " + hit.collider?.name); // Tránh lỗi NullReferenceException

            if (hit.collider == null)
            {
                Debug.Log("Không thấy Ground, flip thôi");
                Flip();
            }
        }
        else
        {
            Debug.Log("isDeath");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

    private void isDeath()
    {
        OnableToMove = false;
        rb.velocity = Vector2.zero; // Dừng toàn bộ vận tốc
        animator?.SetBool("Run", false); // Dừng animation chạy
        animator?.SetTrigger("Death");
    }
}
