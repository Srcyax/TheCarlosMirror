using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class CruzSystem : NetworkBehaviour
{
    [SerializeField] Transform playerHand;

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (playerHand.transform.childCount > 0 && !playerHand.transform.GetChild(0).gameObject.CompareTag("Cruz"))
            return;

        bool pressSpace = Input.GetKey(KeyCode.Space);

        CmdCruzSystem(pressSpace, transform.position, transform.TransformDirection(Vector3.forward));
    }

    [Command]
    void CmdCruzSystem(bool pressSpace, Vector3 position, Vector3 forward)
    {
        RpcCruzSystem(pressSpace, position, forward);
    }

    [ClientRpc]
    void RpcCruzSystem(bool pressSpace, Vector3 position, Vector3 forward)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, forward, out hit, 41))
        {
            if (hit.collider.CompareTag("Enemy") && hit.distance < 30)
            {
                hit.collider.gameObject.GetComponent<EnemyAI>().cruzEffect = pressSpace;
            }
        }
    }
}