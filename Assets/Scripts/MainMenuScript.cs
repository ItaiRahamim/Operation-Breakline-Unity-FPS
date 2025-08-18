using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
  public string levelToLoad = "Level 1";
  public static bool GameStartedFromMenu = false;

  public void StartGame()
  {
    GameStartedFromMenu = true;

    // אפס הכל
    PlayerPrefs.DeleteAll();

    // התחלה נקייה
    PlayerPrefs.SetInt("PlayerHealth", 100);
    PlayerPrefs.SetInt("LastLevelEndHP", 100);
    PlayerPrefs.SetInt("LevelStartHP", 100);

    PlayerPrefs.SetInt("KillCount", 0);
    PlayerPrefs.SetInt("LastLevelKillCount", 0);
    PlayerPrefs.SetInt("LevelStartKillCount", 0);
    PlayerPrefs.SetInt("IsRespawn", 0);

    // ✅ אם יש KillCounter חי – אפס אותו בזיכרון מיד
    if (KillCounter.Instance != null)
      KillCounter.Instance.ResetAllToNewGame();

    SceneManager.LoadScene(levelToLoad);
  }

  public void QuitGame()
  {
    Debug.Log("Quit Game");
    Application.Quit();
  }
}