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
    private string lastControlScheme = ControlScheme.KeyboardMouse.ToString();

    public static Action onVictory;

    private FadeInOut fade;

    private void Start()
    {
        fade = FindObjectOfType<FadeInOut>();
    }

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

    public void RestartGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        yield return fade.FadeIn();
        SceneManager.LoadScene(1);
        fade.FadeOut();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
