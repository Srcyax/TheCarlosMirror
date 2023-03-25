using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JsonReadWriteSystem : MonoBehaviour {

    public int key = 1337;

    public void PlayerDataSaveToJson(bool tutorial, string playerName) {
        PlayerData data = new PlayerData();
        data.tutorial = tutorial;
        data.playerName = playerName;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText("C:/userdata/playerData.json", EncryptDecrypt( json, key ) );
    }

    public void PlayerDataLoadFromJson(Settings settings) {
        string json = File.ReadAllText("C:/userdata/playerData.json");
        PlayerData data = JsonUtility.FromJson<PlayerData>(EncryptDecrypt(json, key ));
        settings.tutorial = data.tutorial;
        settings.playerName = data.playerName;
    }

    public void SettingsDataSaveToJson(int sensibility, int graphics, int resolution, float menuMusicVolume) {
        SettingsData data = new SettingsData();
        data.sensibility = sensibility;
        data.graphics = graphics;
        data.resolution = resolution;
        data.menuMusicVolume = menuMusicVolume;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText("C:/userdata/settingsData.json", EncryptDecrypt( json, key ) );
    }

    public void SettingsDataLoadFromJson(Slider sensibility, TMP_Dropdown graphics, TMP_Dropdown resolution, Slider menuMusicVolume) {
        string json = File.ReadAllText("C:/userdata/settingsData.json");
        SettingsData data = JsonUtility.FromJson<SettingsData>(EncryptDecrypt(json, key ));
        sensibility.value = data.sensibility;
        graphics.value = data.graphics;
        resolution.value = data.resolution;
        menuMusicVolume.value = data.menuMusicVolume;
    }

    public void MatchMakingSaveToJson(int maxConnections, int botConnections, int gameMode) {
        MatchMakingData data = new MatchMakingData();
        data.maxConnection = maxConnections;
        data.botConnection = botConnections;
        data.gameMode = gameMode;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText("C:/userdata/matchMakingData.json", EncryptDecrypt( json, key ) );
    }

    public void MatchMakingLoadFromJson(TMP_Dropdown maxConnections, TMP_Dropdown botConnections, TMP_Dropdown gameMode) {
        string json = File.ReadAllText("C:/userdata/matchMakingData.json");
        MatchMakingData data = JsonUtility.FromJson<MatchMakingData>(EncryptDecrypt(json, key ));
        maxConnections.value = data.maxConnection;
        botConnections.value = data.botConnection;
        gameMode.value = data.gameMode;
    }

    public void PlayerCoinSaveToJson(int coins)
    {
        if ( File.Exists( "C:/userdata/coins.json" ) ) {
            string json = File.ReadAllText("C:/userdata/coins.json");
            PlayerCoin data = JsonUtility.FromJson<PlayerCoin>(EncryptDecrypt(json, key ));

            data.coin += coins;
            string json1 = JsonUtility.ToJson(data, true);
            File.WriteAllText( "C:/userdata/coins.json", EncryptDecrypt( json1, key ) );
        }
        else {
            PlayerCoin data = new PlayerCoin();
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText( "C:/userdata/coins.json", EncryptDecrypt( json, key ) );
        }
    }

    public int PlayerCoinLoadFromJson()
    {
        string json = File.ReadAllText("C:/userdata/coins.json");
        PlayerCoin data = JsonUtility.FromJson<PlayerCoin>(EncryptDecrypt(json, key ));
        return data.coin;
    }

    public void PlayerItemsSaveToJson(string[] items)
    {
        PlayerItems data = new PlayerItems();

        for (int i = 0; i < items.Length; i++ ) {
            data.items = items;
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText( "C:/userdata/playerItems.json", EncryptDecrypt( json, key ) );
    }

    public void PlayerItemsLoadFromJson()
    {
        string json = File.ReadAllText("C:/userdata/playerItems.json");
        PlayerItems data = JsonUtility.FromJson<PlayerItems>(EncryptDecrypt(json, key ));

        for ( int i = 0; i < data.items.Length; i++ ) {
            print( data.items[ i ] );
        }
    }

    public string EncryptDecrypt( string data, int key )
    {
        StringBuilder input = new StringBuilder(data);
        StringBuilder output = new StringBuilder(data.Length);

        char character;

        for ( int i = 0; i < data.Length; ++i ) {
            character = input[ i ];
            character = (char)( character ^ key );
            output.Append( character );
        }

        return output.ToString();
    }
}