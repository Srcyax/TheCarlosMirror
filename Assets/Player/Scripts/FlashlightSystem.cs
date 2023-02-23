using Mirror;
using UnityEngine;

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

    void Update()
    {
        if (player.IsInventoryActivated())
            return;

        if (player.IsGameOver())
            Destroy(playerLight);

        bool button = Input.GetKeyDown(KeyCode.F);

        CmdFlashlight(button);
    }

    [Command]
    void CmdFlashlight(bool button)
    {
        RpcFlashlight(button);
    }

    [ClientRpc]
    void RpcFlashlight(bool button)
    {
        if (button)
        {
            playerLight.enabled = !playerLight.enabled;
            flashLightAudio.clip = playerLight.enabled ? turnOffClip : turnOnClip;
            flashLightAudio.Play();
        }
    }
}
