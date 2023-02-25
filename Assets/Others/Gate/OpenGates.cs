using Mirror;
using TMPro;
using UnityEngine;

public class OpenGates : NetworkBehaviour
{
    GameObject[] players;
    GameObject[] waitingForPlayers;
    [SerializeField] GameObject[] gates;
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
                Destroy(gate, 5f);
                Destroy(gameObject, 5f);
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
}