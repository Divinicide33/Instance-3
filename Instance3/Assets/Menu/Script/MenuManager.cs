using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [System.Serializable]
    public class Menu
    {
        public string menuName;
        public GameObject menuRoot;
        public Selectable[] menuElements;
    }

    public Menu[] menus;

    private int currentMenuIndex = 0;
    private int currentElementIndex = 0;

    private bool isUsingController = false;
    [SerializeField] float inputDelay = 0.2f;
    private float lastInputTime;
    private Selectable selectedElement;
    private FadeInOut fade;

    private void Start()
    {
        fade = FindObjectOfType<FadeInOut>();
        ShowMenu(0);
    }

    private void Update()
    {
        if (menus.Length == 0) 
            return;
        
        HandleInput();
        UpdateSelection();
    }

    private void HandleInput()
    {
        if (Input.GetJoystickNames().Length > 0)
        {
            HandleControllerInput();
        }

        HandleMouseInput();
    }

    public void Submit(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (selectedElement is Button button)
            {
                button.onClick.Invoke();
            }
        }
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (!context.performed || Time.time - lastInputTime < inputDelay)
            return;

        Vector2 input = context.ReadValue<Vector2>();

        if (input.y > 0.5f) // Up
        {
            Selectable previousElement = menus[currentMenuIndex].menuElements[currentElementIndex];
            DeselectButton(previousElement);

            currentElementIndex = Mathf.Max(0, currentElementIndex - 1);
            lastInputTime = Time.time;
        }
        else if (input.y < -0.5f) // Down
        {
            Selectable previousElement = menus[currentMenuIndex].menuElements[currentElementIndex];
            DeselectButton(previousElement);

            currentElementIndex = Mathf.Min(menus[currentMenuIndex].menuElements.Length - 1, currentElementIndex + 1);
            lastInputTime = Time.time;
        }
    }

    public void Back(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ShowMenu(0);
        }
    }

    private void HandleControllerInput()
    {
        selectedElement = menus[currentMenuIndex].menuElements[currentElementIndex];

        if (selectedElement is Slider slider)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            slider.value += horizontalInput * 0.01f;
            slider.value = Mathf.Clamp(slider.value, slider.minValue, slider.maxValue);
        }

        if (selectedElement is Button selectedButton)
        {
            ButtonHoverImage buttonHoverImage = selectedButton.GetComponent<ButtonHoverImage>();
            if (buttonHoverImage != null)
            {
                buttonHoverImage.OnSelect(null);
            }
        }
    }

    private void HandleMouseInput()
    {
        Selectable[] menuElements = menus[currentMenuIndex].menuElements;

        bool foundHover = false;

        for (int i = 0; i < menuElements.Length; i++)
        {
            Selectable element = menuElements[i];
            RectTransform rectTransform = element.GetComponent<RectTransform>();

            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
            {
                foundHover = true;

                if (currentElementIndex != i)
                {
                    Selectable previousElement = menuElements[currentElementIndex];
                    DeselectButton(previousElement);

                    currentElementIndex = i;
                    selectedElement = menuElements[currentElementIndex];

                    EventSystem.current.SetSelectedGameObject(selectedElement.gameObject);
                    UpdateSelection();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (selectedElement is Button button)
                    {
                        button.onClick.Invoke();
                    }
                    else if (selectedElement is Slider slider)
                    {
                        Vector2 localMousePos;
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out localMousePos);
                        float normalized = Mathf.InverseLerp(-rectTransform.rect.width / 2, rectTransform.rect.width / 2, localMousePos.x);
                        slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, normalized);
                    }
                }

                break;
            }
        }

        if (!foundHover && selectedElement != null)
        {
            DeselectButton(selectedElement);
            EventSystem.current.SetSelectedGameObject(null);
            selectedElement = null;
        }
    }





    public void BackToPlay()
    {
        ShowMenu(0);
        UpdateSelection();
    }

    public void ShowMenu(int index)
    {
        if (menus.Length == 0)
            return;
        
        if (index < 0 || index >= menus.Length)
            return;

        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuRoot != null)
                menus[i].menuRoot.SetActive(i == index);
        }

        currentMenuIndex = index;
        currentElementIndex = 0;

        UpdateSelection();
    }

    private void UpdateSelection()
    {
        foreach (Selectable element in menus[currentMenuIndex].menuElements)
        {
            if (element is Button button)
            {
                button.GetComponent<MenuButtonEffect>()?.RemoveEffect();
                ButtonHoverImage buttonHoverImage = button.GetComponent<ButtonHoverImage>();
                if (buttonHoverImage != null)
                {
                    buttonHoverImage.OnDeselect(null);
                }
            }
        }

        if (menus[currentMenuIndex].menuElements.Length > 0)
        {
            selectedElement = menus[currentMenuIndex].menuElements[currentElementIndex];

            if (selectedElement is Button button)
            {
                button.GetComponent<MenuButtonEffect>()?.ApplyEffect();
                ButtonHoverImage buttonHoverImage = button.GetComponent<ButtonHoverImage>();
                if (buttonHoverImage != null)
                {
                    buttonHoverImage.OnSelect(null);
                }
            }
        }
    }

    private void DeselectButton(Selectable element)
    {
        if (element is Button button)
        {
            ButtonHoverImage buttonHoverImage = button.GetComponent<ButtonHoverImage>();
            if (buttonHoverImage != null)
            {
                buttonHoverImage.OnDeselect(null);
            }
        }
    }
    
    public void PlayGame()
    {
        StartCoroutine(ChangeScene(1));
    }

    private IEnumerator ChangeScene(int sceneValue)
    {
        yield return fade.FadeIn();
        SceneManager.LoadScene(sceneValue);
        fade.FadeOut();
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void BackToMainMenu()
    {
        StartCoroutine(ChangeScene(0));
    }
}
