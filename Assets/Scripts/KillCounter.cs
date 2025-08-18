using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class KillCounter : MonoBehaviour
{
  public static KillCounter Instance;

  [Header("UI")]
  public TextMeshProUGUI killText;

  private int killCount;

  void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
      SceneManager.sceneLoaded += OnSceneLoaded; // ✅ נאתחל בכל טעינת סצנה
    }
    else
    {
      Destroy(gameObject);
      return;
    }
  }

  void Start()
  {
    // רץ רק בפעם הראשונה; האתחול האמיתי מתבצע גם ב-OnSceneLoaded
    InitializeForScene();
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    // ריביינד ל-UI אם צריך (כי הסינגלטון עובר בין סצנות)
    if (killText == null)
    {
      // נסה למצוא TextMeshProUGUI עם שם/טאג מתאים – ניתן להתאים לפי הפרויקט שלך
      // אפשר גם לגרור ידנית ב-Inspector בכל סצנה
      killText = FindObjectOfType<TextMeshProUGUI>();
    }

    InitializeForScene();
  }

  private void InitializeForScene()
  {
    bool isRespawn = PlayerPrefs.GetInt("IsRespawn", 0) == 1;

    if (isRespawn)
    {
      // חזרה ל-start של השלב (במוות)
      killCount = PlayerPrefs.GetInt("LevelStartKillCount", 0);
    }
    else
    {
      // התחלה "טרייה" של הסצנה: השלב לוקח את הערך שסיים בו השלב הקודם
      killCount = PlayerPrefs.GetInt("LastLevelKillCount", 0);
      PlayerPrefs.SetInt("LevelStartKillCount", killCount);
    }

    UpdateKillText();
    PlayerPrefs.SetInt("IsRespawn", 0);
  }

  public void AddKill()
  {
    killCount++;
    UpdateKillText();
  }

  public void SaveKillCountAsLevelComplete()
  {
    PlayerPrefs.SetInt("LastLevelKillCount", killCount);
  }

  public void ResetToLevelStart()
  {
    killCount = PlayerPrefs.GetInt("LevelStartKillCount", 0);
    UpdateKillText();
  }

  // ✅ איפוס מלא למשחק חדש
  public void ResetAllToNewGame()
  {
    killCount = 0;
    PlayerPrefs.SetInt("KillCount", 0);
    PlayerPrefs.SetInt("LastLevelKillCount", 0);
    PlayerPrefs.SetInt("LevelStartKillCount", 0);
    UpdateKillText();
  }

  private void UpdateKillText()
  {
    if (killText != null)
      killText.text = killCount.ToString();
  }

  public int GetKillCount()
  {
    return killCount;
  }

  void OnDestroy()
  {
    if (Instance == this)
      SceneManager.sceneLoaded -= OnSceneLoaded;
  }
}