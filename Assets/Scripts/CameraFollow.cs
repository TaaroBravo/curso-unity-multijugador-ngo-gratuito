using System;
using UnityEngine;

namespace Player
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 12f, -8f);
        [SerializeField] private float smoothSpeed = 8f;

        private void Start()
        {
            var players = FindObjectsByType<ShooterPlayer>(FindObjectsSortMode.None);
            foreach (var player in players)
            {
                if (player.IsOwner)
                {
                    target = player.transform;
                    player.SetCamera(GetComponent<Camera>());
                }
            }
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        private void LateUpdate()
        {
            if (target == null) 
                return;

            var desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position,desiredPosition,smoothSpeed * Time.deltaTime);
        }
    }
}
