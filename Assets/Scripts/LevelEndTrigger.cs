using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      // שמירת KillCount כסיום שלב
      KillCounter.Instance?.SaveKillCountAsLevelComplete();

      // ✅ נשמור את ה-HP בעת סיום השלב, כדי שהשלב הבא יתחיל ממנו
      var ph = other.GetComponent<PlayerHealth>();
      if (ph != null)
      {
        int endHp = ph.GetCurrentHealth();
        PlayerPrefs.SetInt("LastLevelEndHP", endHp);
        PlayerPrefs.SetInt("PlayerHealth", endHp); // סנכרון שוטף
      }

      // זה מעבר לשלב חדש, לא Respawn
      PlayerPrefs.SetInt("IsRespawn", 0);

      int currentIndex = SceneManager.GetActiveScene().buildIndex;
      SceneManager.LoadScene(currentIndex + 1);
    }
  }
}