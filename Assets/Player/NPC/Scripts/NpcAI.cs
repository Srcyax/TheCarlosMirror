using Mirror;
using UnityEngine;
using UnityEngine.AI;

public enum NPCAIstate
{
    walking
};

[RequireComponent ( typeof ( NavMeshAgent ) )]
public class NpcAI : NetworkBehaviour
{
    [SerializeField] Animator animator;
    NavMeshAgent navMesh => GetComponent<NavMeshAgent> ();

    [Header("Carlos setup")]
    [Tooltip("Get Carlos standard settings")]
    [SerializeField] private CarlosSetup carlosSetup;


    [SerializeField] public NPCAIstate stateAI;
    private GameObject[] wayPoints;

    [SyncVar] float wayPointTime = 0;
    [SyncVar] float wayPointDistance;
    [SyncVar] int currentWayPoint;

    PlayersAlreadyJoined server;

    void Start()
    {
        wayPoints = GameObject.FindGameObjectsWithTag ( "WayPoint" );

        Physics.IgnoreLayerCollision ( 0, 11 );
        currentWayPoint = Random.Range ( 0, wayPoints.Length );
        stateAI = NPCAIstate.walking;

        server = GameObject.FindGameObjectWithTag ( "NetworkManager" ).GetComponent<PlayersAlreadyJoined> ();
    }

    void Update()
    {
        if ( !server.PlayersAlreadyJoinedInServer () )
        {
            AnimationsManager ( false, false, true );
            return;
        }

        CmdMainCode ();
    }

    [Command ( requiresAuthority = false )]
    void CmdMainCode()
    {
        wayPointDistance = Vector3.Distance ( wayPoints[currentWayPoint].transform.position, transform.position );
        switch ( stateAI )
        {
            case NPCAIstate.walking:
                Walking ();
                break;
        }
    }

    void Walking()
    {
        transform.rotation = Quaternion.Slerp ( transform.rotation, Quaternion.LookRotation ( wayPoints[currentWayPoint].transform.position - transform.position ), Time.deltaTime );
        if ( wayPointDistance < 2 )
        {
            wayPointTime += Time.deltaTime;
            SetDestinatation ( wayPoints[currentWayPoint].transform.position, 0, false, false, true );
            if ( wayPointTime > 4 )
            {
                currentWayPoint = Random.Range ( 0, wayPoints.Length );

                if ( Vector3.Distance ( wayPoints[currentWayPoint].transform.position, transform.position ) < 40 )
                    currentWayPoint = Random.Range ( 0, wayPoints.Length );

                stateAI = NPCAIstate.walking;
                wayPointTime = 0;
            }
        }
        else
        {
            SetDestinatation ( wayPoints[currentWayPoint].transform.position, carlosSetup.minVelocity, true, false, false );
        }
    }

    void AnimationsManager( bool a, bool b, bool c )
    {
        animator.SetBool ( "walk", a );
        animator.SetBool ( "run", b );
        animator.SetBool ( "idle", c );
    }

    void SetDestinatation( Vector3 target, float speed, bool walk, bool run, bool idle )
    {
        AnimationsManager ( walk, run, idle );
        navMesh.speed = speed;
        navMesh.SetDestination ( target );
    }
}