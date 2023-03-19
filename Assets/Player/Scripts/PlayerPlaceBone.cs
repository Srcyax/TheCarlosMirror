using Mirror;
using UnityEngine;

public class PlayerPlaceBone : NetworkBehaviour {
    private CurrentPoints current;

    void Start() {
        current = GameObject.FindGameObjectWithTag("PointHolder").GetComponent<CurrentPoints>();
    }


    void Update() {
        if ( !( current.points > 0 ) )
            return;

        CmdPlaceBone();
    }

    [Command(requiresAuthority = false)]
    void CmdPlaceBone() {
        GameObject[] placeBoneRitual = GameObject.FindGameObjectsWithTag("RitualPos");

        for ( int j = 0; j < placeBoneRitual.Length; j++ ) {
            if ( Vector3.Distance(placeBoneRitual[j].transform.position, transform.position) > 2 )
                continue;

            if ( placeBoneRitual[j].transform.GetChild(0).gameObject.GetComponent<Animator>().enabled )
                continue;

            AudioSource audioSource = placeBoneRitual[j].GetComponent<AudioSource>();
            RitualComplet ritual = GameObject.FindGameObjectWithTag("Ritual").GetComponent<RitualComplet>();

            placeBoneRitual[j].transform.GetChild(0).gameObject.GetComponent<Animator>().enabled = true;
            audioSource.Play();
            current.points--;
            ritual.currentBones++;
        }
    }
}
