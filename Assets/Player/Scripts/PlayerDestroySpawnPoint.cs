using UnityEngine;
using Mirror;

public class PlayerDestroySpawnPoint : NetworkBehaviour
{   
    void Start()
    {
        CmdDestroySpawnPoint();
    }

    [Command(requiresAuthority = false)]
    void CmdDestroySpawnPoint()
    {
        GameObject[] spawn = GameObject.FindGameObjectsWithTag("SpawnPoint");

        for ( int i = 0; i < spawn.Length; i++ ) {
            if ( Vector3.Distance( transform.position, spawn[ i ].transform.position ) > 1f )
                continue;

            NetworkServer.Destroy( spawn[i] );
            break;
        }   
    }
}
