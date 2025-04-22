using System;
using UnityEngine;
using System.Collections.Generic;

public class DoorArena : Door
{
    private List<Enemy> remainingEnemies = new();

    public static Action<Enemy> onAddEnemy;
    public static Action<Enemy> onRemoveEnemy;

    protected bool isCleared = false;

    protected override void OnEnable()
    {
        onAddEnemy += AddEnemy;
        onRemoveEnemy += RemoveEnemy;
        
        isCleared = PlayerPrefs.HasKey(refSave) && PlayerPrefs.GetInt(refSave) == 1; // truc a ajouter
        Debug.Log($"isCleared : {isCleared}");
        
        TryGetComponent(out sprite);
        if (isCleared) 
        {
            DisableSprite();
            return;
        }
        EnableSprite();
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

        if (remainingEnemies.Count > 0) 
            return;
        
        isCleared = true;
        DisableSprite();
            
        // save
        PlayerPrefs.SetInt(refSave, 1);
        PlayerPrefs.Save();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCleared)
            return;
        
        base.OnTriggerEnter2D(other); // autorise le passage si plus d’ennemis   
    }
}