using UnityEngine;

[RequireComponent(typeof(Stats))]
public class Enemy : Entity
{
    [HideInInspector] public Stats stats;
    [HideInInspector] public EnemyHurtFX enemyHurtFX;

    private void Start()
    {
        TryGetComponent(out stats);
        enemyHurtFX = GetComponentInChildren<EnemyHurtFX>();
    }

    public override void Defeat()
    {
        base.Defeat();
        DoorArena.onRemoveEnemy?.Invoke(this);
        DoorBoss.onSetBossDefeated?.Invoke(true);
        Destroy(gameObject);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        enemyHurtFX?.ShowFX();
    }
}
