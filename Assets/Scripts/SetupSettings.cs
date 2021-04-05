using System.IO;
using UnityEngine;

[System.Serializable]
public class Settings
{
    public string phoneURL;
    public string serverURL;

    /// <summary>
    /// Get settings from .json file or generate it with default params.
    /// </summary>
    /// <param name="phoneURL">Standart index.html url for first generate .json</param>
    /// <param name="serverURL">Standart "pong" hub url for first generate .json</param>
    public Settings(string phoneURL, string serverURL)
    {
        string path = Path.Combine(Application.dataPath, "settings.json");

#if UNITY_EDITOR
        path = Path.Combine(Directory.GetCurrentDirectory(), "settings.json");
#endif

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Settings buffSettings = JsonUtility.FromJson<Settings>(json);
            this.serverURL = buffSettings.serverURL;
            this.phoneURL = buffSettings.phoneURL;
        }
        else
        {
            this.phoneURL = phoneURL;
            this.serverURL = serverURL;
            File.WriteAllText(path, JsonUtility.ToJson(this));
        }
    }
}