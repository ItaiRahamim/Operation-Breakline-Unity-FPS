using UnityEngine;

public class BreakOnGroundHit : MonoBehaviour
{
  public GameObject fracturedVersion;
  public float breakVelocityThreshold = 3f;
  public string groundTag = "Ground";

  private bool hasBroken = false;

  void OnCollisionEnter(Collision collision)
  {
    if (hasBroken) return;

    if (collision.gameObject.CompareTag(groundTag))
    {
      if (collision.relativeVelocity.magnitude > breakVelocityThreshold)
      {
        GameObject fractured = Instantiate(fracturedVersion, transform.position, transform.rotation);

        foreach (Rigidbody rb in fractured.GetComponentsInChildren<Rigidbody>())
        {
          rb.isKinematic = false;
        }

        hasBroken = true;
        Destroy(gameObject);
      }
    }
  }
}