using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    public static Action<InputActionMap> onSwitchInputMap;

    private void Start()
    {
        onSwitchInputMap += SwitchActionMap;

    }
    private void OnDestroy()
    {
        onSwitchInputMap -= SwitchActionMap;
    }
    private void SwitchActionMap(InputActionMap newActionMap)
    {
        playerInput.SwitchCurrentActionMap(newActionMap.ToString());
    }


}
public enum InputActionMap 
{ 
    UI, 
    Player
}

public enum ControlScheme
{
    Joystick,
    KeyboardMouse
}