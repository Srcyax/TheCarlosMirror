using UnityEngine;

public class CameraBob : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerController player;

    [Header("Foot step sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;
    private int currentSound;

    private float walkingBobbingSpeed;
    private float bobbingAmount;
    private float defaultPosY;
    private float timer;

    private void Start()
    {
        defaultPosY = transform.localPosition.y;
    }

    void Update()
    {
        if (player.IsInventoryActivated || player.IsGameOver || !player.IsLocalPlayerAlive || player.IsGamePaused)
            return;

        walkingBobbingSpeed = Input.GetKey(KeyCode.LeftShift) ? 15 : 10;
        bobbingAmount = Input.GetKey(KeyCode.LeftShift) ? 0.1f : 0.05f;

        timer += Mathf.Abs(controller.velocity.magnitude) > 0 ? Time.deltaTime * walkingBobbingSpeed : 0;
        transform.localPosition = Mathf.Abs(controller.velocity.magnitude) > 0 ? new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, transform.localPosition.z) : new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed), transform.localPosition.z); ;
        
        ChooseFootSound();
    }

    void ChooseFootSound()
    {
        if (Mathf.Abs(controller.velocity.magnitude) > 0)
        {
            if (audioSource.isPlaying)
                return;

            currentSound = currentSound < 2 ? currentSound + 1 : 0;
            audioSource.PlayOneShot(clips[currentSound]);
        }
    }
}
