using Mirror;
using UnityEngine;

public class PlayerGameOver : NetworkBehaviour
{
    [SerializeField] private Camera gameOverCamera;

    private GameObject[] enemys;

    void Update()
    {
        /*enemys = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        gameOverCamera.enabled = players.Length < 0;
        if (players.Length > 0)
            for (int i = 0; i < enemys.Length; ++i)
                NetworkServer.Destroy(enemys[i]);*/
    }
}
