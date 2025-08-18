using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MissionCompleteManager : MonoBehaviour
{
  [Header("UI & Flow")]
  public GameObject VictoryCanvas;      // קנבס עם הטקסט/כפתורים
  public string mainMenuSceneName = "MainMenu"; // שם סצנת המיין-תפריט

  [Header("Timing (seconds)")]
  public float delayBeforeMessage = 2f;    // כמה זמן לחכות מרגע ההריגה האחרונה עד להודעה
  public float delayBeforeMainMenu = 3f;   // כמה זמן אחרי ההודעה לעבור ל־Main Menu

  private int enemiesInLevel;
  private bool victoryTriggered = false;

  void Start()
  {
    if (VictoryCanvas != null)
      VictoryCanvas.SetActive(false); // ההודעה מוסתרת בתחילה

    // נבדוק רק ב־Level 3 (כפי שביקשת)
    if (SceneManager.GetActiveScene().name == "Level 3")
    {
      enemiesInLevel = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
  }

  void Update()
  {
    if (victoryTriggered) return;

    if (SceneManager.GetActiveScene().name == "Level 3")
    {
      int currentKills = KillCounter.Instance.GetKillCount();
      int startKills = PlayerPrefs.GetInt("LevelStartKillCount", 0);
      int killsThisLevel = currentKills - startKills;

      if (killsThisLevel >= enemiesInLevel)
      {
        victoryTriggered = true;
        StartCoroutine(HandleVictorySequence());
      }
    }
  }

  private IEnumerator HandleVictorySequence()
  {
    // מחכים 2 שניות "רגילות" לפני שמציגים את ההודעה (המשחק עדיין רץ)
    if (delayBeforeMessage > 0f)
      yield return new WaitForSeconds(delayBeforeMessage);

    ShowMissionComplete(); // מציג קנבס + מקפיא זמן

    // עכשיו הזמן קפוא, לכן נשתמש ב-Realtime כדי ש-3 השניות יעברו באמת
    if (delayBeforeMainMenu > 0f)
      yield return new WaitForSecondsRealtime(delayBeforeMainMenu);

    // לפני טעינת תפריט, נחזיר את קצב הזמן כדי שלא נישאר "תקועים" בסצנה הבאה
    Time.timeScale = 1f;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    if (!string.IsNullOrEmpty(mainMenuSceneName))
      SceneManager.LoadScene(mainMenuSceneName);
  }

  public void ShowMissionComplete()
  {
    if (VictoryCanvas != null)
      VictoryCanvas.SetActive(true);

    // הצג את העכבר
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    // עצירת המשחק אחרי שההודעה הופיעה
    Time.timeScale = 0f;
  }
}