using UnityEngine;
using Mirror;

public class PlayerWhistle : NetworkBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] whistleClips;
    [SyncVar] private int sort_clip;

    void Update()
    {
        if ( !isLocalPlayer )
            return;

        bool key = Input.GetKeyDown( KeyCode.V );

        CmdWhistle( key );
    }

    [Command]
    void CmdWhistle(bool key)
    {
        RpcWhistle(key);
    }

    [ClientRpc]
    void RpcWhistle(bool key)
    {
        if ( !key )
            return;

        if ( audioSource.isPlaying )
            return;

        sort_clip = Random.Range(whistleClips.Length - whistleClips.Length, whistleClips.Length);

        audioSource.clip = whistleClips[ sort_clip ];
        audioSource.Play();
    }
}