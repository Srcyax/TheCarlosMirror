using Mirror;
using UnityEngine;

public class PlaceBone : NetworkBehaviour
{
    [SerializeField] private PlayersAlreadyJoined server;

    [SerializeField] private GameObject bonePrefab;

    private CurrentPoints current;
    private RitualComplet ritual;

    AudioSource audioSource => GetComponent<AudioSource> ();

    void Start()
    {
        current = GameObject.FindGameObjectWithTag ( "PointHolder" ).GetComponent<CurrentPoints> ();
        ritual = GameObject.FindGameObjectWithTag ( "Ritual" ).GetComponent<RitualComplet> ();
    }

    private void Update()
    {

    }

    [Command ( requiresAuthority = false )]
    private void CmdPlaceBone()
    {
        GameObject prefab = Instantiate(bonePrefab, transform);
        NetworkServer.Spawn ( prefab );
        audioSource.Play ();
        current.points--;
        ritual.currentBones++;
    }
}