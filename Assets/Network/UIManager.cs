using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;

    [SerializeField] private TMP_Dropdown maxClient;
    [SerializeField] private NetworkManager transport;
    [SerializeField] private TextMeshProUGUI ipAdress;
    [Space(10)]
    [SerializeField] private Settings settings;
    [SerializeField] private JsonReadWriteSystem json;
    [Space(10)]
    [SerializeField] private GameObject loadScreen;
    [SerializeField] private SpawnScene scene;

    void Awake()
    {
        Cursor.visible = true;
    }

    private void Start()
    {
        json.SaveToJson((int)settings.sensitivy, settings.graphics, settings.tutorial, settings.resolution);

        HostButton?.onClick.AddListener(() =>
        {
            if ((maxClient.value + 1) > 1)
            {
                NetworkManager.singleton.StartHost();
                transport.maxConnections = maxClient.value;
                scene.LoadScene();
                Instantiate(loadScreen);
            }
        });
        ClientButton?.onClick.AddListener(() =>
        {
            NetworkManager.singleton.StartClient();
            scene.LoadScene();
            Instantiate(loadScreen);
        });
    }
    private void Update()
    {
#if !UNITY_EDITOR
        HostButton.interactable = settings.playerName.Length > 1 && ipAdress.text.Length > 1;
        ClientButton.interactable = settings.playerName.Length > 1 && ipAdress.text.Length > 1;
#endif
    }
}