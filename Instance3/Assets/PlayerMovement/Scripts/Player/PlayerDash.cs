using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D rb;
    private Player player;
    private PlayerMove playerMove;

    [Header("Dash Variable")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;
    private float timerDash;
    private float timerCooldown;
    private bool canDash = true;
    private bool isDashing = false;
    private int direction;
    private float height;
    private bool isGrounded = false;
    void Awake()
    {
        player = GetComponent<Player>();
        playerMove = GetComponent<PlayerMove>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        Player.onDash += Dash;
        GroundCheck.onGrounded += ChangeBool;
    }
    void OnDisable()
    {
        Player.onDash -= Dash;
        GroundCheck.onGrounded -= ChangeBool;
    }

    void Update() 
    {
        if (canDash == false) CheckIfCanDash();
        else timerCooldown = 0;

        if (isDashing) Dashing();
        else timerDash = 0;
    }

    private void ChangeBool(bool value)
    {
        isGrounded = value;
    }

    public void Dash()
    {
        if (!isDashing && canDash)
        {
            playerMove.canMove = false;
            height = transform.position.y;
            rb.linearVelocityY = 0;

            if (player.isFacingRight) direction = 1;
            else direction = -1;

            canDash = false;
            isDashing = true;
        }
    }

    void Dashing()
    {
        timerDash += Time.deltaTime;

        transform.position = new Vector3(transform.position.x, height, transform.position.z);
        rb.linearVelocityX = direction * dashForce;
        
        if (timerDash >= dashDuration)
        {
            StopDash();
        }

    }
    void CheckIfCanDash()
    {
        timerCooldown += Time.deltaTime;
        if (timerCooldown >= dashCooldown && isGrounded) canDash = true;
    }

    public void StopDash()
    {
        isDashing = false;
        playerMove.canMove = true;
        rb.linearVelocityX = 0;
    }

}
