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
    private bool isPause = false;
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

    private Vector2 SetDirection(Vector2 input)
    {
        Vector2 direction = input;

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

        if (direction.y > offSetInput)
        {
            player.isFacingUp = true;
        }
        else
        {
            player.isFacingUp = false;
        }

        return direction;
    }

    private void ForAnimation()
    {
        PlayerAnimator.onSetIsFacingRight?.Invoke(player.isFacingRight);
        PlayerAnimator.onSetIsFacingUp?.Invoke(player.isFacingUp);
        PlayerAnimator.onSetIsMoving?.Invoke(isMoving);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started && !isMoving)
        {
            isMoving = true;
        }
        else if (context.canceled)
        {
            isMoving = false;
        }

        Vector2 direction = SetDirection(context.ReadValue<Vector2>());

        if (isEnable)
            ForAnimation();

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
        ForAnimation();
    }

    private void DisableInput()
    {
        isEnable = false;
        PlayerDash.onStopDash?.Invoke();
        PlayerAttack.onStopAction?.Invoke();
        PlayerMove.onSetMove?.Invoke(false);
        PlayerGlide.onCanGlide?.Invoke(false);
        PlayerAnimator.onSetIsMoving?.Invoke(false);
    }

    public void StartPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!isPause)
            {
                isPause = true;
                DisableInput();
                MapManager.instance.CloseLargeMap();
                PauseMenu.onStartPause?.Invoke();
            }
            else
            {
                isPause = false;
                EnableInput();
                PauseMenu.onCancel?.Invoke();
            } 
        }
    }

    public void OnOpenMap(InputAction.CallbackContext context)
    {
        if (!isEnable)
            return;

        if (context.started)
        {
            MapManager.instance.OpenMap();
        }
    }
}
