using UnityEngine;
using System;

public abstract class AbstractMovement : MonoBehaviour
{
    public float moveSpeed = 0.3f; // Tốc độ di chuyển
    public float flipInterval = 2f; // Thay đổi hướng sau mỗi khoảng thời gian này (giây)
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected bool movingRight;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float attackRange = 0.02f; // Khoảng cách tấn công
    public bool MovingRight { get { return movingRight; } set { movingRight = value; } }
    [SerializeField] protected BoxCollider2D boxCollider;
    public BoxCollider2D BoxCollider { get { return boxCollider; } set { boxCollider = value; } }
    [SerializeField] protected float timer = 0f; // Biến theo dõi thời gian đã trôi qua
    public float Timer { get { return timer; } set { timer = value; } }
    public bool isMoving;
    public AbstractSearchingTarget Normalmoving;
    public AbstractSearchingTarget attackPlayer;
    public Action OnableToAttack;
    public bool isInAttackRange;

    private void Awake()
    {
        if (Normalmoving != null)
        {
            Normalmoving.Justmoving += Justmoving;
        }
        else
        {
            Debug.LogWarning("Normalmoving không được gán.");
        }

        if (attackPlayer != null)
        {
            attackPlayer.attackPlayerMove += attackPlayerMove;
        }
        else
        {
            Debug.LogWarning("attackPlayer không được gán.");
        }
    }

    private void OnDestroy()
    {
        if (Normalmoving != null)
        {
            Normalmoving.Justmoving -= Justmoving;
        }

        if (attackPlayer != null)
        {
            attackPlayer.attackPlayerMove -= attackPlayerMove;
        }
    }

    private void Start()
    {
        isMoving = true;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    public void attackPlayerMove()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= attackRange)
        {
            if (!isInAttackRange) // Đảm bảo rằng chỉ kích hoạt khi lần đầu tiên vào phạm vi tấn công
            {
                OnableToAttack?.Invoke();
                Debug.Log("Đủ tầm tấn công");
                isInAttackRange = true; // Đánh dấu là đã vào phạm vi tấn công
            }
        }
        else
        {
            isInAttackRange = false; // Đặt lại trạng thái khi ra khỏi phạm vi tấn công
            MoveTowardsPlayer();
        }
    }


    private void MoveTowardsPlayer()
    {
        if (playerTransform == null) return;

        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        transform.Translate(directionToPlayer * moveSpeed * Time.deltaTime);
    }

    public void Justmoving()
    {
        if (isMoving)
        {
            animator.SetBool("isMoving", true);

            if (movingRight)
            {
                transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            }

            timer += Time.deltaTime;
            if (Timer >= flipInterval)
            {
                Timer = 0f;
                movingRight = !movingRight;

                if (spriteRenderer != null)
                {
                    spriteRenderer.flipX = !movingRight;
                }

                if (boxCollider != null)
                {
                    AdjustColliderOffset();
                }
            }
        }
    }

    public void AdjustColliderOffset()
    {
        if (spriteRenderer.flipX)
        {
            BoxCollider.offset = new Vector2(-Mathf.Abs(BoxCollider.offset.x), BoxCollider.offset.y);
        }
        else
        {
            BoxCollider.offset = new Vector2(Mathf.Abs(BoxCollider.offset.x), BoxCollider.offset.y);
        }
    }

    public Vector2 GetCurrentDirection()
    {
        return spriteRenderer.flipX ? Vector2.left : Vector2.right;
    }
}
