using UnityEngine;
using Mirror;

public class PlayerWhistle : NetworkBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] whistleClips;


    void Update()
    {
        if ( !isLocalPlayer )
            return;

        CmdWhistle();
    }

    [Command]
    void CmdWhistle()
    {
        RpcWhistle();
    }

    [ClientRpc]
    void RpcWhistle()
    { 
        if ( !Input.GetKeyDown( KeyCode.V ) )
            return;

        if ( audioSource.isPlaying )
            return;

        int sort_clip = Random.Range(whistleClips.Length - whistleClips.Length, whistleClips.Length);

        audioSource.clip = whistleClips[ sort_clip ];
        audioSource.Play();
    }
}