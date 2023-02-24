using Mirror;
using UnityEngine;

public class PlaceBone : NetworkBehaviour
{
    [SerializeField] private GameObject bonePrefab;

    private CurrentPoints current;
    private RitualComplet ritual;
    private NetworkManager networkManager;

    AudioSource audioSource => GetComponent<AudioSource>();

    void Start()
    {
        current = GameObject.FindGameObjectWithTag("PointHolder").GetComponent<CurrentPoints>();
        ritual = GameObject.FindGameObjectWithTag("Ritual").GetComponent<RitualComplet>();
        networkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    int test;
    private void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length >= networkManager.maxConnections && test < 10)
        {
            test++;
        }
        if (test > 0)
            CmdPlaceBone();
    }


    [Command (requiresAuthority = false)]
    void CmdPlaceBone()
    {
        RpcPlaceBone();
    }

    [ClientRpc]
    void RpcPlaceBone()
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