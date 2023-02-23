using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Net;

public class GetSettings : MonoBehaviour
{
    // UI elements
    [SerializeField] Slider slider;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] TextMeshProUGUI playerName;

    // Reference to the settings scriptable object
    [SerializeField] Settings settings;

    private void Start()
    {
        // Check if the version file exists, and if so, delete it
        if (File.Exists(@"version.txt"))
        {
            File.Delete(@"version.txt");
        }

        // Download the version file from the server
        WebClient webClient = new WebClient();
        webClient.DownloadFile("https://nexuscheat.store/version.txt", @"version.txt");

        // Check if the current version matches the one in the file
        foreach (string line in File.ReadLines(@"version.txt"))
        {
            if (Application.version != line)
            {
                // If not, create a file with instructions to download the new version, and quit the application
                File.WriteAllText(@"readme.txt", "download new version here: https://srcyax.itch.io/carlos");
                Application.Quit();
            }
            else
            {
                // If the versions match, delete the readme file if it exists
                if (File.Exists(@"readme.txt"))
                {
                    File.Delete(@"readme.txt");
                }
            }
        }

        if (Directory.Exists(@"userdata"))
        {
            foreach (string line in File.ReadLines(@"userdata/sense.txt"))
            {
                slider.value = Int32.Parse(line);
            }
            foreach (string line in File.ReadLines(@"userdata/graphics.txt"))
            {
                dropdown.value = Int32.Parse(line);
            }
            foreach (string line in File.ReadLines(@"userdata/tutorial.txt"))
            {
                settings.tutorial = Boolean.Parse(line);
            }
        }
        else
        {
            DirectoryInfo di = Directory.CreateDirectory(@"userdata");
            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            File.WriteAllText(@"userdata/sense.txt", "3");
            File.WriteAllText(@"userdata/graphics.txt", "0");
            File.WriteAllText(@"userdata/tutorial.txt", "true");
        }
    }

    void Update()
    {
        settings.sensitivy = slider.value;
        settings.graphics = dropdown.value;
        settings.playerName = playerName.text;
    }
}