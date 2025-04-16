using UnityEngine;

[RequireComponent(typeof(Stats))]
public abstract class Entity : MonoBehaviour
{
    protected Stats stat;
    protected bool isDead = false;

    private void Awake() 
    {
        stat = GetComponent<Stats>();
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;

        Debug.Log($"{gameObject.name} is taking {damage} damage");

        stat.health -= damage;
        if (stat.health <= 0) 
        {
            stat.health = 0;
            Defeat();
        }
    }

    public virtual void TakeDamage(int damage, Vector3 originPosOfDamage, float power = 3)
    {
        TakeDamage(damage);
    }

    public virtual void Healing(int heal)
    {
        if (isDead) return;

        //stat.health += heal;
        stat.AddHp(heal);
        
        if (stat.health > stat.healthMax)
        {
            //stat.health = stat.healthMax;
            stat.SetHpToHpMax();
        }
    }
    
    public virtual void Reset()
    {
        stat.health = stat.healthMax;
        isDead = false;
    }

    public virtual void Defeat()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} is dead");
    }
}
