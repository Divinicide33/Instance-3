using System;
using UnityEngine;
using System.Collections.Generic;

public class DoorArena : Door
{
    private List<Enemy> remainingEnemies = new();

    public static Action<Enemy> onAddEnemy;
    public static Action<Enemy> onRemoveEnemy;

    private void OnEnable()
    {
        onAddEnemy += AddEnemy;
        onRemoveEnemy += RemoveEnemy;
    }

    private void OnDisable()
    {
        onAddEnemy -= AddEnemy;
        onRemoveEnemy -= RemoveEnemy;
    }

    private void AddEnemy(Enemy enemy)
    {
        if (!remainingEnemies.Contains(enemy))
            remainingEnemies.Add(enemy);
    }

    private void RemoveEnemy(Enemy enemy)
    {
        if (remainingEnemies.Contains(enemy))
            remainingEnemies.Remove(enemy);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (remainingEnemies.Count > 0)
        {
            Debug.Log("🚫 La porte de l’arène est verrouillée. Ennemis restants : " + remainingEnemies.Count);
            return;
        }

        base.OnTriggerEnter2D(other); // autorise le passage si plus d’ennemis
    }
}