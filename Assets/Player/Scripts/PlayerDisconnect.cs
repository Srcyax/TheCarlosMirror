using UnityEngine;

public class PlayerDisconnect : MonoBehaviour
{
    [SerializeField] private Settings settings;
    public void Disconnect()
    {
        JsonReadWriteSystem json = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<JsonReadWriteSystem>();
        json.PlayerDataSaveToJson(false, settings.playerName);
        json.SettingsDataSaveToJson((int)settings.sensitivy, settings.graphics, settings.resolution, settings.menuMusicVolume);
        Application.Quit();
    }
}