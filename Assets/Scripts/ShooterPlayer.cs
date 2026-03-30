using System.Collections;
using Player;
using Unity.Netcode;
using UnityEngine;
using Weapons;
public class ShooterPlayer : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Shooting")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("Feedback")]
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private Color flashColor = new Color(1f, 0.3f, 0.3f);
    [SerializeField] private float flashDuration = 0.12f;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioClip shootSound;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private static readonly int SpeedParam = Animator.StringToHash("Speed");
    private static readonly int ShootParam = Animator.StringToHash("Shoot");
    private static readonly int HitParam = Animator.StringToHash("Hit");
    private static readonly int DieParam = Animator.StringToHash("Die");

    private AudioSource _audioSource;
    private int _currentHealth;
    private Color _originalColor;
    private Coroutine _flashCoroutine;
    private float _nextFireTime;

    private Camera _camera;

    public void SetCamera(Camera cam)
    {
        _camera = cam;
    }

    private void Awake()
    {
        _camera = Camera.main;
        _audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public override void OnNetworkSpawn()
    {
        _currentHealth = maxHealth;

        if (playerRenderer != null)
            _originalColor = playerRenderer.material.color;
    }

    private void Update()
    {
        if (!IsAlive() || !IsOwner)
            return;

        HandleMovement();
        HandleAiming();
        HandleShooting();
        UpdateAnimations();
    }

    private void HandleMovement()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        var direction = new Vector3(horizontal, 0f, vertical).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }

    private void HandleAiming()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            var targetPoint = ray.GetPoint(distance);
            var lookDirection = (targetPoint - transform.position).normalized;
            lookDirection.y = 0f;

            if (lookDirection.sqrMagnitude > 0.001f)
            {
                var targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButton(0) && Time.time >= _nextFireTime)
        {
            var direction = GetAimDirection();
            ShootServerRpc(firePoint.position, direction);
            _nextFireTime = Time.time + fireRate;
        }
    }

    [ServerRpc]
    private void ShootServerRpc(Vector3 position, Vector3 direction)
    {
        var bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        bullet.Init(this, direction);
        bullet.GetComponent<NetworkObject>().Spawn();

        ShootClientRpc();
    }

    [ClientRpc]
    private void ShootClientRpc()
    {
        muzzleFlash.Play();
        _audioSource.PlayOneShot(shootSound);

        SetAnimTrigger(ShootParam);
    }

    private Vector3 GetAimDirection()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            var point = ray.GetPoint(distance);
            var dir = (point - firePoint.position).normalized;
            dir.y = 0f;
            return dir;
        }

        return transform.forward;
    }

    public void TakeDamage(int amount)
    {
        if (!IsAlive()) 
            return;

        _currentHealth = Mathf.Max(0, _currentHealth - amount);

        if (_flashCoroutine != null) 
            StopCoroutine(_flashCoroutine);
        _flashCoroutine = StartCoroutine(FlashDamage());

        SetAnimTrigger(HitParam);

        if (_currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (!IsAlive())
            return;
        _currentHealth = Mathf.Min(maxHealth, _currentHealth + amount);
    }

    private void Die()
    {
        SetAnimTrigger(DieParam);
        gameObject.SetActive(false);
    }

    public void ResetHealth()
    {
        _currentHealth = maxHealth;
        gameObject.SetActive(true);
        playerRenderer.material.color = _originalColor;
            
    }

    private IEnumerator FlashDamage()
    {
        playerRenderer.material.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        playerRenderer.material.color = _originalColor;
    }

    private void UpdateAnimations()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var speed = new Vector2(h, v).magnitude > 0.1f ? 1f : 0f;
        animator.SetFloat(SpeedParam, speed);
    }

    private void SetAnimTrigger(int hash)
    {
        animator.SetTrigger(hash);
    }

    public bool IsAlive() => _currentHealth > 0;
}