using UnityEngine;

public class FlashlightSystem : MonoBehaviour
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
        if (player.inventoryActive)
            return;

        if (player.playerPoints.gameWon.activeSelf)
            Destroy(playerLight);

        if (Input.GetKeyDown(KeyCode.F))
        {
            playerLight.enabled = !playerLight.enabled;
            flashLightAudio.clip = playerLight.enabled ? turnOffClip : turnOnClip;
            flashLightAudio.Play();
        }
    }
}
