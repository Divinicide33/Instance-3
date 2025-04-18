using System.Collections;
using UnityEngine;

public class MoveFX : FxElement<MoveFX>
{
    public new ParticleSystem particleSystem;

    private bool isGrounded = false;
    private Vector2 isMoving;
    private bool isVFXPlaying = false;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        particleSystem.Stop();
    }

    protected override void Show()
    {

    }

    protected override void Hide()
    {

    }

    protected override void UpdateFX()
    {

    }

    public void checkGround(bool playerOnGround)
    {
        isGrounded = playerOnGround;
    }

    private void checkMove(Vector2 move)
    {
        isMoving = move;
    }

    private void MoveVFX(Vector2 playerMove)
    {
        bool shouldPlayVFX = playerMove.x != 0 && isGrounded;

        if (shouldPlayVFX && !isVFXPlaying)
        {
            particleSystem.Play();
            isVFXPlaying = true;
        }
        else if (!shouldPlayVFX && isVFXPlaying)
        {
            particleSystem.Stop();
            isVFXPlaying = false;
        }
    }

    private void FixedUpdate()
    {
        MoveVFX(isMoving);
    }
}
