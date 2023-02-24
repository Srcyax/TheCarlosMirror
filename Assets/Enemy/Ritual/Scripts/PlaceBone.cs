using Mirror;
using UnityEngine;

public class PlaceBone : NetworkBehaviour
{
    [SerializeField] private CurrentPoints current;
    [SerializeField] private GameObject bonePrefab;

    [Space(10)]
    [SerializeField] private RitualComplet ritual;

    AudioSource audioSource => GetComponent<AudioSource>();

    private void Update()
    {
        CmdPlaceBone();
    }


    [Command (requiresAuthority = false)]
    void CmdPlaceBone()
    {
        if (!(transform.childCount < 1) || !(current.points > 0))
            return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        print(players.Length);
        for (int i = 0; i < players.Length; i++)
        {
            if (Vector3.Distance(transform.position, players[i].transform.position) > 3)
                continue;
            print(Vector3.Distance(transform.position, players[i].transform.position));
            Instantiate(bonePrefab, transform);
            audioSource.Play();
            current.points--;
            ritual.currentBones++;
        }
    }
}