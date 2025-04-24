using UnityEngine;
using UnityEngine.InputSystem;
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

    void Start()
    {
        ShowMenu(0);
    }

    void Update()
    {
        HandleInput();
        UpdateSelection();
    }

    void HandleInput()
    {
        isUsingController = Input.GetJoystickNames().Length > 0;

        if (isUsingController)
            HandleControllerInput();
        else
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
            var previousElement = menus[currentMenuIndex].menuElements[currentElementIndex];
            DeselectButton(previousElement);

            currentElementIndex = Mathf.Max(0, currentElementIndex - 1);
            lastInputTime = Time.time;
        }
        else if (input.y < -0.5f) // Down
        {
            var previousElement = menus[currentMenuIndex].menuElements[currentElementIndex];
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

    void HandleControllerInput()
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
            var buttonHoverImage = selectedButton.GetComponent<ButtonHoverImage>();
            if (buttonHoverImage != null)
            {
                buttonHoverImage.OnSelect(null); // Appelle OnSelect pour afficher l'image du survol
            }
        }
    }

    void HandleMouseInput()
    {
        var mb = menus[currentMenuIndex].menuElements;
        for (int i = 0; i < mb.Length; i++)
        {
            var rt = mb[i].GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition))
            {
                var previousElement = menus[currentMenuIndex].menuElements[currentElementIndex];
                DeselectButton(previousElement);

                currentElementIndex = i;
                var selectedElement = menus[currentMenuIndex].menuElements[currentElementIndex];

                if (selectedElement is Button selectedButton)
                {
                    var buttonHoverImage = selectedButton.GetComponent<ButtonHoverImage>();
                    if (buttonHoverImage != null)
                    {
                        buttonHoverImage.OnSelect(null);
                    }
                }
                break;
            }
        }
    }

    public void BackToPlay()
    {
        ShowMenu(0);
        UpdateSelection();
    }

    public void ShowMenu(int index)
    {
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

    void UpdateSelection()
    {
        foreach (var element in menus[currentMenuIndex].menuElements)
        {
            if (element is Button button)
            {
                button.GetComponent<MenuButtonEffect>()?.RemoveEffect();
                var buttonHoverImage = button.GetComponent<ButtonHoverImage>();
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
                var buttonHoverImage = button.GetComponent<ButtonHoverImage>();
                if (buttonHoverImage != null)
                {
                    buttonHoverImage.OnSelect(null);
                }
            }
        }
    }

    void DeselectButton(Selectable element)
    {
        if (element is Button button)
        {
            var buttonHoverImage = button.GetComponent<ButtonHoverImage>();
            if (buttonHoverImage != null)
            {
                buttonHoverImage.OnDeselect(null);
            }
        }
    }
}
