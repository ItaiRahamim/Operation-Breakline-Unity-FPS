using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class UnusedAssetsFinder
{
  [MenuItem("Tools/Find Truly Unused Assets")]
  public static void FindTrulyUnusedAssets()
  {
    // 1. קבל את כל הנתיבים של קבצי Assets
    var allAssets = AssetDatabase.GetAllAssetPaths()
        .Where(path => path.StartsWith("Assets/") &&
                       !Directory.Exists(path) &&
                       !path.EndsWith(".cs")) // אל תכלול סקריפטים
        .ToList();

    var usedAssets = new HashSet<string>();

    // 2. סרוק את כל הסצנות הפעילות ב-Build
    var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path);
    foreach (var scene in scenes)
    {
      var deps = AssetDatabase.GetDependencies(scene, true);
      foreach (var dep in deps)
        usedAssets.Add(dep);
    }

    // 3. סרוק את כל ה-Prefabs
    var prefabPaths = AssetDatabase.FindAssets("t:Prefab")
                            .Select(AssetDatabase.GUIDToAssetPath);
    foreach (var prefab in prefabPaths)
    {
      var deps = AssetDatabase.GetDependencies(prefab, true);
      foreach (var dep in deps)
        usedAssets.Add(dep);
    }

    // 4. סרוק Materials, ScriptableObjects, Animations, etc.
    string[] typesToScan = new[] { "Material", "ScriptableObject", "Animation", "AnimatorController", "Shader", "Texture" };
    foreach (var type in typesToScan)
    {
      var guids = AssetDatabase.FindAssets($"t:{type}");
      foreach (var guid in guids)
      {
        var path = AssetDatabase.GUIDToAssetPath(guid);
        var deps = AssetDatabase.GetDependencies(path, true);
        foreach (var dep in deps)
          usedAssets.Add(dep);
      }
    }

    // 5. סרוק Resources (נטען דינמית)
    var resourcesAssets = AssetDatabase.FindAssets("", new[] { "Assets/Resources" })
                                       .Select(AssetDatabase.GUIDToAssetPath);
    foreach (var path in resourcesAssets)
    {
      var deps = AssetDatabase.GetDependencies(path, true);
      foreach (var dep in deps)
        usedAssets.Add(dep);
    }

    // 6. מצא את הקבצים שלא בשימוש
    var unusedAssets = allAssets.Where(asset => !usedAssets.Contains(asset)).ToList();

    Debug.Log($"🔍 נמצאו {unusedAssets.Count} קבצים שלא בשימוש:");

    foreach (var asset in unusedAssets)
    {
      Debug.Log("❌ Unused: " + asset);
    }

    Debug.Log("✅ הסריקה הושלמה. הקבצים הודפסו ל-Console. לא בוצעה מחיקה.");
  }
}