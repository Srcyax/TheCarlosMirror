using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JsonReadWriteSystem : MonoBehaviour
{
    public void SaveToJson(int sensibility, int graphics, bool tutorial, int resolution)
    {
        PlayerData data = new PlayerData();
        data.sensibility = sensibility;
        data.resolution = resolution;
        data.tutorial = tutorial;
        data.graphics = graphics;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText("C:/userdata/data.json", json);
    }

    public void LoadFromJson(Slider sensibility, TMP_Dropdown graphics, Settings settings, TMP_Dropdown resolution)
    {
        string json = File.ReadAllText("C:/userdata/data.json");
        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        sensibility.value = data.sensibility;
        graphics.value = data.graphics;
        resolution.value = data.resolution;
        settings.tutorial = data.tutorial;
    }
}
