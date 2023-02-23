using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;

    [SerializeField] TMP_Dropdown maxClient;
    [SerializeField] NetworkManager transport;
    [SerializeField] TextMeshProUGUI ipAdress;
    [Space(10)]
    [SerializeField] Settings settings;
    [Space(10)]
    [SerializeField] private GameObject loadScreen;

    void Awake()
    {
        Cursor.visible = true;
    }

    private void Start()
    {
        var holdSense = settings.sensitivy;
        var holdGraphics = settings.graphics;

        File.WriteAllText("C:/userdata/sense.txt", holdSense.ToString());
        File.WriteAllText("C:/userdata/graphics.txt", holdGraphics.ToString());

        HostButton?.onClick.AddListener(() =>
        {
            if ((maxClient.value + 1) > 1)
            {
                NetworkManager.singleton.StartHost();
                transport.maxConnections = maxClient.value;
                Instantiate(loadScreen);
            }
        });
        ClientButton?.onClick.AddListener(() =>
        {
            NetworkManager.singleton.StartClient();
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