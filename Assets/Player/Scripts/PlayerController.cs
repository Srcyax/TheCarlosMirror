using Mirror;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[RequireComponent(typeof(CharacterController))]

[Serializable]
public class PlayerController : NetworkBehaviour
{
    [Header("Settings")]
    [Tooltip("Get current settings player")]
    [SerializeField] private Settings settings;
    [SerializeField] private Slider sensitivity;
    [SerializeField] private TMP_Dropdown graphics;
    [SerializeField] public TMP_Dropdown resolution;

    [Space(10)]

    [Header("Pause menus")]
    [SerializeField] private GameObject pauseMenu;

    [Space(10)]
    [Header("Player components")]
    [SerializeField] private PlayerName playerNameSystem;
    [SerializeField] public PlayerPoints playerPoints;
    [Space(10)]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] public GameObject playerHand;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private AudioSource breath;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private TextMeshProUGUI playerIsDead;
    [SerializeField] private TextMeshProUGUI waitForPlayers;

    [Space(10)]
    [SerializeField] private GameObject playerTutorial;
    [SerializeField] private TextMeshProUGUI playerTutorialText;

    [Space(10)]

    [SerializeField] private GameObject jumpScare;
    [SerializeField] private GameObject wiiJumpScare;

    [Space(10)]
    [Header("Player flashlight")]
    [SerializeField] private Light flashLight;
    [SerializeField] private Sway swayFlashlight;

    [Space(10)]
    [Header("Player screen Game state")]
    [SerializeField] private GameObject gameOver;
    [SerializeField] GameObject bonePrefab;

    [Header("Stamine")]
    [SerializeField] private Slider stamineSlider;
    [SerializeField] private float stamine = 100;

    [HideInInspector] public bool energyEffect = false;
    [HideInInspector] bool energyEffectCheck = false;
    [HideInInspector] public bool eyeEffect = false;
    [HideInInspector][SyncVar] public bool whatIsItJumpscare = false;
    [SyncVar] public bool isDead = false;

    #region Others
    private CharacterController characterController;
    private PlayerInventory inventory => GetComponent<PlayerInventory>();
    private Vector3 moveDirection = Vector3.zero;
    private GameObject[] players;

    public float walkingSpeed = .1f;
    public float runningSpeed = 10.7f;

    private float gravity = 20.0f;
    private float lookXLimit = 80.0f;
    private float rotationX = 0;
    private PostProcessVolume postProcess;

    [HideInInspector] public ChromaticAberration chromaticAberration;
    [HideInInspector] public ColorGrading colorGrading;
    [HideInInspector] public LensDistortion lensDistortion;
    [HideInInspector] public Bloom bloom;
    #endregion

    void Start()
    {
        LocalPlayerStart();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        PlayerControler();
        Animations();
        Stamine();
        CheckEffects();
        Graphics();

        // Command server
        CmdPlayerIsDead(isDead);
    }

    void PlayerControler()
    {
        if (IsGameOver || IsInventoryActivated || IsGamePaused)
            return;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && stamine > 1;

        float curSpeedX = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical");
        float curSpeedY = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal");
        moveDirection = ((forward * curSpeedX) + (right * curSpeedY));

        moveDirection = Vector3.ClampMagnitude(moveDirection, 10.7f);

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity;
        }

        characterController.Move(moveDirection * Time.deltaTime);
        rotationX += -Input.GetAxis("Mouse Y") * sensitivity.value;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensitivity.value, 0);

        if (transform.localPosition.y < 0)
            transform.localPosition = new Vector3(69.2f, 15.6f, 46f);
    }

    void Animations()
    {
        bool walkForward = Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.LeftShift);
        bool walkBackward = Input.GetKey(KeyCode.S);
        bool walkStrafeRight = Input.GetKey(KeyCode.D);
        bool walkStrafeLeft = Input.GetKey(KeyCode.A);
        bool run = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift);
        bool idle = !walkForward && !walkBackward && !walkStrafeRight && !walkStrafeLeft && !run;

        animator.SetBool("walkForward", walkForward);
        animator.SetBool("walkBackward", walkBackward);
        animator.SetBool("walkStrafeRight", walkStrafeRight);
        animator.SetBool("walkStrafeLeft", walkStrafeLeft);
        animator.SetBool("run", run);
        animator.SetBool("idle", idle);
        animator.SetBool("hello", Input.GetKey(KeyCode.F1));
        animator.SetBool("dwarf", Input.GetKey(KeyCode.F2));
        animator.SetBool("hello2", Input.GetKey(KeyCode.F3));
        animator.SetBool("GangnamStyle", Input.GetKey(KeyCode.F4));
        animator.SetBool("salute", Input.GetKey(KeyCode.F5));

        cameraAnimator.SetBool("idle", idle);
        cameraAnimator.SetBool("walk", walkForward || walkBackward || walkStrafeRight || walkStrafeLeft);
        cameraAnimator.SetBool("run", run);
    }

    void Stamine()
    {
        if (!IsLocalPlayerAlive)
        {
            stamine = 100;
            return;
        }


        bool isRunning = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W);

        if (isRunning && !energyEffectCheck)
        {
            stamine -= Time.deltaTime * 5;
        }
        else
        {
            if (stamine < 100)
                stamine += Time.deltaTime * 7;
        }

        breath.volume = isRunning && stamine < 60 ? breath.volume + Time.deltaTime * 0.3f : breath.volume - Time.deltaTime * 0.1f;

        stamineSlider.value = stamine;
        stamineSlider.gameObject.SetActive(stamineSlider.value < 100);
    }

    void Graphics()
    {
        QualitySettings.SetQualityLevel(graphics.value);
        postProcess.weight = graphics.value < 2 ? 1f : .25f;
        colorGrading.temperature.value = IsLocalPlayerAlive ? 11f : -100f;
        lensDistortion.intensity.value = IsLocalPlayerAlive ? 16 : -48;
    }

    private void LocalPlayerStart()
    {
#if !UNITY_EDITOR
        GetSettings version = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<GetSettings>();
        if (version)
        {
            if (Application.version != version.GetGameVersion())
                SceneManager.LoadScene("Game");
        }
#endif

        sensitivity.value = settings.sensitivy;
        resolution.value = settings.resolution;
        graphics.value = settings.graphics;

        GameObject.FindGameObjectWithTag("CanvasMenu")?.SetActive(false);
        playerCanvas.SetActive(isLocalPlayer);
        playerCamera.GetComponent<AudioListener>().enabled = isLocalPlayer;
        playerCamera.enabled = isLocalPlayer;
        playerNameSystem.playerName.enabled = !isLocalPlayer;
        playerModel.SetActive(!isLocalPlayer);
        inventory.inventoryObject.SetActive(isLocalPlayer);
        swayFlashlight.enabled = isLocalPlayer;
        characterController = GetComponent<CharacterController>();
        postProcess = playerCamera.GetComponent<PostProcessVolume>();
        postProcess.enabled = isLocalPlayer;
        chromaticAberration = postProcess.profile.GetSetting<ChromaticAberration>();
        bloom = postProcess.profile.GetSetting<Bloom>();
        colorGrading = postProcess.profile.GetSetting<ColorGrading>();
        lensDistortion = postProcess.profile.GetSetting<LensDistortion>();
        waitForPlayers.enabled = isServer;

        if (settings.tutorial)
        {
            StartCoroutine(RemoveTutorial());
        }
    }

    void CheckEffects()
    {
        if (!IsLocalPlayerAlive)
        {
            jumpScare.SetActive(true);
            StartCoroutine(JumpScare(jumpScare.GetComponent<Image>()));
        }

        if (energyEffect)
            StartCoroutine(EnergyEffectEnd());
    }

    [Command]
    public void CmdPlayerIsDead(bool isDead)
    {
        RpcPlayerIsDead(isDead);
    }

    [ClientRpc]
    void RpcPlayerIsDead(bool isDead)
    {
        if (isDead)
        {
            gameObject.layer = 7;
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 7;
            }
            gameObject.tag = "Untagged";
            playerIsDead.enabled = true;
            inventory.inventoryObject.SetActive(false);
            playerModel.SetActive(false);
            flashLight.enabled = false;
            Destroy(playerHand);
        }

        PlayersAlive();
    }

    void PlayersAlive()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length < 1)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameOver.SetActive(true);
        }
    }

    public void JumpScare()
    {
        wiiJumpScare.SetActive(true);
        whatIsItJumpscare = true;
        if (whatIsItJumpscare)
            StartCoroutine(JumpScare2());
    }

    IEnumerator EnergyEffectEnd()
    {
        energyEffect = false;
        energyEffectCheck = true;
        runningSpeed += 3;
        yield return new WaitForSeconds(15f);
        runningSpeed -= 3;
        energyEffectCheck = false;
    }

    IEnumerator JumpScare(Image image)
    {
        yield return new WaitForSeconds(5f);
        image.enabled = false;
    }

    IEnumerator JumpScare2()
    {
        yield return new WaitForSeconds(5f);
        wiiJumpScare.SetActive(false);
        whatIsItJumpscare = false;
    }

    IEnumerator RemoveTutorial()
    {
        playerTutorial.SetActive(true);
        yield return new WaitForSeconds(5f);
        playerTutorial.SetActive(false);
        yield return new WaitForSeconds(2f);
        playerTutorial.SetActive(true);
        playerTutorialText.text = "press ESC to access the settings menu";
        yield return new WaitForSeconds(5f);
        playerTutorial.SetActive(false);
        yield return new WaitForSeconds(2f);
        playerTutorial.SetActive(true);
        playerTutorialText.text = "press TAB to access your inventory";
        yield return new WaitForSeconds(5f);
        playerTutorial.SetActive(false);
        yield return new WaitForSeconds(2f);
        playerTutorial.SetActive(true);
        playerTutorialText.text = "press F to turn on/off your flashlight";
        yield return new WaitForSeconds(5f);
        playerTutorial.SetActive(false);
    }

    public bool IsGamePaused
    {
        get
        {
            return pauseMenu.activeSelf;
        }
    }

    public bool IsGameOver
    {
        get
        {
            return gameOver.activeSelf || playerPoints.gameWon.activeSelf;
        }
    }

    public bool IsInventoryActivated
    {
        get
        {
            return inventory.inventoryObject.activeSelf;
        }
    }

    public bool IsLocalPlayerAlive
    {
        get
        {
            return !isDead;
        }
    }
}