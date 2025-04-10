using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;


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
        SetSelected(settingsMenuFirstButton);
    }

    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
        SetSelected(creditsMenuFirstButton);
    }

    public void BackToMainMenu()
    {
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
        SetSelected(mainMenuFirstButton);
    }


    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
