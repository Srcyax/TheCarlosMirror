using Mirror;
using TMPro;
using UnityEngine;

public class OpenGates : NetworkBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private GameObject[] gates;

    GameObject[] players;
    GameObject[] waitingForPlayers;

    void Update()
    {
        CmdOpenGates();
    }

    [Command(requiresAuthority = false)]
    void CmdOpenGates()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        waitingForPlayers = GameObject.FindGameObjectsWithTag("WaitingPlayers");

        if ((players.Length >= networkManager.maxConnections))
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