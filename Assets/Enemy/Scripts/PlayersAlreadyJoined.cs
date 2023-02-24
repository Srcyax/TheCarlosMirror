using Mirror;
using UnityEngine;

public class PlayersAlreadyJoined : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;

    private int timer;

    public bool PlayersAlreadyJoinedInServer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        timer = players.Length >= networkManager.maxConnections ? timer + 1 : timer;

        return timer > 0;      
    }
}
