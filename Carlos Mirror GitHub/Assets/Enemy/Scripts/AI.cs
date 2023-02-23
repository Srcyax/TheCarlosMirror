using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AI : MonoBehaviour
{
    [SerializeField] Transform playerPosition;

    [SerializeField] GameObject jumpScareImage;

    [Header("Audio Clip's")]
    [SerializeField] GameObject jumpScareAudio; 

    NavMeshAgent navMeshAgent => GetComponent<NavMeshAgent>();
    AudioSource audioSource => GetComponent<AudioSource>();

    public bool cruzEffect = false;

    void Start()
    {
        audioSource.Play();
    }

    void Update()
    {
        navMeshAgent.destination = playerPosition.position;

        if (cruzEffect) { 
            StartCoroutine(CruzEffect());
            navMeshAgent.speed = 1;
        }
        else
            navMeshAgent.speed = 19;

        JumpScare();
    }

    void JumpScare()
    {
        if (Vector3.Distance(transform.position, playerPosition.position) > 2) 
            return;

        jumpScareImage.SetActive(true);
        jumpScareAudio.SetActive(true);
        audioSource.Stop();
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Game");
    }

    IEnumerator CruzEffect()
    {
        cruzEffect = false;
        yield return new WaitForSeconds(3f);
    }
}
