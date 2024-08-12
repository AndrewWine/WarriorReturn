using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Player", order = 1)]
public class PlayerStats : ScriptableObject
{
    [SerializeField] protected float health = 20;
    public float Health { get { return health; } set { health = value; } }

    [SerializeField] protected float moveSpeed = 3;
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    [SerializeField] protected float attackDamage = 5;
    public float AttackDamage { get { return attackDamage; } set { attackDamage = value; } }

}
