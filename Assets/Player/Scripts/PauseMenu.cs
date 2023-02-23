using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject[] playerUI;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private PlayerController player;

    private PostProcessVolume postProcess;
    private DepthOfField depthOfField;
    private bool isPaused = false;

    void Start()
    {
        postProcess = playerCamera.GetComponent<PostProcessVolume>();
        depthOfField = postProcess.profile.GetSetting<DepthOfField>();
    }

    void Update()
    {
        if (inventory.activeSelf || player.isDead || player.playerPoints.gameWon.activeSelf)
            return;

        if (Input.GetKeyUp(KeyCode.Escape))
            isPaused = !isPaused;

        foreach (GameObject ui in playerUI)
        {
            ui.SetActive(isPaused);
        }
        pauseMenu.SetActive(isPaused);
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
        depthOfField.enabled.value = isPaused;
    }

    public void SettingsMenu(bool value)
    {
        settingsMenu.SetActive(value);
    }
}