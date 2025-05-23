using UnityEngine;

public class EnemyAttack : Enemy
{
    [SerializeField] private Vector2 size;
    [SerializeField] private float knockBackPower;

    void Update()
    {
        if (isDead) 
            return;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            transform.position, size, 0, LayerMask.GetMask(LayerMap.Player.ToString()
            ));

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.transform.parent == null ||
                !collider.gameObject.transform.parent.TryGetComponent(out PlayerController player)) 
                continue;
            
            player.TakeDamage(stats.damage, transform.position ,knockBackPower);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
