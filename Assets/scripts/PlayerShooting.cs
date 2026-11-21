
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float shootRange = 100f;

    private Camera playerCamera;

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
