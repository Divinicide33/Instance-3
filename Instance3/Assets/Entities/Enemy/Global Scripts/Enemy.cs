using UnityEngine;

[RequireComponent(typeof(Stats))]
public class Enemy : Entity
{
    [HideInInspector] public Stats stats;
    [HideInInspector] public EnemyHurtFX enemyHurtFX;

    [Header("FX")]
    public string sfxHurtName;
    public string sfxDeathName;
    public string sfxAttackName;

    private void Start()
    {
        TryGetComponent(out stats);
        enemyHurtFX = GetComponentInChildren<EnemyHurtFX>();
        DoorArena.onAddEnemy?.Invoke(this);
    }

    public override void Defeat()
    {
        base.Defeat();
        enemyHurtFX?.ShowSFX(sfxDeathName);
        DoorArena.onRemoveEnemy?.Invoke(this);
        Destroy(gameObject);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        enemyHurtFX?.ShowVFX();
        if (stats.health > 0)
        {
            enemyHurtFX?.ShowSFX(sfxHurtName);
        }
    }
}
