using System;
using UnityEngine;
using System.Collections;

public class DoorBoss : DoorArena
{
    protected override void AddToList(Enemy enemy)
    {
        if (isCleared)
        {
            enemy.gameObject.SetActive(false);
        }
        else
        {
            base.AddToList(enemy);
        }
    }
}