using Mirror;
using TMPro;
using UnityEngine;

public class PlayerPoints : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playersPoint;

    [SerializeField] public GameObject gameWon;
    [SerializeField] public GameObject[] playerUI;

    private CurrentPoints current;

    void Start()
    {
        current = GameObject.FindGameObjectWithTag("PointHolder").GetComponent<CurrentPoints>(); ;
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        CmdPointText(current.points.ToString() + "/6");
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