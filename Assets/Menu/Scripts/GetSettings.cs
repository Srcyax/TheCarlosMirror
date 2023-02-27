using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetSettings : MonoBehaviour
{
    [SerializeField] Slider sense;
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
        playerName.text = Environment.UserName;

        wrongVersion.SetActive(Application.version != GetGameVersion());
        scene.LoadScene();
        if (Directory.Exists("C:/userdata"))
        {
            if(File.Exists(Application.dataPath + "C:/userdata.json"))
                json.LoadFromJson(sense, graphics, settings, resolution);
            else
                json.SaveToJson(3, 0, true, 0);
        }
        else
        {
            DirectoryInfo di = Directory.CreateDirectory("C:/userdata");
            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            json.SaveToJson(3, 0, true, 0);
        }     
    }

    void Update()
    {
        settings.sensitivy = sense.value;
        settings.graphics = graphics.value;
        settings.resolution = resolution.value;
        settings.playerName = playerName.text;
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
}