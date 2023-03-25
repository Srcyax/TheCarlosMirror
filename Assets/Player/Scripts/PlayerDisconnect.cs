using Mirror;
using UnityEngine;

public class PlayerDisconnect : NetworkBehaviour {
    [SerializeField] private Settings settings;
    public void Disconnect() {
        JsonReadWriteSystem json = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<JsonReadWriteSystem>();
        json.PlayerDataSaveToJson(false, settings.playerName);
        json.SettingsDataSaveToJson(( int )settings.sensitivy, settings.graphics, settings.resolution, settings.menuMusicVolume);
        json.PlayerCoinSaveToJson(0);

        Application.Quit();
    }
}