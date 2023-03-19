using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum NPCAIstate {
    walking,
    searchBone,
    placeBone,
    run,
    interacting
};

[RequireComponent(typeof(NavMeshAgent))]
public class NpcAI : NetworkBehaviour {
    [SerializeField] private CarlosSetup carlosSetup;

    [SerializeField] private Animator animator;
    [SerializeField] private NPCAIstate stateAI;
    [SerializeField] private GameObject playerRagdoll;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private Light flashLight;

    [SyncVar] private float wayPointTime = 0;
    [SyncVar] private float wayPointDistance;
    [SyncVar] private int currentWayPoint;
    [SyncVar] private bool IsTarget;
    [SyncVar] public bool IsDead;

    private PlayersAlreadyJoined server;
    private NavMeshAgent navMesh;
    private GameObject[] wayPoints;
    private GameObject[] interactPoints;
    private GameObject[] bones;
    private GameObject[] enemyTarget;
    private bool move = false;

    void Awake() {
        navMesh = GetComponent<NavMeshAgent>();
        Physics.IgnoreLayerCollision(0, 11);
        stateAI = NPCAIstate.walking;
    }

    void Start() {
        enemyTarget = GameObject.FindGameObjectsWithTag("Enemy");
        wayPoints = GameObject.FindGameObjectsWithTag("NPCWalkPoint");
        interactPoints = GameObject.FindGameObjectsWithTag("NPCInteractPoint");
        bones = GameObject.FindGameObjectsWithTag("Points");
        server = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<PlayersAlreadyJoined>();

        /* random walk point */
        currentWayPoint = Random.Range(0, wayPoints.Length);
    }

    void Update() {
        if ( !server.PlayersAlreadyJoinedInServer() )
            return;

        if ( !this.gameObject )
            return;

        if ( !move )
            StartCoroutine(Move());

        CmdMainCode();
        CmdPlayerIsDead(IsDead);
    }

    IEnumerator Move() {
        yield return new WaitForSeconds(2f);
        move = true;
    }

    [Command(requiresAuthority = false)]
    void CmdMainCode() {
        if ( IsDead )
            return;

        wayPointDistance = Vector3.Distance(wayPoints[currentWayPoint].transform.position, transform.position);
        IsTarget = stateAI == NPCAIstate.run;
        switch ( stateAI ) {
            case NPCAIstate.walking:
                Walking();
                break;
            case NPCAIstate.searchBone:
                SearchForBones();
                break;
            case NPCAIstate.placeBone:
                PlaceBones();
                break;
            case NPCAIstate.run:
                RunAway();
                break;
            case NPCAIstate.interacting:
                RunAway();
                break;
        }
    }

    void Walking() {
        if ( wayPointDistance < 2 ) {
            wayPointTime += Time.deltaTime;
            if ( wayPointTime > 4 ) {
                currentWayPoint = Random.Range(0, wayPoints.Length);
                stateAI = NPCAIstate.walking;
                wayPointTime = 0;
            }
        }

        Vector3 location = wayPointDistance < 2 ? Vector3.zero : wayPoints[currentWayPoint].transform.position;
        SetDestinatation(location, carlosSetup.minVelocity, location != Vector3.zero, false, !( location != Vector3.zero ), false);

        SearchForBones();
        RunAway();
        InteractionPoints();
        PlaceBones();
    }

    void RunAway() {
        for ( int i = 0; i < enemyTarget.Length; i++ ) {
            if ( !enemyTarget[i] || !( enemyTarget[i].GetComponent<EnemyMain>().visibleTarget.Count > 0 ) || !( enemyTarget[i].GetComponent<EnemyMain>().visibleTarget.Contains(gameObject.transform) ) ) {
                stateAI = NPCAIstate.walking;
                continue;
            }

            stateAI = NPCAIstate.run;
            currentWayPoint = wayPointDistance < 2 ? Random.Range(0, wayPoints.Length) : currentWayPoint;
            SetDestinatation(wayPoints[currentWayPoint].transform.position, carlosSetup.maxVelocity, false, true, false, false);
        }
    }

    void SearchForBones() {
        if ( IsTarget )
            return;

        for ( int i = 0; i < bones.Length; ++i ) {
            if ( !bones[i] || Vector3.Distance(transform.position, bones[i].transform.position) > 20 ) {
                stateAI = NPCAIstate.walking;
                continue;
            }

            stateAI = NPCAIstate.searchBone;
            SetDestinatation(bones[i].transform.position, carlosSetup.minVelocity, true, false, false, false);
        }
    }

    void PlaceBones() {
        if ( IsTarget )
            return;

        GameObject[] bonePos = GameObject.FindGameObjectsWithTag("RitualPos");
        GameObject points = GameObject.FindGameObjectWithTag("PointHolder");

        for ( int i = 0; i < bonePos.Length; i++ ) {
            if ( Vector3.Distance(transform.position, bonePos[i].transform.position) > 20 || !( points.GetComponent<CurrentPoints>().points > 0 ) || bonePos[i].transform.GetChild(0).GetComponent<Animator>().enabled ) {
                stateAI = NPCAIstate.walking;
                continue;
            }

            stateAI = NPCAIstate.placeBone;

            if ( Vector3.Distance(transform.position, bonePos[i].transform.position) > 1.5f ) {
                SetDestinatation(bonePos[i].transform.position, carlosSetup.minVelocity, true, false, false, false);
            }
            else {
                SetDestinatation(bonePos[i].transform.position, 0, false, false, true, false);
                AudioSource audioSource = bonePos[i].GetComponent<AudioSource>();
                RitualComplet ritual = GameObject.FindGameObjectWithTag("Ritual").GetComponent<RitualComplet>();

                bonePos[i].transform.GetChild(0).gameObject.GetComponent<Animator>().enabled = true;
                audioSource.Play();
                points.GetComponent<CurrentPoints>().points--;
                ritual.currentBones++;
                stateAI = NPCAIstate.walking;
            }
        }
    }

    float interactTime;
    void InteractionPoints() {
        if ( IsTarget )
            return;

        for ( int i = 0; i < interactPoints.Length; i++ ) {
            if ( Vector3.Distance(transform.position, interactPoints[i].transform.position) > 15 || !interactPoints[i].gameObject.activeSelf )
                continue;

            stateAI = NPCAIstate.interacting;
            if ( Vector3.Distance(transform.position, interactPoints[i].transform.position) > 1.5f ) {
                SetDestinatation(interactPoints[i].transform.position, carlosSetup.minVelocity, true, false, false, false);
            }
            else {
                interactTime = interactTime < 5 ? interactTime + Time.deltaTime : interactTime;
                SetDestinatation(interactPoints[i].transform.position, 0, false, false, false, true);
                if ( interactTime > 4 ) {
                    interactPoints[i].SetActive(false);
                    stateAI = NPCAIstate.walking;
                    interactTime = 0;
                }
            }
        }
    }

    void AnimationsManager(bool walk, bool run, bool idle, bool interacting) {
        animator.SetBool("walk", walk);
        animator.SetBool("run", run);
        animator.SetBool("idle", idle);
        animator.SetBool("interacting", interacting);
    }

    void SetDestinatation(Vector3 target, float speed, bool walk, bool run, bool idle, bool interacting) {
        AnimationsManager(walk, run, idle, interacting);
        navMesh.speed = speed;
        navMesh.SetDestination(target);
    }

    [Command(requiresAuthority = false)]
    public void CmdPlayerIsDead(bool isDead) {
        RpcPlayerIsDead(isDead);
    }

    bool ragdollAlreadyInstantiate = false;

    [ClientRpc]
    void RpcPlayerIsDead(bool isDead) {
        if ( !isDead )
            return;

        gameObject.layer = 7;
        foreach ( Transform child in transform ) {
            child.gameObject.layer = 7;
        }

        if ( !ragdollAlreadyInstantiate ) {
            ragdollAlreadyInstantiate = true;
            GameObject ragdoll = Instantiate(playerRagdoll, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            NetworkServer.Spawn(ragdoll);
        }

        gameObject.tag = "Untagged";
        playerModel.SetActive(false);
        flashLight.enabled = false;
    }
}