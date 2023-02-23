using Mirror;
using System;
using System.Collections;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]

[Serializable]
public class PlayerController : NetworkBehaviour
{
    [Serializable]
    public sealed class Test {
        public bool cyu;
    }

    [Header("Settings")]
    [Tooltip("Get current settings player")]
    [SerializeField] private Settings settings;
    [SerializeField] private Slider sensitivity;
    [SerializeField] private TMP_Dropdown graphics;

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
    [SerializeField] private GameObject playerHand;
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

    [Space(10)]
    [Header("Player flashlight")]
    [SerializeField] private Light flashLight;
    [SerializeField] private Sway swayFlashlight;

    [Space(10)]
    [Header("Player screen Game state")]
    [SerializeField] private GameObject gameOver;

    [Space(10)]
    [Header("Player inventory components")]
    [SerializeField] private GameObject inventory;
    [SerializeField] private Transform selectedSlot;
    [SerializeField] private GameObject[] inventoryObjects;

    [Header("Stamine")]
    [SerializeField] private Slider stamineSlider;
    [SerializeField] private float stamine = 100;


    [HideInInspector] public bool energyEffect = false;
    [HideInInspector] bool energyEffectCheck = false;
    [HideInInspector] public bool eyeEffect = false;
    [HideInInspector] public CurrentPoints current;
    [SyncVar] [HideInInspector] public bool isDead = false;

    #region Others
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    GameObject[] players;
    public float walkingSpeed = .1f;
    public float runningSpeed = 10.7f;
    public bool inventoryActive = false;
    float gravity = 20.0f;
    float lookXLimit = 80.0f;
    float rotationX = 0;
    PostProcessVolume postProcess;
    ChromaticAberration chromaticAberration;
    Bloom bloom;
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
        Inventory();
        CheckEffects();
        Graphics();

        // Command server
        CmdPlayerIsDead(isDead);
    }

    void PlayerControler()
    {
        if (pauseMenu.activeSelf || gameOver.activeSelf || inventory.activeSelf || playerPoints.gameWon.activeSelf)
            return;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && stamine > 1;

        float curSpeedX = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical");
        float curSpeedY = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal");
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity;
        }

        characterController.Move(moveDirection * Time.deltaTime);
        rotationX += -Input.GetAxis("Mouse Y") * sensitivity.value;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sensitivity.value, 0);
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
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W);

        if (isRunning && !energyEffectCheck)
        {
            stamine -= Time.fixedDeltaTime * 5;
        }
        else
        {
            if (stamine < 100)
                stamine += Time.fixedDeltaTime * 7;
        }

        breath.volume = isRunning ? breath.volume + Time.fixedDeltaTime * 0.3f : breath.volume - Time.fixedDeltaTime * 0.1f;

        stamineSlider.value = stamine;
        stamineSlider.gameObject.SetActive(stamineSlider.value < 100);
    }

    public void Disconnect()
    {
        var holdSense = sensitivity.value;
        var holdGraphics = graphics.value;

        File.WriteAllText(@"userdata/sense.txt", holdSense.ToString());
        File.WriteAllText(@"userdata/graphics.txt", holdGraphics.ToString());
        File.WriteAllText(@"userdata/tutorial.txt", "false");

        Application.Quit();
    }

    void Graphics()
    {
        QualitySettings.SetQualityLevel(graphics.value);
        postProcess.weight = graphics.value < 2 ? 1f : .25f;
    }

    string animName;
    void Inventory()
    {
        if (pauseMenu.activeSelf || isDead || playerPoints.gameWon.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
            inventoryActive = !inventoryActive;

        inventory.SetActive(inventoryActive);

        flashLight.enabled = inventory.activeSelf || flashLight.enabled;
        chromaticAberration.intensity.value = inventory.activeSelf ? 0.04f : .5f;
        bloom.intensity.value = inventory.activeSelf ? 1f : 23f;

        Cursor.lockState = inventory.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = inventory.activeSelf;

        animName = inventory.activeSelf ? "Hide" : "UnHide";
        playerHand.GetComponent<Animator>().Play(animName);

        CleanSelectedSlot();
    }

    void CleanSelectedSlot()
    {
        if (inventory.activeSelf)
            return;

        if (selectedSlot.childCount > 0)
        {
            for (int i = 0; i < inventoryObjects.Length; i++)
            {
                if (inventoryObjects[i].transform.GetChild(0).childCount > 0 && !inventoryObjects[i].transform.GetChild(0).transform.GetChild(0).CompareTag(selectedSlot.transform.GetChild(0).gameObject.tag))
                    continue;

                if (inventoryObjects[i].transform.GetChild(0).childCount > 0 && !(inventoryObjects[i].transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<ItemInfo>().itemType == ItemType.Consumable))
                    continue;

                if (Instantiate(selectedSlot.transform.GetChild(0).gameObject, inventoryObjects[i].transform.GetChild(0).transform))
                {
                    Destroy(selectedSlot.transform.GetChild(0).gameObject);
                    break;
                }
            }
        }
    }

    private void LocalPlayerStart()
    {
        sensitivity.value = settings.sensitivy;
        graphics.value = settings.graphics;

        GameObject.FindGameObjectWithTag("CanvasMenu")?.SetActive(false);
        playerCanvas.SetActive(isLocalPlayer);
        playerCamera.GetComponent<AudioListener>().enabled = isLocalPlayer;
        playerCamera.enabled = isLocalPlayer;
        playerNameSystem.playerName.enabled = !isLocalPlayer;
        playerModel.SetActive(!isLocalPlayer);
        inventory.SetActive(isLocalPlayer);
        swayFlashlight.enabled = isLocalPlayer;
        characterController = GetComponent<CharacterController>();
        postProcess = playerCamera.GetComponent<PostProcessVolume>();
        postProcess.enabled = isLocalPlayer;
        chromaticAberration = postProcess.profile.GetSetting<ChromaticAberration>();
        bloom = postProcess.profile.GetSetting<Bloom>();

        current = GameObject.FindGameObjectWithTag("PointHolder").GetComponent<CurrentPoints>();
        waitForPlayers.enabled = isServer;

        if (settings.tutorial)
        {
            StartCoroutine(RemoveTutorial());
        }
    }

    void CheckEffects()
    {
        if (isDead)
        {
            jumpScare.SetActive(true);
            StartCoroutine(JumpScare(jumpScare.GetComponent<Image>()));
        }
        if (energyEffect)
        {
            StartCoroutine(EnergyEffectEnd());
        }
        if (eyeEffect)
        {
            StartCoroutine(EyeEffectEnd());
        }
    }

    [Command]
    public void CmdPlayerIsDead(bool isDead)
    {
        RpcPlayerIsDead(isDead);
    }

    [ClientRpc]
    void RpcPlayerIsDead(bool isDead)
    {
        if (!isDead)
            return;

        gameObject.layer = 7;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = 7;
        }
        gameObject.tag = "Untagged";
        Destroy(playerModel);
        playerIsDead.enabled = true;

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

    IEnumerator EyeEffectEnd()
    {
        eyeEffect = false;
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        chromaticAberration.intensity.value = 1f;
        if (Vector3.Distance(transform.position, enemy.transform.position) <= 70)
        {
            print(Vector3.Distance(transform.position, enemy.transform.position));
            enemy.GetComponent<Outline>().enabled = true;
        }
        yield return new WaitForSeconds(7f);
        chromaticAberration.intensity.value = 0.246f;
        enemy.GetComponent<Outline>().enabled = false;
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
}