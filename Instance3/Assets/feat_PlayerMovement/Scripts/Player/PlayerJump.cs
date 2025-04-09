using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private int nbJump;
    private int nbJumpMax = 2;
    [SerializeField] private float jumpForce;
    private Rigidbody2D rb;
    private bool isGrounded = false;

    private void Awake() 
    {
        nbJump = nbJumpMax;
        rb = GetComponent<Rigidbody2D>();
        GroundCheck.onGrounded += ChangeBool;
        Player.onJump += Jump;
    }
    private void ChangeBool(bool value)
    {
        isGrounded = value;
    }

    void Update()
    {
        if (nbJump != nbJumpMax && isGrounded)
        {
            ResetJump();
        }
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

    private void ResetJump() // make the player able to jump again
    {
        nbJump = nbJumpMax;
    }
}
