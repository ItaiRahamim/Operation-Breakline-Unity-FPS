using UnityEngine;

public class MedKit : MonoBehaviour
{
  public int healAmount = 30;

  void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
      if (playerHealth != null)
      {
        playerHealth.Heal(healAmount);
        Destroy(gameObject);
      }
    }
  }
}