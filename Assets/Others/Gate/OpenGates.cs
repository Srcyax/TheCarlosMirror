using Mirror;
using TMPro;
using UnityEngine;

public class OpenGates : NetworkBehaviour
{
    [SerializeField] private TMP_Dropdown gameMode;
    [SerializeField] private Transform enemyPosSapawn;
    [SerializeField] GameObject[] gates;
    GameObject[] players;
    GameObject[] waitingForPlayers;
    NetworkManager network;
    void Start()
    {
        network = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    void Update()
    {
        CmdOpenGates();
    }

    [Command(requiresAuthority = false)]
    void CmdOpenGates()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        waitingForPlayers = GameObject.FindGameObjectsWithTag("WaitingPlayers");

        if ((players.Length >= network.maxConnections))
        {
            foreach (GameObject wait in waitingForPlayers)
            {
                Destroy(wait);
            }

            foreach (GameObject gate in gates)
            {
                Destroy(gate);
                Destroy(gameObject);
                SpawnEnemys();
            }
        }
        else
        {
            foreach (GameObject wait in waitingForPlayers)
            {
                if (wait)
                    wait.GetComponent<TextMeshProUGUI>().enabled = true;
            }
        }      
    }

    void SpawnEnemys()
    {
        if (gameMode.value == 0)
        {
            NetworkServer.Destroy(enemyPosSapawn.GetChild(2).gameObject);
        }
        else if (gameMode.value == 1)
        {
            NetworkServer.Destroy(enemyPosSapawn.GetChild(1).gameObject);
            NetworkServer.Destroy(enemyPosSapawn.GetChild(2).gameObject);
        }
    }
}