using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(PlayerController))]
public class PlayerInputScript : PlayerController
{
    private bool isMoving = false;
    private bool isEnable = true;
    public static Action onEnableInput { get; set; }
    public static Action onDisableInput { get; set; }
    [SerializeField] private float offSetInput = 0.5f;
    private PlayerController player;
    
    private void OnEnable()
    {
        if (player == null)
            player = GetComponent<PlayerController>();

        onEnableInput += EnableInput;
        onDisableInput += DisableInput;
    }

    private void OnDisable()
    {
        onEnableInput -= EnableInput;
        onDisableInput -= DisableInput;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started && !isMoving)
        {
            isMoving = true;
            PlayerAnimator.onSetIsMoving?.Invoke(isMoving);
        }
        else if (context.canceled)
        {
            isMoving = false;
            PlayerAnimator.onSetIsMoving?.Invoke(isMoving);
        }

        Vector2 direction = context.ReadValue<Vector2>();

        if (direction.x > offSetInput) 
        {
            player.isFacingRight = true;
            PlayerAnimator.onSetIsFacingRight?.Invoke(player.isFacingRight);
            direction.x = 1;
        }
        else if (direction.x < -offSetInput) 
        {
            player.isFacingRight = false;
            PlayerAnimator.onSetIsFacingRight?.Invoke(player.isFacingRight);
            direction.x = -1;
        }

        if (direction.y > offSetInput)
        {
            player.isFacingUp = true;
            PlayerAnimator.onSetIsFacingUp?.Invoke(player.isFacingUp);
        }
        else
        {
            player.isFacingUp = false;
            PlayerAnimator.onSetIsFacingUp?.Invoke(player.isFacingUp);
        }

        if (isMoving)
        {
            PlayerController.onMove?.Invoke(direction);
            return;
        }

        PlayerController.onMove?.Invoke(Vector2.zero);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerController.onGlide?.Invoke(true);
        
            if (!isEnable) 
                return;

            PlayerController.onJump?.Invoke(true);
        }
        else if (context.canceled)
        {
            PlayerController.onGlide?.Invoke(false);

            if (!isEnable) 
                return;

            PlayerController.onJump?.Invoke(false);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!isEnable) 
            return;

        if (context.started)
            PlayerController.onDash?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!isEnable)
            return;

        if (context.started)
            PlayerController.onAttack?.Invoke();
    }

    public void OnUsePotion(InputAction.CallbackContext context)
    {
        if (!isEnable) 
            return;

        if (context.started)
            PlayerController.onUsePotion?.Invoke();
    }

    private void EnableInput()
    {
        if (isDead) 
            return;

        isEnable = true;
        PlayerMove.onSetMove?.Invoke(true);
        PlayerGlide.onCanGlide?.Invoke(true);
    }

    private void DisableInput()
    {
        isEnable = false;
        PlayerDash.onStopDash?.Invoke();
        PlayerAttack.onStopAction?.Invoke();
        PlayerMove.onSetMove?.Invoke(false);
        PlayerGlide.onCanGlide?.Invoke(false);
    }

    public void StartPause(InputAction.CallbackContext context)
    {
        if (context.started)
            PauseMenu.onStartPause?.Invoke();
    }

    public void HandleCancel(InputAction.CallbackContext context)
    {
        if (context.started)
            PauseMenu.onCancel?.Invoke();
    }
}
