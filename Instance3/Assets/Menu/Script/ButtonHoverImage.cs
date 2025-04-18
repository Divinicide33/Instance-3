using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private GameObject hoverImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverImage != null)
            hoverImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverImage != null)
            hoverImage.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (hoverImage != null)
            hoverImage.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (hoverImage != null)
            hoverImage.SetActive(false);
    }
}
