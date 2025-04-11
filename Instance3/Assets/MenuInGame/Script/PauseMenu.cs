using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using UnityEngine.InputSystem;


public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject firstSelectedButton;
    [SerializeField] private GameObject optionsFirstButton;
    [SerializeField] private GameObject controlsFirstButton;
    [SerializeField] private GameObject soundFirstButton;


    [Header("Settings")]
    [SerializeField] private float transitionDuration = 1f;

    private PlayerInput playerInput;


    public static Action onStartPause;
    public static Action onCancel;

    private bool isPaused = false;
    private Coroutine transitionCoroutine;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        onStartPause += TogglePause;
        onCancel += HandleCancelInput;

    }

    private void OnDestroy()
    {
        onStartPause -= TogglePause;
        onCancel -= HandleCancelInput;

    }

    public void TogglePause()
    {
        if (transitionCoroutine != null) return;

        if (!isPaused)
            transitionCoroutine = StartCoroutine(RampTimeScale(1f, 0f, OnPauseComplete));
        else
            transitionCoroutine = StartCoroutine(RampTimeScale(0f, 1f, OnResumeComplete));
    }

    private IEnumerator RampTimeScale(float from, float to, Action onComplete)
    {
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            Time.timeScale = Mathf.Lerp(from, to, t);
            yield return null;
        }

        Time.timeScale = to;
        transitionCoroutine = null;
        onComplete?.Invoke();
    }

    private void OnPauseComplete()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        SetSelectedUI(firstSelectedButton);
        InputManager.onSwitchInputMap?.Invoke(InputActionMap.UI);
    }

    private void OnResumeComplete()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        InputManager.onSwitchInputMap?.Invoke(InputActionMap.Player);
    }

    private void SetSelectedUI(GameObject button)
    {
        if (button == null) return;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }

    private void OpenPanel(GameObject panel, GameObject firstButton)
    {
        if (panel == null) return;

        panel.SetActive(true);

        if (playerInput != null && playerInput.currentControlScheme == "Joystick")
        {
            SetSelectedUI(firstButton);
        }
    }
    private void ClosePanel(GameObject panel)
    {
        if (panel != null)
            panel.SetActive(false);
    }


    private void HandleCancelInput()
    {
        if (optionsPanel != null && optionsPanel.activeSelf)
        {
            CloseOptions();
        }
        else if (controlsPanel != null && controlsPanel.activeSelf)
        {
            CloseControls();
        }
        else if (soundPanel != null && soundPanel.activeSelf)
        {
            CloseSounds();
        }
        else
        {
            onStartPause?.Invoke();
        }
    }


    #region Submenus

    public void OpenOptions() => OpenPanel(optionsPanel, optionsFirstButton);
    public void CloseOptions() => ClosePanel(optionsPanel);

    public void OpenControls() => OpenPanel(controlsPanel, controlsFirstButton);
    public void CloseControls() => ClosePanel(controlsPanel);

    public void OpenSounds() => OpenPanel(soundPanel, soundFirstButton);
    public void CloseSounds() => ClosePanel(soundPanel);

    private void ShowPanel(GameObject panel)
    {
        if (panel != null)
            panel.SetActive(true);
    }

    private void HidePanel(GameObject panel)
    {
        if (panel != null)
            panel.SetActive(false);
    }

    #endregion
}
