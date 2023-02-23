using UnityEngine;
using Mirror;
using TMPro;

public class PlayerName : NetworkBehaviour
{
    [SerializeField] private Settings settings;
    [SerializeField] public TextMeshPro playerName;
    [SerializeField] private TextMeshProUGUI inventoryPlayerName;

    void Update()
    {
        CmdPlayerNameUGUI();

        if (!isLocalPlayer)
            return;

        CmdPlayerName(settings.playerName);
    }


    [Command]
    void CmdPlayerName(string name)
    {
        RpcPlayerName(name);
    }

    [ClientRpc]
    void RpcPlayerName(string name)
    {
        playerName.text = name;
        inventoryPlayerName.text = name;
    }

    [Command(requiresAuthority = false)]
    void CmdPlayerNameUGUI()
    {
        RpcPlayerNameUGUI();
    }

    [ClientRpc]
    void RpcPlayerNameUGUI()
    {
        if (isLocalPlayer)
            return;

        playerName.enabled = Vector3.Distance(playerName.transform.position, Camera.main.transform.position) < 15;
        var targetRotation = Quaternion.LookRotation(Camera.main.transform.position - playerName.transform.position);
        playerName.transform.rotation = Quaternion.Slerp(playerName.transform.rotation, targetRotation, 8);
    }
}
