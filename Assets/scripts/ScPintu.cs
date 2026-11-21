
using UnityEngine;

public class ScPintu : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private float openDistance = 3f;
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] private float detectionDistance = 3f;
    [SerializeField] private float closeDistance = 5f;

    private Vector3 posPintuAwal;
    private Vector3 posPintuBuka;

    private GameObject hero;

    private void Start()
    {
        // Save initial door position
        posPintuAwal = transform.position;

        // Calculate open position
        posPintuBuka = posPintuAwal;
        posPintuBuka.x += openDistance;

        // Find player
        hero = GameObject.Find("hero");

        if (hero == null)
        {
            Debug.LogError("GameObject 'hero' tidak ditemukan!");
        }
    }

    private void Update()
    {
        if (hero == null) return;

        // Calculate distance between hero and door
        float jarakHero = Vector3.Distance(
            hero.transform.position,
            transform.position
        );

        // Open door
        if (jarakHero < detectionDistance)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                posPintuBuka,
                openSpeed * Time.deltaTime
            );
        }

        // Close door
        else if (jarakHero > closeDistance)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                posPintuAwal,
                openSpeed * Time.deltaTime
            );
        }
    }
}
