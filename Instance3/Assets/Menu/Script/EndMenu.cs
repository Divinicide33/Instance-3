using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject menuEndFirstButton;

    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Scene")]
    [SerializeField] private int nextSceneIndex = 1;

    private string lastControlScheme = ControlScheme.KeyboardMouse.ToString();

    public static Action onVictory;


    private void OnEnable()
    {
        if (playerInput != null)
            playerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDisable()
    {
        if (playerInput != null)
            playerInput.onControlsChanged -= OnControlsChanged;
    }

    private void OnControlsChanged(PlayerInput input)
    {
        lastControlScheme = input.currentControlScheme;

        if (lastControlScheme == ControlScheme.Joystick.ToString())
        {
            SetSelected(menuEndFirstButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void SetSelected(GameObject button)
    {
        if (button == null) return;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void LoadNextScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
