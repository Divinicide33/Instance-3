using System;
using UnityEngine;
using System.Collections;

public class DoorBoss : Door
{
    [SerializeField] private bool bossDefeated = false;
    public static Action<bool> onSetBossDefeated;
    
    private void OnEnable()
    {
        onSetBossDefeated += SetBossDefeated;
    }

    private void OnDisable()
    {
        onSetBossDefeated -= SetBossDefeated;
    }

    public void SetBossDefeated(bool value)
    {
        bossDefeated = value;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!bossDefeated)
        {
            Debug.Log("🚫 Le boss n’est pas encore vaincu !");
            return;
        }

        base.OnTriggerEnter2D(other);
    }
}