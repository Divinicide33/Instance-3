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

    [Header("First Selected Buttons")]
    [SerializeField] private GameObject mainMenuFirstButton;
    [SerializeField] private GameObject settingsMenuFirstButton;
    [SerializeField] private GameObject creditsMenuFirstButton;

    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;

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
        // detect if using gamepad
        if (input.currentControlScheme == ControlScheme.Joystick.ToString())
        {
            SelectFirstButton();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void SelectFirstButton()
    {
        if (settingsMenu.activeSelf)
            SetSelected(settingsMenuFirstButton);
        else if (creditsMenu.activeSelf)
            SetSelected(creditsMenuFirstButton);
        else
            SetSelected(mainMenuFirstButton);
    }

    private FadeInOut fade;

    private void Start()
    {
        fade = FindObjectOfType<FadeInOut>();
    }

    private IEnumerator _ChangeScene()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);

    }

    private void SetSelected(GameObject button)
    {
        if (button == null) return;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void PlayGame()
    {
        StartCoroutine(_ChangeScene());
    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);

        if (playerInput.currentControlScheme == ControlScheme.Joystick.ToString())
            SetSelected(settingsMenuFirstButton);
    }

    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);

        if (playerInput.currentControlScheme == ControlScheme.Joystick.ToString())
            SetSelected(creditsMenuFirstButton);
    }

    public void BackToMainMenu()
    {
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);

        if (playerInput.currentControlScheme == ControlScheme.Joystick.ToString())
            SetSelected(mainMenuFirstButton);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
