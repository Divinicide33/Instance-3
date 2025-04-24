using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : ItemModule
{
    Stats stats;
    private bool isDoingDamage;
    [SerializeField] private Transform damagePos;
    [SerializeField] private Vector2 size;
    [SerializeField] private float radius;

    private List<Collider2D> colliderHited = new List<Collider2D>();
    public static Action onSword { get; set; }

    protected override void Use()
    {
        PlayerAnimator.onSetIsAttacking?.Invoke(true);
    }

    private void DealDamage()
    {
        Collider2D[] entities = Physics2D.OverlapBoxAll(damagePos.position, size, radius);

        foreach (Collider2D entity in entities)
        {
            if (colliderHited.Contains(entity))
                return;

            if (entity.TryGetComponent(out Entity damageScript))
                damageScript.TakeDamage(stats.damage);

            colliderHited.Add(entity);
        }
    }

    private void OnEnable() 
    {
        if (stats == null) 
            stats = GetComponent<Stats>();

        onSword += Use;
    }

    private void OnDisable() 
    {
        onSword -= Use;
    }

    void Update()
    {
        if (isDoingDamage) 
            DealDamage();
    }

    public void EnableDamage()
    {
        isDoingDamage = true;
        colliderHited.Clear();
    }

    public void DisableDamage()
    {
        isDoingDamage = false;
    }

    void OnDrawGizmos()
    {
        if (damagePos == null) 
            return;

        if (isDoingDamage) 
            Gizmos.color = Color.red;
        else 
            Gizmos.color = Color.blue;

        Vector3 center = damagePos.position;
        float angle = radius;
        Vector2 halfSize = size * 0.5f;

        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        Vector2[] corners = new Vector2[4];
        corners[0] = rotation * new Vector2(-halfSize.x, -halfSize.y) + center;
        corners[1] = rotation * new Vector2(-halfSize.x, halfSize.y) + center;
        corners[2] = rotation * new Vector2(halfSize.x, halfSize.y) + center;
        corners[3] = rotation * new Vector2(halfSize.x, -halfSize.y) + center;

        for (int i = 0; i < 4; i++)
        {
            Vector2 current = corners[i];
            Vector2 next = corners[(i + 1) % 4];
            Gizmos.DrawLine(current, next);
        }
    }

}
