using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    private Stats stats;
    private Vector2 direction;

    public bool canMove = true;
    private void Awake() 
    {
        stats = GetComponent<Stats>();
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        Player.onMove += Move;
    }

    private void Update() 
    {
        if (canMove)
        {
            rb.linearVelocityX = direction.x * stats.speed;
        }
        
    }
    public void Move(Vector2 inputDirection)
    {
        direction = inputDirection;
        if (direction.x > 0.1) player.isFacingRight = true;
        else if (direction.x < -0.1) player.isFacingRight = false;
    }
}
