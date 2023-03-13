using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDisconnect : NetworkBehaviour
{
    [SerializeField] private Settings settings;
    public void Disconnect()
    {
        JsonReadWriteSystem json = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<JsonReadWriteSystem>();
        json.PlayerDataSaveToJson(false, settings.playerName);
        json.SettingsDataSaveToJson((int)settings.sensitivy, settings.graphics, settings.resolution, settings.menuMusicVolume);

        if (isServer)
        {
            if (NetworkServer.connections.Count <= 1)
            {
                NetworkServer.Shutdown();
                SceneManager.LoadScene("Game");
            }
        }
        else
        {
            NetworkClient.Shutdown();
            SceneManager.LoadScene("Game");
        }
    }
}