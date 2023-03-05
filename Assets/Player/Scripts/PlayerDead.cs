using Mirror;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerDead : NetworkBehaviour
{ 
    [SerializeField] private TextMeshProUGUI playerIsDead;
    [SerializeField] private GameObject playerRagdoll;
    private PlayerInventory playerInventory => GetComponent<PlayerInventory>();
    private PlayerController player => GetComponent<PlayerController>();

    void Update()
    {
        CmdPlayerIsDead(player.isDead);
    }

    [Command]
    public void CmdPlayerIsDead(bool isDead)
    {
        RpcPlayerIsDead(isDead);
    }

    bool ragdollAlreadyInstantiate = false;

    [ClientRpc]
    void RpcPlayerIsDead(bool isDead)
    {
        if (isDead)
        {
            gameObject.layer = 7;
            foreach (Transform child in transform)
            {
                child.gameObject.layer = 7;
            }

            if (!ragdollAlreadyInstantiate)
            {
                ragdollAlreadyInstantiate = true;
                GameObject ragdoll = Instantiate(playerRagdoll, transform);
                NetworkServer.Spawn(ragdoll);
            }

            gameObject.tag = "Untagged";
            playerIsDead.enabled = true;
            playerInventory.inventoryObject.SetActive(false);
            player.playerModel.SetActive(false);
            player.flashLight.enabled = false;
            Destroy(player.playerHand);
        }

        PlayersAlive();
    }

    void PlayersAlive()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length < 1)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            player.gameOver.SetActive(true);
        }
    }
}
