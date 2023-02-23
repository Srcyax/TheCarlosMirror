using Mirror;
using System.Collections;
using TMPro;
using UnityEngine;

public class ItemFeed : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI itemFeed;

    [Command(requiresAuthority = false)]
    public void CmdItemFeedCallback(string user, string mensage)
    {
        RpcItemFeedCallback(user, mensage);
    }

    [ClientRpc]
    void RpcItemFeedCallback(string user, string mensage)
    {
        itemFeed.text = "";
        itemFeed.text += ("\n" + user + mensage);
    }
}