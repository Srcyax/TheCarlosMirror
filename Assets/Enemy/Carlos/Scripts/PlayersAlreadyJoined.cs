using Mirror;
using UnityEngine;

public class PlayersAlreadyJoined : MonoBehaviour {
    [SerializeField] private NetworkManager networkManager;

    private float timer;

    public bool PlayersAlreadyJoinedInServer() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        timer = players.Length >= networkManager.maxConnections ? timer + Time.deltaTime : timer;
        return timer > 10;
    }
}