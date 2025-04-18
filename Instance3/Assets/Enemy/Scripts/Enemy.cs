using System;
using UnityEngine;

[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(EnemyTest))]
public class Enemy : Entity
{
    public Stats stats;

    protected EnemyHurtFX enemyHurtFX;

    private void Start()
    {
        stats = GetComponent<Stats>();
    }

    public override void Defeat()
    {
        base.Defeat();
        Destroy(gameObject);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        enemyHurtFX?.ShowFX();
    }
}
