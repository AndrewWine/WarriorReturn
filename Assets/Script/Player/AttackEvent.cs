using System;

public class AttackEvent
{
    public static event Action OnAttack;

    public static void NotifyAttack()
    {
        OnAttack?.Invoke();
    }
}
