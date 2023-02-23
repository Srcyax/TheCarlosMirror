using UnityEngine;

public class JumpScare : MonoBehaviour
{
    AudioSource audioSource => GetComponent<AudioSource>();
    void Update()
    {
        if (!audioSource.isPlaying)
            Destroy(gameObject, .4f);
    }
}
