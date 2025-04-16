using UnityEngine;

namespace AI.Zeus
{
    public class ZeusLightningBehavior : MonoBehaviour
    {
        public float speed = 10f;
        public int damage = 1;

        private Vector2 direction;
        private float lifetime = 2f; // Dur�e de vie de l'�clair avant suppression

        public void StartLightning(Vector2 lightningDirection)
        {
            direction = lightningDirection;
            Destroy(gameObject, lifetime); // D�truit l'�clair apr�s sa dur�e de vie
        }

        void Update()
        {
            // Se d�place si l'�clair n'est pas encore d�truit
            if (gameObject != null)
            {
                transform.Translate(direction * speed * Time.deltaTime);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            // D�truire l'�clair si il entre en collision avec un objet
            Destroy(gameObject);
        }
    }
}
