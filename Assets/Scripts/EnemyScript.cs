using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
  [Header("References")]
  public Transform[] waypoints;
  public Transform player;
  public GameObject bulletPrefab;
  public GameObject muzzleFlashPrefab;
  public Transform firePoint;
  public Animator animator;

  [Header("Stats")]
  public float viewDistance = 30f;
  public float viewAngle = 60f;
  public float stoppingDistance = 10f;
  public float fireRate = 1f;
  public float bulletSpeed = 50f;
  public float patrolSpeed = 3f;
  public float chaseSpeed = 5f;
  public int maxHealth = 1;
  public float waypointWaitTime = 2f;

  private int currentHealth;
  private NavMeshAgent agent;
  private int currentWaypoint = 0;
  private float fireTimer = 0f;
  private bool isDead = false;

  // Waiting logic
  private bool isWaiting = false;
  private float waitTimer = 0f;

  void Start()
  {
    agent = GetComponent<NavMeshAgent>();
    currentHealth = maxHealth;

    if (waypoints.Length > 0)
      agent.SetDestination(waypoints[0].position);
  }

  void Update()
  {
    if (isDead) return;

    fireTimer += Time.deltaTime;
    animator.SetFloat("Speed", agent.velocity.magnitude);

    float distanceToPlayer = Vector3.Distance(transform.position, player.position);

    if (CanSeePlayer())
    {
      isWaiting = false;
      agent.isStopped = false;

      if (distanceToPlayer > stoppingDistance)
      {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
        animator.SetBool("isAiming", false);
      }
      else
      {
        agent.ResetPath();
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        animator.SetBool("isAiming", true);

        if (fireTimer >= fireRate)
        {
          Shoot();
          fireTimer = 0f;
        }
      }
    }
    else
    {
      animator.SetBool("isAiming", false);
      Patrol();
    }
  }

  void Patrol()
  {
    if (waypoints.Length == 0) return;

    agent.speed = patrolSpeed;

    if (isWaiting)
    {
      waitTimer += Time.deltaTime;
      if (waitTimer >= waypointWaitTime)
      {
        isWaiting = false;
        GoToNextWaypoint();
      }
      else
      {
        agent.isStopped = true;
      }
    }
    else
    {
      if (!agent.pathPending && agent.remainingDistance < 0.5f)
      {
        isWaiting = true;
        waitTimer = 0f;
      }
    }
  }

  void GoToNextWaypoint()
  {
    agent.isStopped = false;
    currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    agent.SetDestination(waypoints[currentWaypoint].position);
  }

  bool CanSeePlayer()
  {
    if (!player) return false;

    Vector3 dirToPlayer = (player.position - transform.position).normalized;
    float angle = Vector3.Angle(transform.forward, dirToPlayer);

    if (Vector3.Distance(transform.position, player.position) < viewDistance && angle < viewAngle / 2f)
    {
      if (Physics.SphereCast(transform.position + Vector3.up, 0.5f, dirToPlayer, out RaycastHit hit, viewDistance))
      {
        return hit.transform.CompareTag("Player");
      }
    }

    return false;
  }

  void Shoot()
  {
    if (!player) return;

    Vector3 target = player.GetComponent<PlayerController>()?.camPitchRoot.position ?? player.position;
    Vector3 dir = (target - firePoint.position).normalized;

    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(dir));

    if (bullet.TryGetComponent(out Rigidbody rb))
      rb.linearVelocity = dir * bulletSpeed;

    if (bullet.TryGetComponent(out Bullet b))
      b.shooter = gameObject;

    if (muzzleFlashPrefab)
      Destroy(Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation), 0.1f);
  }

  public void TakeDamage(int damage)
  {
    if (isDead) return;

    currentHealth -= damage;

    if (currentHealth <= 0)
    {
      Die();
    }
  }

  void Die()
  {
    KillCounter.Instance?.AddKill(); // ✅ נרשמת רק לזיכרון זמני (ולא ל־PlayerPrefs)
    isDead = true;
    animator.SetBool("isDead", true);
    agent.isStopped = true;
    GetComponent<Collider>().enabled = false;
    Destroy(gameObject, 3f);
  }
}