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
    public PlayerStats playerStats; // Giả sử chứa giá trị sát thương
    private bool isAttacking = false;
    public float attackDelay = 1.0f;
    private Collider2D attackCollider;

    // Sự kiện thông báo khi người chơi gây sát thương
    public static event System.Action<float> OnPlayerDamage;

    private void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        if (inputManager != null)
        {
            inputManager.doOnAttack += OnClick;
        }
        else
        {
            Debug.LogError("Không tìm thấy InputManager.");
        }

        model = GameObject.Find("Model");
        if (model != null)
        {
            animator = model.GetComponent<Animator>();
            playerSpriteRenderer = model.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogError("Không tìm thấy Model.");
        }

        attackCollider = GetComponent<Collider2D>();
        if (attackCollider == null)
        {
            Debug.LogError("Không tìm thấy Collider2D trên PlayerAttack.");
        }
    }

    private void OnDestroy()
    {
        if (inputManager != null)
        {
            inputManager.doOnAttack -= OnClick;
        }
    }

    public void OnClick()
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackWithDelay());
        }
    }

    private IEnumerator AttackWithDelay()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);
        ToggleCollider(true);
        yield return new WaitForSeconds(attackDelay - 0.1f);
        isAttacking = false;
        ToggleCollider(false);
    }

    private void Update()
    {
        // Xử lý sprite của người chơi nếu cần
    }

    private void ToggleCollider(bool enable)
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = enable;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Gửi thông báo khi va chạm với đối tượng có thể nhận thông báo
        OnPlayerDamage?.Invoke(playerStats.AttackDamage); // Gửi sát thương đến sự kiện
    }
}
