using Mirror;
using UnityEngine;
using UnityEngine.AI;

public enum NPCAIstate
{
    walking,
    searchBone,
    run,
    interacting
};

[RequireComponent(typeof(NavMeshAgent))]
public class NpcAI : NetworkBehaviour
{
    private GameObject[] enemyTarget;

    [SerializeField] Animator animator;
    NavMeshAgent navMesh => GetComponent<NavMeshAgent>();

    [SerializeField] private CarlosSetup carlosSetup;

    [SerializeField] public NPCAIstate stateAI;
    [SerializeField] private GameObject playerRagdoll;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private Light flashLight;
    private GameObject[] wayPoints;
    private GameObject[] interactPoints;
    private GameObject[] bones;

    [SyncVar] float wayPointTime = 0;
    [SyncVar] float wayPointDistance;
    [SyncVar] int currentWayPoint;
    [SyncVar] public bool IsDead;
    [SyncVar] private bool IsTarget;

    PlayersAlreadyJoined server;

    void Start()
    {
        enemyTarget = GameObject.FindGameObjectsWithTag("Enemy");
        wayPoints = GameObject.FindGameObjectsWithTag("NPCWalkPoint");
        interactPoints = GameObject.FindGameObjectsWithTag("NPCInteractPoint");
        bones = GameObject.FindGameObjectsWithTag("Points");

        Physics.IgnoreLayerCollision(0, 11);
        currentWayPoint = Random.Range(0, wayPoints.Length);
        stateAI = NPCAIstate.walking;

        server = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<PlayersAlreadyJoined>();
    }

    void Update()
    {
        if ( !server.PlayersAlreadyJoinedInServer() )
            return;

        CmdMainCode();
        CmdPlayerIsDead(IsDead);
    }

    [Command(requiresAuthority = false)]
    void CmdMainCode()
    {
        if ( IsDead )
            return;

        wayPointDistance = Vector3.Distance(wayPoints[currentWayPoint].transform.position, transform.position);
        switch ( stateAI )
        {
            case NPCAIstate.walking:
                Walking();
                break;
            case NPCAIstate.searchBone:
                SearchForBones();
                break;
            case NPCAIstate.run:
                RunAway();
                break;
            case NPCAIstate.interacting:
                RunAway();
                break;
        }
    }

    void Walking()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(wayPoints[currentWayPoint].transform.position - transform.position), Time.deltaTime);
        IsTarget = false;
        if ( wayPointDistance < 2 )
        {
            wayPointTime += Time.deltaTime;
            SetDestinatation(Vector3.zero, 0, false, false, true, false);
            if ( wayPointTime > 4 )
            {
                currentWayPoint = Random.Range(0, wayPoints.Length);

                if ( Vector3.Distance(wayPoints[currentWayPoint].transform.position, transform.position) < 40 )
                    currentWayPoint = Random.Range(0, wayPoints.Length);

                stateAI = NPCAIstate.walking;
                wayPointTime = 0;
            }
        }
        else
        {
            SetDestinatation(wayPoints[currentWayPoint].transform.position, carlosSetup.minVelocity, true, false, false, false);
        }

        SearchForBones();
        RunAway();
        InteractionPoints();
    }

    void RunAway()
    {
        for ( int i = 0; i < enemyTarget.Length; i++ )
        {
            if ( !enemyTarget[i] || !( enemyTarget[i].GetComponent<EnemyMain>().visibleTarget.Count > 0 ) || !( enemyTarget[i].GetComponent<EnemyMain>().visibleTarget.Contains(gameObject.transform) ) )
            {
                stateAI = NPCAIstate.walking;
                continue;
            }
            IsTarget = true;
            stateAI = NPCAIstate.run;           
            if ( wayPointDistance < 2 )
            {
                currentWayPoint = Random.Range(0, wayPoints.Length);

                if ( Vector3.Distance(wayPoints[currentWayPoint].transform.position, transform.position) < 40 )
                    currentWayPoint = Random.Range(0, wayPoints.Length);
            }
            else
            {
                SetDestinatation(wayPoints[currentWayPoint].transform.position, carlosSetup.maxVelocity, false, true, false, false);
            }   
        }
    }

    void SearchForBones()
    {
        if ( IsTarget )
            return;

        for ( int i = 0; i < bones.Length; ++i )
        {
            if ( !bones[i] || Vector3.Distance(transform.position, bones[i].transform.position) > 20 )
            {
                stateAI = NPCAIstate.walking;
                continue;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(bones[i].transform.position - transform.position), Time.deltaTime);
            stateAI = NPCAIstate.searchBone;
            SetDestinatation(bones[i].transform.position, carlosSetup.minVelocity, true, false, false, false);
        }
    }

    float interactTime;
    void InteractionPoints()
    {
        if ( IsTarget )
            return;

        for ( int i = 0; i < interactPoints.Length; i++ )
        {
            if ( Vector3.Distance(transform.position, interactPoints[i].transform.position) > 15 || !interactPoints[i] .gameObject.activeSelf)
                continue;

            stateAI = NPCAIstate.interacting;

            if (Vector3.Distance(transform.position, interactPoints[i].transform.position) > 5 )
            {
                SetDestinatation(interactPoints[i].transform.position, carlosSetup.minVelocity, true, false, false, false);
            }
            else
            {
                interactTime = interactTime < 5 ? interactTime + Time.deltaTime : interactTime;
                SetDestinatation(interactPoints[i].transform.position, 0, false, false, false, true);
                if ( interactTime > 4)
                {
                    interactPoints[i].gameObject.SetActive(false);
                    stateAI = NPCAIstate.walking;
                }            
            }
        }
    }

    void AnimationsManager(bool walk, bool run, bool idle, bool interacting)
    {
        animator.SetBool("walk", walk);
        animator.SetBool("run", run);
        animator.SetBool("idle", idle);
        animator.SetBool("interacting", interacting);
    }

    void SetDestinatation(Vector3 target, float speed, bool walk, bool run, bool idle, bool interacting)
    {
        AnimationsManager(walk, run, idle, interacting);
        navMesh.speed = speed;
        navMesh.SetDestination(target);
    }

    [Command (requiresAuthority = false)]
    public void CmdPlayerIsDead(bool isDead)
    {
        RpcPlayerIsDead(isDead);
    }

    bool ragdollAlreadyInstantiate = false;

    [ClientRpc]
    void RpcPlayerIsDead(bool isDead)
    {
        if ( !isDead )
            return;

        gameObject.layer = 7;
        foreach ( Transform child in transform )
        {
            child.gameObject.layer = 7;
        }

        if ( !ragdollAlreadyInstantiate )
        {
            ragdollAlreadyInstantiate = true;
            GameObject ragdoll = Instantiate(playerRagdoll, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            NetworkServer.Spawn(ragdoll);
        }

        gameObject.tag = "Untagged";
        playerModel.SetActive(false);
        flashLight.enabled = false;
    }
}