
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform hero;

    [Header("Detection")]
    [SerializeField] private float sightRange = 12f;
    [SerializeField] private float attackRange = 2f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1.5f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool alerted;
    private float lastAttackTime = -999f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        // Copies made in the scene may lose the inspector reference
        if (hero == null)
        {
            GameObject heroObject = GameObject.Find("hero");
            if (heroObject != null) hero = heroObject.transform;
        }
    }

    // Called by PlayerShooting when the raycast hits this zombie
    public void Alert()
    {
        alerted = true;
    }

    private void Update()
    {
        if (hero == null || agent == null || !agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, hero.position);

        // Chase only after the zombie sees the hero or gets shot
        if (!alerted && distance <= sightRange && CanSeeHero())
        {
            alerted = true;
        }

        if (!alerted)
        {
            animator.SetBool("IsMoving", false);
            return;
        }

        if (distance <= attackRange)
        {
            agent.isStopped = true;
            animator.SetBool("IsMoving", false);
            FaceHero();

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetTrigger("Attack");
                lastAttackTime = Time.time;
                Debug.Log(gameObject.name + " menyerang hero!");
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(hero.position);
            animator.SetBool("IsMoving", agent.velocity.magnitude > 0.1f);
        }
    }

    private bool CanSeeHero()
    {
        Vector3 eye = transform.position + Vector3.up * 1.4f;
        int walls = ~LayerMask.GetMask("pemain", "zombie");
        return !Physics.Linecast(eye, hero.position + Vector3.up, walls);
    }

    private void FaceHero()
    {
        Vector3 direction = hero.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                10f * Time.deltaTime
            );
        }
    }
}
