using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMain : NetworkBehaviour
{
    [SerializeField] private PlayersAlreadyJoined server;
    [Space(10)]

    [SerializeField] private float visionDistance = 316.5f;
    [SerializeField] private float radiusLayer = 74.2f;
    [SerializeField] private float visionAngle = 250f;
    [SerializeField] private float distanceBetweenLayers = 0.03f;

    [SerializeField] private int numberOfLayers = 24;

    private const string targetTag = "Player";

    private float timeToCheck = 0;
    private float check = 0;

    [SerializeField] private Transform enemyHead;
    [SerializeField] NetworkManager networkManager;
    [Space(10)]
    [SyncVar] public List<Transform> visibleTarget = new List<Transform>();
    [SyncVar] public List<Transform> collisionList = new List<Transform>();


    void Update()
    {
        if (!server.PlayersAlreadyJoinedInServer())
            return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        AIPerformance(players);

        check += Time.fixedDeltaTime;
        if (check > timeToCheck)
        {
            CmdChecarInimigos();
            check = 0;
        }    
    }

    [Command(requiresAuthority = false)]
    private void CmdChecarInimigos()
    {
        float limiteCamadas = numberOfLayers * 0.5f;
        for (int x = 0; x <= radiusLayer; x++)
        {
            for (float y = -limiteCamadas + 0.5f; y <= limiteCamadas; y++)
            {
                float angleToRay = x * (visionAngle / radiusLayer) + ((180.0f - visionAngle) * 0.5f);
                Vector3 directionMultipl = (-enemyHead.right) + (enemyHead.up * y * distanceBetweenLayers);
                Vector3 rayDirection = Quaternion.AngleAxis(angleToRay, enemyHead.up) * directionMultipl;
                RaycastHit hitRaycast;
                if (Physics.Raycast(enemyHead.position, rayDirection, out hitRaycast, visionDistance))
                {
                    if (hitRaycast.collider.gameObject.CompareTag(targetTag) && visibleTarget.Count < 1)
                    {
                        Debug.DrawLine(enemyHead.position, hitRaycast.point, Color.red);

                        if (!collisionList.Contains(hitRaycast.transform))
                        {
                            collisionList.Add(hitRaycast.transform);
                        }
                        if (!visibleTarget.Contains(hitRaycast.transform))
                        {
                            visibleTarget.Add(hitRaycast.transform);
                        }
                    }
                }
                else
                {
                    Debug.DrawRay(enemyHead.position, rayDirection * visionDistance, Color.green);
                }
            }
        }
    }

    void AIPerformance(GameObject[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (Vector3.Distance(transform.position, players[i].transform.position) > 50)
            {
                timeToCheck = 5f;
                continue;
            }

            timeToCheck = 0.1f;
        }
    }

    public void ClearTargets()
    {
        for (int x = 0; x < visibleTarget.Count; x++)
        {
            if (!collisionList.Contains(visibleTarget[x]))
            {
                visibleTarget.Remove(visibleTarget[x]);
            }
        }
        collisionList.Clear();
    }
}