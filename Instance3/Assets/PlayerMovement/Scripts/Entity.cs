using UnityEngine;

[RequireComponent(typeof(Stats))]
public abstract class Entity : MonoBehaviour
{
    protected Stats stat;

    private void Awake() 
    {
        stat = GetComponent<Stats>();
    }

    public void TakeDamage(int damage)
    {
        stat.health -= damage;
        if (stat.health < 0) 
        {
            stat.health = 0;
            Defeat();
        }
    }

    public void Healing(int heal)
    {
        stat.health += heal;
        if (stat.health > stat.healthMax) stat.health = stat.healthMax;
    }

    public virtual void Defeat()
    {
        Debug.Log("Enemy is dead");
    }
}
