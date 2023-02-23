using Mirror;
using UnityEngine;

public class Hide : NetworkBehaviour
{
    [SerializeField] private PlayerController player;

    void Update()
    {
        if (!isLocalPlayer)
            return;

        bool pressE = Input.GetKeyUp(KeyCode.E);

        CmdInteractCabinet(pressE, transform.position, Camera.main.transform.TransformDirection(Vector3.forward));
    }

    [Command]
    void CmdInteractCabinet(bool pressE, Vector3 position, Vector3 forward)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, forward, out hit, 41))
        {
            if (pressE && hit.collider.CompareTag("Cabinet") && hit.distance < 3)
            {
                hit.collider.gameObject.transform.root.GetComponent<Animator>().Play("Open");
            }
        }
    }
}