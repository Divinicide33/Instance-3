using UnityEngine;

public class Stats : MonoBehaviour
{
    public int health;
    public int healthMax;
    public int damage;
    public float speed;

    private void Start()
    {
        SetHpToHpMax();
    }

    public void SetHpToHpMax()
    {
        if (health <= 0)
            PlayerController.onIsDead?.Invoke(false);
        
        health = healthMax;
        DisplayHealth.onUpdate?.Invoke();
    }

    public void AddHp(int value)
    {
        health += value;
        DisplayHealth.onUpdate?.Invoke();
    }
}
