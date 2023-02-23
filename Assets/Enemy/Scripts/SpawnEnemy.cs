using Mirror;
using UnityEngine;

public class SpawnEnemy : NetworkBehaviour
{
    [SerializeField] GameObject carlos;
    [SerializeField] Transform postion;

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 1)
        {
            if (Instantiate(carlos, postion))
                Destroy(gameObject);
        }
    }
}
