using UnityEngine;

namespace AI.Zeus
{
    public class ZeusLightningBehavior : MonoBehaviour
    {
        public float speed = 10f;
        public int damage = 1;

        private Vector2 direction;
        private float lifetime = 2f; // Durée de vie de l'éclair avant suppression

        public void StartLightning(Vector2 lightningDirection)
        {
            direction = lightningDirection;
            Destroy(gameObject, lifetime); // Détruit l'éclair après sa durée de vie
        }

        void Update()
        {
            // Se déplace si l'éclair n'est pas encore détruit
            if (gameObject != null)
            {
                transform.Translate(direction * speed * Time.deltaTime);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            // Détruire l'éclair si il entre en collision avec un objet
            Destroy(gameObject);
        }
    }
}
