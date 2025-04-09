using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private int nbJump;
    [SerializeField] private int nbJumpMax = 2;
    [SerializeField] private float jumpForce;
    private Rigidbody2D rb;
    private bool isGrounded = false;

    private void Awake() 
    {
        nbJump = nbJumpMax;
        rb = GetComponent<Rigidbody2D>();
    }
    private void ResetJump(bool value)
    {
        isGrounded = value;
        if (isGrounded) nbJump = nbJumpMax;
    }

    void OnEnable()
    {
        GroundCheck.onGrounded += ResetJump;
        PlayerController.onJump += Jump;
    }
    void OnDisable()
    {
        GroundCheck.onGrounded -= ResetJump;
        PlayerController.onJump -= Jump;
    }
    public void Jump(bool isJumpPressed) // if you can jump : decremente from the count and do the jump
    {
        if (isJumpPressed)
        {
            if (nbJump <= 0) return;
            nbJump--;
            Debug.Log($"nbJump = {nbJump}");

            Vector2 jumpDirection = new Vector2(0, jumpForce);
            rb.linearVelocityY = 0;
            isGrounded = false;
            rb.AddForce(jumpDirection, ForceMode2D.Impulse);
        }
        else
        {
            StopJump();
        }
    }

    private void StopJump() // if you stop your jump mid air you slow down his momentum
    {
        if (rb.linearVelocityY > 0) rb.linearVelocityY /= 2;
    }

}
