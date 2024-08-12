using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    
    private InputManager inputManager;
    public PlayerMovement playerMovement;
    private Animator animator;
    [SerializeField] private GameObject model;
    private SpriteRenderer playerSpriteRenderer;
    private bool isAttacking = false;
    public float attackDelay = 1.0f;
    private Collider2D attackCollider; // Thêm biến để lưu trữ Collider2D

    private void Start()
    {
        // Lấy đối tượng InputManager và lưu trữ tham chiếu
        inputManager = FindObjectOfType<InputManager>();
        if (inputManager != null)
        {
            // Đăng ký sự kiện
            inputManager.doOnAttack += OnClick;
            // Lưu tham chiếu của doOnAttack từ InputManager
         
        }
        else
        {
            Debug.LogError("Không tìm thấy InputManager trong scene.");
        }

        model = GameObject.Find("Model");
        if (model != null)
        {
            animator = model.GetComponent<Animator>();
            playerSpriteRenderer = model.GetComponent<SpriteRenderer>();
            if (animator == null)
            {
                Debug.LogError("Animator không được gán vào đối tượng này.");
            }
            if (playerSpriteRenderer == null)
            {
                Debug.LogWarning("Model không có SpriteRenderer.");
            }
        }
        else
        {
            Debug.LogError("Model không được tìm thấy.");
        }

        // Lấy Collider2D từ đối tượng này
        attackCollider = GetComponent<Collider2D>();
        if (attackCollider == null)
        {
            Debug.LogError("Collider2D không được gán vào đối tượng này.");
        }
    }

    private void OnDestroy()
    {
        if (inputManager != null)
        {
            // Hủy đăng ký sự kiện khi đối tượng bị hủy
            inputManager.doOnAttack -= OnClick;
        }
    }

    public void OnClick()
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackWithDelay());
            // Chờ một chút để đảm bảo Animator đã được kích hoạt
            // Đây có thể là điều chỉnh thời gian delay cho phù hợp
        }
    }

    public void FlipxCollider()
    {
        if (playerMovement.horizontal != 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(playerMovement.horizontal), transform.localScale.y, transform.localScale.z);
        }
    }

    private IEnumerator AttackWithDelay()
    {
        isAttacking = true;

        // Kích hoạt hoạt ảnh tấn công
        animator.SetTrigger("Attack");

        // Đợi cho đến khi hoạt ảnh bắt đầu trước khi bật Collider2D
        yield return new WaitForSeconds(0.1f); // Điều chỉnh thời gian này cho phù hợp với hoạt ảnh của bạn

        // Bật Collider2D sau khi hoạt ảnh đã bắt đầu
        ToggleCollider(true);

        // Đợi cho đến khi thời gian delay đã trôi qua
        yield return new WaitForSeconds(attackDelay - 0.1f); // Trừ thời gian chờ ban đầu

        isAttacking = false;
        ToggleCollider(false); // Tắt Collider2D khi kết thúc tấn công
    }

    private void Update()
    {
        if (playerSpriteRenderer != null)
        {
            // Đồng bộ hóa flipX của PlayerAttack với flipX của PlayerMovement
            bool isFlipped = playerSpriteRenderer.flipX;
            // Thay đổi hướng của collider hoặc sprite của PlayerAttack dựa trên isFlipped
            // Ví dụ:
            // transform.localScale = new Vector3(isFlipped ? -1 : 1, 1, 1);
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
}
