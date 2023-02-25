using Mirror;
using UnityEngine;

public class PlaceBone : NetworkBehaviour
{
    [SerializeField] private PlayersAlreadyJoined server;

    [SerializeField] private GameObject bonePrefab;

    private CurrentPoints current;
    private RitualComplet ritual;

    AudioSource audioSource => GetComponent<AudioSource>();

    void Start()
    {
        current = GameObject.FindGameObjectWithTag("PointHolder").GetComponent<CurrentPoints>();
        ritual = GameObject.FindGameObjectWithTag("Ritual").GetComponent<RitualComplet>();
    }

    private void Update()
    {
        if (!server.PlayersAlreadyJoinedInServer())
            return;

        CmdPlaceBone();
    }


    [Command (requiresAuthority = false)]
    void CmdPlaceBone()
    {
        if (!(transform.childCount < 1) || !(current.points > 0))
            return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        print("a");
        for (int i = 0; i < players.Length; i++)
        {
            if (Vector3.Distance(transform.position, players[i].transform.position) > 2)
                continue;

            GameObject prefab = Instantiate(bonePrefab, transform);
            NetworkServer.Spawn(prefab);
            audioSource.Play();
            ritual.currentBones++;
            current.points--;
        }
    }
}