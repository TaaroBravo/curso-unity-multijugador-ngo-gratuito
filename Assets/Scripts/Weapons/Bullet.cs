using Unity.Netcode;
using UnityEngine;

namespace Weapons
{
    public class Bullet : NetworkBehaviour
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

        public override void OnNetworkSpawn()
        {
            if(IsServer)
                Invoke(nameof(DestroyBulletAfterTime), lifetime);
        }

        private void DestroyBulletAfterTime()
        {
            GetComponent<NetworkObject>().Despawn();
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer)
                return;
            
            if (_shooter != null && other.transform.IsChildOf(_shooter.transform))
                return;

            if (other.TryGetComponent<ShooterPlayer>(out var player))
                player.TakeDamage(damage);

            HitEffectClientRpc(transform.position);
            
            GetComponent<NetworkObject>().Despawn();
        }

        [ClientRpc]
        private void HitEffectClientRpc(Vector3 position)
        {
            var effect = Instantiate(hitEffectPrefab, position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }
}
