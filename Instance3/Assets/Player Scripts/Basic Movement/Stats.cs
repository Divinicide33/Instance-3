using UnityEngine;

public class Stats : MonoBehaviour
{
    public int health;
    public int healthMax;
    public int damage;
    public float speed;

    void Start()
    {
        health = healthMax;
    }
}
