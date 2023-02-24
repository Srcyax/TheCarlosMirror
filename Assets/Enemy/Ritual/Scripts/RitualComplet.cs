using Mirror;
using UnityEngine;

public class RitualComplet : NetworkBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject enemyRitualPrefab;
    [SerializeField] private Transform enemyRitualPos;
    [SerializeField] private Camera ritualCamera;
    AudioSource audioSource => GetComponent<AudioSource>();

    [SyncVar] public int currentBones = 0;

    void Update()
    {
        CmdRitualComplet(currentBones);
    }

    [Command]
    void CmdRitualComplet(int bones)
    {
        if (!(bones > 5))
            return;

        ritualCamera.enabled = true;
        audioSource.Play();
        Instantiate(enemyRitualPrefab, enemyRitualPos);
        NetworkServer.Destroy(enemy);
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