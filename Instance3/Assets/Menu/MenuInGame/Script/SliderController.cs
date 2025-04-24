using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider slider; // Le slider à contrôler
    public float sliderSpeed = 0.01f; // La vitesse de changement de valeur du slider

    void Update()
    {
        // Si on utilise la manette, contrôler le slider avec les boutons haut et bas
        if (Input.GetAxis("Horizontal") > 0.5f) // Appuyer sur le bouton du haut ou joystick vers le haut
        {
            slider.value += sliderSpeed;
        }
        else if (Input.GetAxis("Horizontal") < -0.5f) // Appuyer sur le bouton du bas ou joystick vers le bas
        {
            slider.value -= sliderSpeed;
        }
    }
}
