
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float shootRange = 100f;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioSource audioSource;

    private Camera playerCamera;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        audioSource.playOnAwake = false;
    }

    private void Start()
    {
        playerCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        Debug.DrawRay(
            ray.origin,
            ray.direction * shootRange,
            Color.red,
            5f
        );

        Debug.Log("Raycast ditembakkan!");

        if (Physics.Raycast(ray, out RaycastHit hit, shootRange))
        {
            Debug.Log("Terkena objek: " + hit.collider.name);

            ZombieAI zombie = hit.collider.GetComponentInParent<ZombieAI>();
            if (zombie != null)
            {
                zombie.Alert();
                Debug.Log("Zombie tertembak, mulai mengejar hero!");
            }
        }
    }
}
