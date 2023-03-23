using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightSystem : NetworkBehaviour
{

    [SerializeField] private PlayerController player;


    [Space(10)]

    [Header("Player FlashLight")]
    [SerializeField] private Light playerLight;

    [Header("Audio System")]
    [SerializeField] private AudioClip turnOnClip;
    [SerializeField] private AudioClip turnOffClip;
    [SerializeField] private AudioSource flashLightAudio;

    [SerializeField] private Slider flashLightSlider;

    [HideInInspector] public float flashLightTime = 100;
    private PlayersAlreadyJoined server;

    void Start()
    {
        server = GameObject.FindGameObjectWithTag( "NetworkManager" ).GetComponent<PlayersAlreadyJoined>();
    }

    void Update()
    {
        if ( flashLightTime <= 0 )
            return;

        if ( player.IsInventoryActivated || !isLocalPlayer )
            return;

        if ( player.IsGameOver || !player.IsLocalPlayerAlive )
            return;

        bool button = Input.GetKeyDown(KeyCode.F);

        CmdFlashlight( button );
        FlashLightTime();
    }

    [Command( requiresAuthority = false )]
    void CmdFlashlight( bool button )
    {
        RpcFlashlight( button );
    }

    [ClientRpc]
    void RpcFlashlight( bool button )
    {
        if ( button ) {
            playerLight.enabled = !playerLight.enabled;
            flashLightAudio.clip = playerLight.enabled ? turnOffClip : turnOnClip;
            flashLightAudio.Play();
        }
    }

    void FlashLightTime()
    {
        if ( !server.PlayersAlreadyJoinedInServer() )
            return;

        if ( playerLight && !playerLight.enabled )
            return;

        flashLightTime = flashLightTime > 0 ? flashLightTime - Time.deltaTime : flashLightTime;
        flashLightSlider.value = flashLightTime;

        if ( flashLightTime < 9 )
            playerLight.intensity = flashLightTime / 10;
    }
}