using Mirror;
using UnityEngine;

public class PlaceBone : NetworkBehaviour
{
    [SerializeField] private CurrentPoints current;
    [SerializeField] private GameObject bonePrefab;

    [Space(10)]
    [SerializeField] private RitualComplet ritual;
    [Space(10)]
    [SerializeField] private NetworkManager networkManager;

    AudioSource audioSource => GetComponent<AudioSource>();

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

        for (int i = 0; i < players.Length; i++)
        {
            if (Vector3.Distance(transform.position, players[i].transform.position) > 3)
                continue;
            print(Vector3.Distance(transform.position, players[i].transform.position));
            GameObject prefab = Instantiate(bonePrefab, transform);
            NetworkServer.Spawn(prefab);
            audioSource.Play();
            ritual.currentBones++;
            current.points--;
        }
    }
}