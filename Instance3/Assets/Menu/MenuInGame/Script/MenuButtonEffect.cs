using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    private Vector3 originalScale;
    public float scaleFactor = 1.2f;
    private bool isHoveredOrSelected = false;
    private Transform secondChild;

    void Start()
    {
        originalScale = transform.localScale;


        if (originalScale == Vector3.zero)
        {
            originalScale = new Vector3(1f, 1f, 1f);
            isHoveredOrSelected = true;
            ApplyEffect();
            transform.localScale = originalScale;
        }

        Debug.Log(originalScale);

        if (transform.childCount >= 2)
        {
            secondChild = transform.GetChild(1);
            secondChild.gameObject.SetActive(false); // Désactivé au départ
        }
        else
        {
            Debug.LogWarning("MenuButtonEffect : L'objet n'a pas de deuxième enfant !");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHoveredOrSelected = true;
        ApplyEffect();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isHoveredOrSelected = false;
        RemoveEffect();
    }

    public void OnSelect(BaseEventData eventData)
    {
        isHoveredOrSelected = true;
        ApplyEffect();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isHoveredOrSelected = false;
        RemoveEffect();
    }

    public void ApplyEffect()
    {
        transform.localScale = originalScale * scaleFactor;
        if (secondChild != null)
            secondChild.gameObject.SetActive(true);
    }

    public void RemoveEffect()
    {
        transform.localScale = originalScale;
        if (secondChild != null)
            secondChild.gameObject.SetActive(false);
    }
}
