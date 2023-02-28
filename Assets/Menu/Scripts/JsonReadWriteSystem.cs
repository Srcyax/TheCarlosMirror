using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JsonReadWriteSystem : MonoBehaviour
{
    public void PlayerDataSaveToJson(bool tutorial, string playerName)
    {
        PlayerData data = new PlayerData();
        data.tutorial = tutorial;
        data.playerName = playerName;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText("C:/userdata/playerData.json", json);
    }

    public void PlayerDataLoadFromJson(Settings settings)
    {
        string json = File.ReadAllText("C:/userdata/playerData.json");
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        settings.tutorial = data.tutorial;
        settings.playerName = data.playerName;
    }

    public void SettingsDataSaveToJson(int sensibility, int graphics, int resolution, float menuMusicVolume)
    {
        SettingsData data = new SettingsData();
        data.sensibility = sensibility;
        data.graphics = graphics;
        data.resolution = resolution;
        data.menuMusicVolume = menuMusicVolume;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText("C:/userdata/settingsData.json", json);
    }

    public void SettingsDataLoadFromJson(Slider sensibility, TMP_Dropdown graphics, TMP_Dropdown resolution, Slider menuMusicVolume)
    {
        string json = File.ReadAllText("C:/userdata/settingsData.json");
        SettingsData data = JsonUtility.FromJson<SettingsData>(json);
        sensibility.value = data.sensibility;
        graphics.value = data.graphics;
        resolution.value = data.resolution;
        menuMusicVolume.value = data.menuMusicVolume;
    }
}