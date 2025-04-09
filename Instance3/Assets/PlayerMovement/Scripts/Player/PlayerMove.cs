using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    private Stats stats;
    private Vector2 direction;
    [SerializeField] private float offSetInput = 0.5f;

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
        if (direction.x > offSetInput) 
        {
            player.isFacingRight = true;
            direction.x = 1;
        }
        else if (direction.x < -offSetInput) 
        {
            player.isFacingRight = false;
            direction.x = -1;
        }
    }
}
