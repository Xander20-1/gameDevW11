
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class Sc_hero : MonoBehaviour
{
    private NavMeshAgent agent;
    private Camera mainCamera;

    [SerializeField] private float rayDistance = 1000f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Check whether a mouse is connected
        if (Mouse.current == null)
            return;

        // Detect left mouse click
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            MoveToMousePosition();
        }
    }

    private void MoveToMousePosition()
    {
        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera tidak ditemukan!");
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("NavMeshAgent tidak berada di NavMesh!");
            return;
        }

        // Convert mouse position into a ray
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        // Detect objects using Raycast
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            // Set destination for NavMeshAgent
            bool success = agent.SetDestination(hit.point);

            if (!success)
            {
                Debug.LogWarning("Destination gagal ditemukan!");
            }
        }
    }
}
