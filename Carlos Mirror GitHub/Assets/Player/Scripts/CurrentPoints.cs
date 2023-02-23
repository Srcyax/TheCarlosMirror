using UnityEngine;
using Mirror;

public class CurrentPoints : NetworkBehaviour
{
    [SyncVar] public int points;
}