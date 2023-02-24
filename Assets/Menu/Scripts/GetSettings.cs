using System;
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
    [SerializeField] TextMeshProUGUI playerName;

    [SerializeField] Settings settings;

    [Space(10)]
    [SerializeField] GameObject wrongVersion;

    private void Start()
    {
        wrongVersion.SetActive(Application.version != GetGameVersion());

        if (Directory.Exists("C:/userdata"))
        {
            if (File.Exists("C:/userdata/sense.txt"))
            {
                foreach (string line in File.ReadLines("C:/userdata/sense.txt"))
                {
                    sense.value = Int32.Parse(line);
                }
            }
            else
            {
                File.WriteAllText("C:/userdata/sense.txt", "3");
            }
            if (File.Exists("C:/userdata/graphics.txt"))
            {
                foreach (string line in File.ReadLines("C:/userdata/graphics.txt"))
                {
                    graphics.value = Int32.Parse(line);
                }
            }
            else
            {
                File.WriteAllText("C:/userdata/graphics.txt", "0");
            }
            if (File.Exists("C:/userdata/tutorial.txt"))
            {
                foreach (string line in File.ReadLines("C:/userdata/tutorial.txt"))
                {
                    settings.tutorial = Boolean.Parse(line);
                }
            }
            else
            {
                File.WriteAllText("C:/userdata/tutorial.txt", "true");
            }
            if (File.Exists("C:/userdata/resolution.txt")) 
            { 
                foreach (string line in File.ReadLines("C:/userdata/resolution.txt"))
                {
                    resolution.value = Int32.Parse(line);
                } 
            }
            else
            {
                            File.WriteAllText("C:/userdata/resolution.txt", "0");
            }
        }
        else
        {
            DirectoryInfo di = Directory.CreateDirectory("C:/userdata");
            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            File.WriteAllText("C:/userdata/sense.txt", "3");
            File.WriteAllText("C:/userdata/graphics.txt", "0");
            File.WriteAllText("C:/userdata/resolution.txt", "0");
            File.WriteAllText("C:/userdata/tutorial.txt", "true");
        }
    }

    void Update()
    {
        settings.sensitivy = sense.value;
        settings.graphics = graphics.value;
        settings.resolution = resolution.value;
        settings.playerName = playerName.text;

        Resolution();
    }


    void Resolution()
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