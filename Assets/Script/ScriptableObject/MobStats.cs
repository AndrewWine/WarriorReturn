using UnityEngine;

[CreateAssetMenu(fileName = "NewMobStats", menuName = "Stats/MobStats", order = 1)]
public class MobStats : ScriptableObject
{
    [SerializeField] protected float defaultHealth = 10;
    [SerializeField] protected float defaultMoveSpeed = 3;
    [SerializeField] protected float defaultAttackDamage = 5;

    [SerializeField] protected float health;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float attackDamage;

    public float Health { get { return health; } set { health = value; } }
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public float AttackDamage { get { return attackDamage; } set { attackDamage = value; } }

    private void OnEnable()
    {
        ResetStats();
    }

    private void ResetStats()
    {
        health = defaultHealth;
        moveSpeed = defaultMoveSpeed;
        attackDamage = defaultAttackDamage;
    }
}
