using Mirror;
using UnityEngine;

public class Hide : NetworkBehaviour, IInteraction
{
    public void Interact()
    {
        GetComponent<Animator>().Play("Open");
    }
}