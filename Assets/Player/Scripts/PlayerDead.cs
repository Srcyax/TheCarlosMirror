using Mirror;
using TMPro;
using UnityEngine;

public class PlayerDead : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playerIsDead;
    [SerializeField] private GameObject playerRagdoll;

    private PlayerInventory playerInventory => GetComponent<PlayerInventory>();
    private PlayerController player => GetComponent<PlayerController>();

    void Update()
    {
        CmdPlayerIsDead( player.isDead );
    }

    [Command( requiresAuthority = false )]
    public void CmdPlayerIsDead( bool isDead )
    {
        RpcPlayerIsDead( isDead );
    }

    bool ragdollAlreadyInstantiate = false;

    [ClientRpc]
    void RpcPlayerIsDead( bool isDead )
    {
        if ( !isDead )
            return;

        gameObject.layer = 7;
        foreach ( Transform child in transform ) {
            child.gameObject.layer = 7;
        }

        if ( !ragdollAlreadyInstantiate ) {
            ragdollAlreadyInstantiate = true;
            GameObject ragdoll = Instantiate(playerRagdoll, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            NetworkServer.Spawn( ragdoll );
        }

        gameObject.tag = "Untagged";
        playerIsDead.enabled = true;
        playerInventory.inventoryObject.SetActive( false );
        player.playerModel.SetActive( false );
        player.flashLight.enabled = false;
        Destroy( player.playerHand );

        HasPlayersAlive();
    }

    void HasPlayersAlive()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if ( players.Length > 0 )
            return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.gameOver.SetActive( true );
    }
}
