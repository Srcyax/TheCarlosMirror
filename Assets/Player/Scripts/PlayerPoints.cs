using Mirror;
using TMPro;
using UnityEngine;

public class PlayerPoints : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playersPoint;
    [SerializeField] private PlayerController playerController;

    [SerializeField] public GameObject gameWon;
    [SerializeField] public GameObject[] playerUI;

    RitualComplet ritual;

    void Update()
    {
        if (!isLocalPlayer)
            return;

        ritual = GameObject.FindGameObjectWithTag("Ritual").GetComponent<RitualComplet>();

        CmdPointText(playerController.current.points.ToString() + "/6");
    }

    [Command]
    void CmdPointText(string point)
    {
        RpcPointText(point);
    }

    [ClientRpc]
    void RpcPointText(string point)
    {
        playersPoint.text = point;
    }
}