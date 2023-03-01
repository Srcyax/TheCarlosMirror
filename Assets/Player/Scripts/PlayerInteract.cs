using Mirror;
using UnityEngine;

public interface IInteraction
{
    void Interact();
}

public class PlayerInteract : NetworkBehaviour
{
    void Update()
    {
        bool pressKey = Input.GetKeyDown(KeyCode.E);
        CmdPlayerInteract(pressKey, transform.position, Camera.main.transform.TransformDirection(Vector3.forward));
    }

    [Command(requiresAuthority = false)]
    void CmdPlayerInteract(bool pressKey, Vector3 pos, Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(pos, dir, out hit, 41))
        {
            if (pressKey && hit.distance < 3 && hit.collider)
            {
                IInteraction interaction = hit.collider.gameObject.GetComponent<IInteraction>();
                interaction?.Interact();
            }
        }
    }
}