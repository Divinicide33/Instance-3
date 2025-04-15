using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSword : ItemModule
{
    Stats stats;
    private bool isAttacking = false;
    [SerializeField] private Transform damagePos;
    [SerializeField] private Vector2 size;
    [SerializeField] private float radius;
    public static Action onSword { get; set; }

    protected override void Use()
    {
        // lance l'animation

    }

    public void DealDamage()
    {
        Collider2D[] entities = Physics2D.OverlapBoxAll(damagePos.position, size, radius);

        foreach (Collider2D entity in entities)
        {
            if (TryGetComponent<Entity>(out Entity damageScript))
            {
                damageScript.TakeDamage(stats.damage);
            }
        }
    }

    private void OnEnable() 
    {
        if (stats == null) stats = GetComponent<Stats>();
        onSword += Use;
    }
    private void OnDisable() 
    {
        onSword -= Use;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Use();
        }
    }

    void OnDrawGizmos()
    {
        if (!isAttacking) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(damagePos.position, size);
    }

}
