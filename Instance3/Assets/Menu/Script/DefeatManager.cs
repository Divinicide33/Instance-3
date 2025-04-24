using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DefeatMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject defeatPanel;

    [Header("First Buttons")]
    [SerializeField] private GameObject defeatFirstButton;

    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private float timeToPause = 0.5f;

    private UIPanelManager panelManager;
    private GameObject currentFirstButton;

    private string lastControlScheme = ControlScheme.KeyboardMouse.ToString();
    private Coroutine transitionCoroutine;

    private bool isDefeatShown = false;

    public static Action onDefeat;

    private void Awake()
    {
        panelManager = new UIPanelManager(defeatPanel);
    }

    private void Start()
    {
        onDefeat += TriggerDefeat;

        if (playerInput != null)
            playerInput.onControlsChanged += OnControlsChanged;

        panelManager.HideAll();
    }

    private void OnDestroy()
    {
        onDefeat -= TriggerDefeat;

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

    public static void LooseGame() => onDefeat?.Invoke();

    private void TriggerDefeat()
    {
        if (isDefeatShown) return;

        isDefeatShown = true;
        Time.timeScale = 0f;
        ShowDefeatPanel();
    }

    private void ShowDefeatPanel()
    {
        panelManager.ShowOnly(defeatPanel);
        currentFirstButton = defeatFirstButton;

        if (playerInput != null)
            lastControlScheme = playerInput.currentControlScheme;

        SetSelectedUI(currentFirstButton);
    }

    private void SetSelectedUI(GameObject button)
    {
        if (button == null) return;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }

    // === UI Callbacks ===

    public void RetryLevel()
    {
        print("TP JOUEUR OU JSP");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game called.");
        Application.Quit();
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ShowDefeatPanel();
        }
    }*/
}
