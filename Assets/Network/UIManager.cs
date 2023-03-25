using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;
    [SerializeField] private Slider musicVolumeSlider;

    [SerializeField] private TMP_Dropdown maxClient;
    [SerializeField] private TMP_Dropdown maxBots;
    [SerializeField] private TMP_Dropdown gameMode;
    [SerializeField] private GameObject botPrefab;
    [SerializeField] private Transform[] botPos;
    [SerializeField] private NetworkManager transport;
    [SerializeField] private TextMeshProUGUI ipAdress;
    [Space(10)]
    [SerializeField] private Settings settings;
    [SerializeField] private JsonReadWriteSystem json;
    [Space(10)]
    [SerializeField] private GameObject loadScreen;

    private void Start()
    {
        ConfigData();

        HostButton?.onClick.AddListener( () => {
            if ( ( maxClient.value + 1 ) > 1 ) {
                NetworkManager.singleton.StartHost();
                transport.maxConnections = maxClient.value + maxBots.value;
                Instantiate( loadScreen );
                json.MatchMakingSaveToJson( maxClient.value, maxBots.value, gameMode.value );
                for ( int i = 0; i < maxBots.value; i++ ) {
                    GameObject bots = Instantiate(botPrefab, botPos[i]);
                    NetworkServer.Spawn( bots );
                }
            }
        } );

        ClientButton?.onClick.AddListener( () => {
            NetworkManager.singleton.StartClient();
            Instantiate( loadScreen );
        } );
    }
    private void Update()
    {
#if !UNITY_EDITOR
        HostButton.interactable = settings.playerName.Length > 1 && ipAdress.text.Length > 1;
        ClientButton.interactable = settings.playerName.Length > 1 && ipAdress.text.Length > 1;
#endif
    }

    void ConfigData()
    {
        json.PlayerDataSaveToJson( settings.tutorial, settings.playerName );
        json.PlayerCoinSaveToJson(0);
        json.SettingsDataSaveToJson( (int)settings.sensitivy, settings.graphics, settings.resolution, settings.menuMusicVolume );
        json.MatchMakingLoadFromJson( maxClient, maxBots, gameMode );
    }
}