using UnityEngine;

namespace Weapons
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 30f;
        [SerializeField] private float lifetime = 3f;
        [SerializeField] private int damage = 25;
        [SerializeField] private GameObject hitEffectPrefab;

        private ShooterPlayer _shooter;

        public void Init(ShooterPlayer shooter, Vector3 direction)
        {
            _shooter = shooter;
            direction.y = 0f;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_shooter != null && other.transform.IsChildOf(_shooter.transform))
                return;

            if (other.TryGetComponent<ShooterPlayer>(out var player))
                player.TakeDamage(damage);

            var effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 2f);

            Destroy(gameObject);
        }
    }
}
