using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DoorArena : Door
{
    private List<GameObject> remainingEnemies = new();

    public static Action<Enemy> onAddEnemy;
    public static Action<Enemy> onRemoveEnemy;

    protected bool isCleared = false;
    protected bool hasCheckedPlayerPrefs = false;

    protected override void OnEnable()
    {
        hasCheckedPlayerPrefs = false; // DO NOT REMOVE

        onAddEnemy += AddEnemy;
        onRemoveEnemy += RemoveEnemy;
        
        isCleared = PlayerPrefs.HasKey(refSave) && PlayerPrefs.GetInt(refSave) == 1; // truc a ajouter
        //Debug.Log($"isCleared : {isCleared}");

        hasCheckedPlayerPrefs = true;

        TryGetComponent(out sprite);

        if (isCleared) 
        {
            DisableSprite();
            return;
        }

        EnableSprite();
    }

    protected void OnDisable()
    {
        onAddEnemy -= AddEnemy;
        onRemoveEnemy -= RemoveEnemy;
    }

    protected void AddEnemy(Enemy enemy)
    {
        //Debug.Log("AddEnemy is called");
        StartCoroutine(WaitForPlayerPrefs(enemy));
    }

    protected virtual IEnumerator WaitForPlayerPrefs(Enemy enemy)
    {
        while (!hasCheckedPlayerPrefs)
        {
            yield return null; // Attend une frame, puis recommence
        }

        //Debug.Log($"hasCheckedPlayerPrefs = {hasCheckedPlayerPrefs}");

        AddToList(enemy);
    }

    protected virtual void AddToList(Enemy enemy)
    {
        if (!remainingEnemies.Contains(enemy.gameObject))
            remainingEnemies.Add(enemy.gameObject);

        //Debug.Log($"New enemy = {enemy}");
    }

    protected void RemoveEnemy(Enemy enemy)
    {
        //Debug.Log($"RemoveEnemy = {enemy}");
        if (remainingEnemies.Contains(enemy.gameObject))
            remainingEnemies.Remove(enemy.gameObject);

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