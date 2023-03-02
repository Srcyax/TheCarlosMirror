using Mirror;
using UnityEngine;

public class RitualComplet : NetworkBehaviour
{
    
    [SerializeField] private GameObject enemyRitualPrefab;
    [SerializeField] private Transform enemyRitualPos;
    [SerializeField] private Camera ritualCamera;
    AudioSource audioSource => GetComponent<AudioSource>();

    [SyncVar] public int currentBones = 0;

    private GameObject[] enemy;

    private void Start()
    {
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {
        CmdRitualComplet(currentBones);
    }

    [Command (requiresAuthority = false)]
    void CmdRitualComplet(int bones)
    {
        if (!(bones > 5))
            return;

        ritualCamera.enabled = true;
        audioSource.Play();
        Instantiate(enemyRitualPrefab, enemyRitualPos);
        for (int i = 0; i < enemy.Length; i++)
            NetworkServer.Destroy(enemy[i]);
        currentBones = 0;
        PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in players)
        {
            player.playerPoints.gameWon.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}