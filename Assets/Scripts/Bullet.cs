using UnityEngine;

public class Bullet : MonoBehaviour
{
  public GameObject shooter;
  public int damage = 10;
  public float lifeTime = 4f;

  void Start()
  {
    Destroy(gameObject, lifeTime);
  }

  void OnCollisionEnter(Collision col)
  {
    // מניעת פגיעה עצמית מוחלטת
    if (col.gameObject == shooter || col.transform.root.gameObject == shooter)
    {
      Debug.Log("Ignored self-hit");
      return;
    }

    // פגיעה בשחקן (אם זה לא היורה)
    if (col.gameObject.CompareTag("Player"))
    {
      PlayerHealth player = col.gameObject.GetComponentInParent<PlayerHealth>();
      if (player != null)
      {
        player.TakeDamage(damage);
        Debug.Log("Hit player");
      }
    }

    // פגיעה באויב (אם זה לא היורה)
    if (col.gameObject.CompareTag("Enemy"))
    {
      EnemyScript enemy = col.gameObject.GetComponentInParent<EnemyScript>();
      if (enemy != null)
      {
        enemy.TakeDamage(damage);
        Debug.Log("Hit enemy");
      }
    }

    Destroy(gameObject);
  }
}