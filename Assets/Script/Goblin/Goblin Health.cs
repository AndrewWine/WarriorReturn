using System;
using UnityEngine;

public class GoblinHealth : MonoBehaviour, IHealth
{
    public Animator animator;
    public MobStats mobStats; // Tham chiếu đến ScriptableObject MobStats
    public Action isDeath;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component không được gán.");
            }
        }

        if (mobStats == null)
        {
            Debug.LogError("MobStats ScriptableObject không được gán.");
        }

        // Đăng ký nhận thông báo từ sự kiện OnPlayerDamage
        PlayerAttack.OnPlayerDamage += TakeDamage;
    }

    private void OnDestroy()
    {
        // Hủy đăng ký nhận thông báo từ sự kiện để tránh rò rỉ bộ nhớ
        PlayerAttack.OnPlayerDamage -= TakeDamage;
    }

   

    // Triển khai phương thức TakeDamage từ giao diện IHealth
    public void TakeDamage(float damage)
    {
        if (mobStats == null) return;
        if (mobStats.Health > 0) 
        { 
            mobStats.Health -= damage; // Cập nhật máu
            Debug.Log("Máu Goblin sau khi bị sát thương: " + mobStats.Health);
            animator.SetTrigger("Hurt");
        }
        else Defeated();
    }

    public void Defeated()
    {
        if (animator != null)
        {
           
          
            isDeath?.Invoke();
        }
       
    }
}
