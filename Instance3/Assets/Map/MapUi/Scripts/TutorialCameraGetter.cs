using UnityEngine;
public class TutorialCameraGetter : MonoBehaviour
{
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private Camera playerCamera;

    void Start()
    {
        if (uiCanvas.renderMode == RenderMode.ScreenSpaceCamera)
            uiCanvas.worldCamera = playerCamera;
    }
}