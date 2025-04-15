using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject soundsPanel;

    [Header("First Buttons")]
    [SerializeField] private GameObject pauseMenuFirstButton;
    [SerializeField] private GameObject optionsFirstButton;
    [SerializeField] private GameObject controlsFirstButton;
    [SerializeField] private GameObject soundsFirstButton;

    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private float timeToPause;

    private UIPanelManager panelManager;
    private GameObject currentFirstButton;

    private string lastControlScheme = ControlScheme.KeyboardMouse.ToString();
    private bool isPaused;
    private Coroutine transitionCoroutine;

    public static Action onStartPause;
    public static Action onCancel;

    private void Awake()
    {
        panelManager = new UIPanelManager(pauseMenuPanel, optionsPanel, controlsPanel, soundsPanel);
    }

    private void Start()
    {
        onStartPause += TogglePause;
        onCancel += HandleCancelInput;

        if (playerInput != null)
            playerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDestroy()
    {
        onStartPause -= TogglePause;
        onCancel -= HandleCancelInput;

        if (playerInput != null)
            playerInput.onControlsChanged -= OnControlsChanged;
    }

    private void OnControlsChanged(PlayerInput input)
    {
        lastControlScheme = input.currentControlScheme;
        if (lastControlScheme == ControlScheme.Joystick.ToString() && currentFirstButton != null)
        {
            SetSelectedUI(currentFirstButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void SetSelectedUI(GameObject button)
    {
        if (lastControlScheme == ControlScheme.Joystick.ToString() && button != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void TogglePause()
    {
        if (transitionCoroutine != null) return;

        if (!isPaused)
            transitionCoroutine = StartCoroutine(TransitionTimeScale(1f, 0f, OnPauseComplete));
        else
            transitionCoroutine = StartCoroutine(TransitionTimeScale(0f, 1f, OnResumeComplete));
    }

    private IEnumerator TransitionTimeScale(float from, float to, Action callback)
    {
        float elapsed = 0f;
        while (elapsed < timeToPause)
        {
            elapsed += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(from, to, elapsed / 1f);
            yield return null;
        }

        Time.timeScale = to;
        transitionCoroutine = null;
        callback?.Invoke();
    }

    private void OnPauseComplete()
    {
        isPaused = true;
        panelManager.ShowOnly(pauseMenuPanel);
        currentFirstButton = pauseMenuFirstButton;

        if (playerInput != null)
            lastControlScheme = playerInput.currentControlScheme;

        SetSelectedUI(currentFirstButton);
        InputManager.onSwitchInputMap?.Invoke(InputActionMap.UI);
    }

    private void OnResumeComplete()
    {
        isPaused = false;
        panelManager.HideAll();
        InputManager.onSwitchInputMap?.Invoke(InputActionMap.Player);
    }

    private void HandleCancelInput()
    {
        if (controlsPanel.activeSelf || soundsPanel.activeSelf)
        {
            OpenOptions();
        }
        else if (optionsPanel.activeSelf)
        {
            OpenPauseMenu();
        }
        else if (pauseMenuPanel.activeSelf)
        {
            onStartPause?.Invoke();
        }
    }

    // === UI Callbacks ===

    public void ResumeGame() => onStartPause?.Invoke();

    public void OpenOptions()
    {
        panelManager.ShowOnly(optionsPanel);
        currentFirstButton = optionsFirstButton;
        SetSelectedUI(currentFirstButton);
    }

    public void OpenPauseMenu()
    {
        panelManager.ShowOnly(pauseMenuPanel);
        currentFirstButton = pauseMenuFirstButton;
        SetSelectedUI(currentFirstButton);
    }

    public void OpenControlPanel()
    {
        panelManager.ShowOnly(controlsPanel);
        currentFirstButton = controlsFirstButton;
        SetSelectedUI(currentFirstButton);
    }

    public void OpenSoundPanel()
    {
        panelManager.ShowOnly(soundsPanel);
        currentFirstButton = soundsFirstButton;
        SetSelectedUI(currentFirstButton);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game called.");
        Application.Quit();
    }
}
