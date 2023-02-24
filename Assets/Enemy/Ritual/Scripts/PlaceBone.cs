using Mirror;
using UnityEngine;

public class PlaceBone : NetworkBehaviour
{
    [SerializeField] private CurrentPoints current;
    [SerializeField] private GameObject bonePrefab;

    [Space(10)]
    [SerializeField] private RitualComplet ritual;

    AudioSource audioSource => GetComponent<AudioSource>();

    private void OnTriggerEnter(Collider other)
    {
        if (!(transform.childCount < 1) || !(current.points > 0) || !other.gameObject.CompareTag("Player"))
            return;

        Instantiate(bonePrefab, transform);
        audioSource.Play();
        current.points--;
        ritual.currentBones++;
    }
}