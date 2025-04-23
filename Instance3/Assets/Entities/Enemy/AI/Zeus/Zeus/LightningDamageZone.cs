using UnityEngine;

public class LightningDamageZone : MonoBehaviour
{
    private int damage = 1;
    private int knockbackPower = 0;

    public void SetDamage(int valueDamage) 
    {  
        damage = valueDamage; 
    }
    
    public void SetKnockbackPower(int valueKnockback)
    {
        knockbackPower = valueKnockback;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out _))
            return;

        if (!other.gameObject.transform.parent.TryGetComponent(out PlayerController player))
        {
            return;
        }

        if (player == null)
        {
            return;
        }

        Vector3 direction = (other.transform.position - transform.position).normalized;
        Ray ray = new Ray(other.transform.position - direction * 1f, direction);
        
        player.TakeDamage(damage, direction , knockbackPower);
    }
}
