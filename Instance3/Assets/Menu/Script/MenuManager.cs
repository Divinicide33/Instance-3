using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject optionMenu;
    [SerializeField] private GameObject bindMenu;
    [SerializeField] private GameObject soundMenu;

    [Header("First Selected Buttons")]
    [SerializeField] private GameObject selectResetButton;
    [SerializeField] private GameObject newGameMainMenuFirstButton;
    [SerializeField] private GameObject mainMenuFirstButton;
    [SerializeField] private GameObject settingsMenuFirstButton;
    [SerializeField] private GameObject creditsMenuFirstButton;
    [SerializeField] private GameObject controlMenuFirstButton;
    [SerializeField] private GameObject musicMenuFirstButton;

    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;

    private FadeInOut fade;

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

    private void Start()
    {
        fade = FindObjectOfType<FadeInOut>();
    }

    private void OnControlsChanged(PlayerInput input)
    {
        if (IsGamepad(input))
        {
            SelectFirstButton();
            return;
        }

        EventSystem.current.SetSelectedGameObject(null);
    }

    private bool IsGamepad(PlayerInput input)
    {
        return input.currentControlScheme != null &&
                (input.currentControlScheme.ToLower().Contains("gamepad") ||
                input.currentControlScheme.ToLower().Contains("joystick"));
    }

    private void SelectFirstButton()
    {
        if (settingsMenu != null && settingsMenu.activeSelf)
            SetSelected(settingsMenuFirstButton);

        else if (creditsMenu != null && creditsMenu.activeSelf)
            SetSelected(creditsMenuFirstButton);

        else if (optionMenu != null && optionMenu.activeSelf)
            SetSelected(controlMenuFirstButton);

        else if (bindMenu != null && bindMenu.activeSelf)
            SetSelected(controlMenuFirstButton);

        else if (soundMenu != null & soundMenu.activeSelf)
            SetSelected(musicMenuFirstButton);

        else
            SetSelected(mainMenuFirstButton);
    }

    private void SetSelected(GameObject button)
    {
        if (button == null) return;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void PlayGame()
    {
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        yield return fade.FadeIn();
        SceneManager.LoadScene(1);
        fade.FadeOut();
    }

    public void OpenSettings()
    {
        if (IsGamepad(playerInput))
            SetSelected(settingsMenuFirstButton);
    }

    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);

        if (IsGamepad(playerInput))
            SetSelected(creditsMenuFirstButton);
    }

    public void BackToMainMenu()
    {
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        optionMenu.SetActive(false);
        mainMenu.SetActive(true);

        if (IsGamepad(playerInput))
            SetSelected(mainMenuFirstButton);
    }

    public void NewGameMainMenu()
    {
        if (IsGamepad(playerInput))
            SetSelected(newGameMainMenuFirstButton);
    }

    public void SelectResetButton()
    {
        if (IsGamepad(playerInput))
            SetSelected(selectResetButton);
    }

    public void ControlMenuFirstButton()
    {
        if (IsGamepad(playerInput))
            SetSelected(controlMenuFirstButton);
    }

    public void MusicMenuFirstButton()
    {
        if (IsGamepad(playerInput))
            SetSelected(musicMenuFirstButton);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
