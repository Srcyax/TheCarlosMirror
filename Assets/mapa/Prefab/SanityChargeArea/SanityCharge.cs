using UnityEngine;

public class SanityCharge : MonoBehaviour
{
    float time;
    private void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0;i < players.Length; i++)
        {
            if ( !players[i].GetComponent<PlayerSanity>() )
                continue;

            if ( Vector3.Distance(transform.position, players[i].transform.position) > 5f || players[i].GetComponent<PlayerSanity>().sanity >= 100f )
                continue;

            time = time < 5 ? time + Time.deltaTime : time;

            if ( time >= 4 )
            {
                players[i].GetComponent<PlayerSanity>().sanity = Mathf.Lerp(players[i].GetComponent<PlayerSanity>().sanity, 100, Time.deltaTime);
            }
        }
    }
}