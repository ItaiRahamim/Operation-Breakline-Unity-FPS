using UnityEngine;

public class BreakOnHit : MonoBehaviour
{
  public GameObject fracturedVersion;
  public float explosionForce = 300f;
  public float explosionRadius = 2f;

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.CompareTag("Bullet"))
    {
      GameObject fractured = Instantiate(fracturedVersion, transform.position, transform.rotation);

      foreach (Rigidbody rb in fractured.GetComponentsInChildren<Rigidbody>())
      {
        rb.isKinematic = false;
        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
      }

      Destroy(gameObject);
    }
  }
}