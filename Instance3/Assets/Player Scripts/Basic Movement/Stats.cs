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
        health = healthMax;
        DisplayHealth.onUpdate?.Invoke();
        PlayerController.onIsDead?.Invoke(false);
    }

    public void AddHp(int value)
    {
        health += value;
        DisplayHealth.onUpdate?.Invoke();
    }
}
