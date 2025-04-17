using System;
using UnityEngine;

[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(EnemyTest))]
public class Enemy : Entity
{
    public Stats stats;
    private void Start()
    {
        stats = GetComponent<Stats>();
    }
}
