using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
  public int maxHealth = 100;
  private int currentHealth;

  private PlayerHealthUI healthUI;
  private PlayerController controller;

  void Start()
  {
    // האם מדובר ב-Respawn בתוך הסצנה?
    bool isRespawn = PlayerPrefs.GetInt("IsRespawn", 0) == 1;

    if (isRespawn)
    {
      // ✅ Respawn: תמיד לחזור ל-HP של תחילת השלב
      int levelStart = PlayerPrefs.GetInt("LevelStartHP", maxHealth);
      currentHealth = Mathf.Clamp(levelStart, 0, maxHealth);
      PlayerPrefs.SetInt("PlayerHealth", currentHealth);
    }
    else
    {
      // ✅ כניסה "חדשה" לשלב (לא Respawn):
      // אם יש לנו LastLevelEndHP (מסיום השלב הקודם) – נתחיל ממנו.
      // אחרת זה כניסה לשלב ראשון או אין נתון – נתחיל מ-maxHealth.
      int defaultStart = maxHealth;
      int lastEnd = PlayerPrefs.GetInt("LastLevelEndHP", defaultStart);

      // קובע את HP תחילת השלב הנוכחי
      int levelStartHp = Mathf.Clamp(lastEnd, 0, maxHealth);
      PlayerPrefs.SetInt("LevelStartHP", levelStartHp);

      // ה-HP הנוכחי בתחילת השלב שווה ל-HP תחילת השלב
      currentHealth = levelStartHp;
      PlayerPrefs.SetInt("PlayerHealth", currentHealth);
    }

    // עדכון UI והפניות
    healthUI = FindObjectOfType<PlayerHealthUI>();
    controller = GetComponent<PlayerController>();
    healthUI?.UpdateHealth(currentHealth, maxHealth);

    // אחרי שהשחקן נטען – מצב Respawn חוזר ל-0 עד המוות הבא
    PlayerPrefs.SetInt("IsRespawn", 0);
  }

  public void TakeDamage(int damage)
  {
    currentHealth -= damage;
    currentHealth = Mathf.Max(0, currentHealth);

    // נשמור את המצב השוטף (יעזור גם ב-Build)
    PlayerPrefs.SetInt("PlayerHealth", currentHealth);

    healthUI?.UpdateHealth(currentHealth, maxHealth);

    if (currentHealth <= 0)
    {
      controller?.MarkAsDead();
    }
  }

  public void Heal(int amount)
  {
    currentHealth += amount;
    currentHealth = Mathf.Min(currentHealth, maxHealth);

    PlayerPrefs.SetInt("PlayerHealth", currentHealth);
    healthUI?.UpdateHealth(currentHealth, maxHealth);
  }

  public int GetCurrentHealth() => currentHealth;

  public void SetHealth(int hp)
  {
    currentHealth = Mathf.Clamp(hp, 0, maxHealth);
    PlayerPrefs.SetInt("PlayerHealth", currentHealth);
    healthUI?.UpdateHealth(currentHealth, maxHealth);
  }
}