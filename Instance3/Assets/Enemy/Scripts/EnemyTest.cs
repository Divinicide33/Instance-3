using UnityEngine;

public class EnemyTest : Enemy
{
    [SerializeField] private int damage = 1;
    [SerializeField] private Vector2 size;
    [SerializeField] private float knockBackPower;
    void Update()
    {
        if (isDead) return;

        Collider2D[] colliders = 
        Physics2D.OverlapBoxAll(transform.position, size, 0, LayerMask.GetMask(LayerMap.Player.ToString()));

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.transform.parent != null && 
            collider.gameObject.transform.parent.TryGetComponent(out PlayerController player))
            {
                player.TakeDamage(damage, transform.position ,knockBackPower);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
