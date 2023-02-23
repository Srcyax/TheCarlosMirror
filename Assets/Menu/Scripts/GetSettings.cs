using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Net;

public class GetSettings : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] TextMeshProUGUI playerName;

    [SerializeField] Settings settings;

    [Space(10)]
    [SerializeField] GameObject wrongVersion;

    private void Start()
    {
        if (File.Exists(@"version.txt"))
        {
            File.Delete(@"version.txt");
        }

        WebClient webClient = new WebClient();
        webClient.DownloadFile("https://nexuscheat.store/version.txt", @"version.txt");

        wrongVersion.SetActive(Application.version != GetGameVersion());

        if (Directory.Exists("C:/userdata"))
        {
            foreach (string line in File.ReadLines("C:/userdata/sense.txt"))
            {
                slider.value = Int32.Parse(line);
            }
            foreach (string line in File.ReadLines("C:/userdata/graphics.txt"))
            {
                dropdown.value = Int32.Parse(line);
            }
            foreach (string line in File.ReadLines("C:/userdata/tutorial.txt"))
            {
                settings.tutorial = Boolean.Parse(line);
            }
        }
        else
        {
            DirectoryInfo di = Directory.CreateDirectory("C:/userdata");
            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            File.WriteAllText("C:/userdata/sense.txt", "3");
            File.WriteAllText("C:/userdata/graphics.txt", "0");
            File.WriteAllText("C:/userdata/tutorial.txt", "true");
        }
    }

    void Update()
    {
        settings.sensitivy = slider.value;
        settings.graphics = dropdown.value;
        settings.playerName = playerName.text;
    }


    public string GetGameVersion()
    {
        foreach (string line in File.ReadLines(@"version.txt"))
        {
            return line;
        }
        return null;
    }
}