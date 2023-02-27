using Mirror;
using UnityEngine;

public class WhatIsIt : NetworkBehaviour
{
    Animator animator => GetComponent<Animator>();
    private string anim;

    void Update()
    {
        ShowQuestionMark();
    }

    void ShowQuestionMark()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            anim = Vector3.Distance(transform.position, Camera.main.transform.position) < 15 ? "UnHide" : "Hide";
            animator.Play(anim);
            players[i].GetComponent<PlayerController>().whatIsItJumpscare = Vector3.Distance(transform.position, players[i].transform.position) < 4;
            if (players[i].GetComponent<PlayerController>().whatIsItJumpscare)
                NetworkServer.Destroy(gameObject);
        }
    }
}
