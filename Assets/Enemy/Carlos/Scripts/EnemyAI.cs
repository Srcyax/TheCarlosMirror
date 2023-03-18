using Mirror;
using UnityEngine;
using UnityEngine.AI;

public enum AIstate
{
    walking, following, lookingforTarget, comeToPoint
};

[RequireComponent ( typeof ( NavMeshAgent ) )]
public class EnemyAI : NetworkBehaviour
{
    Vector3 lastPositionKnown = Vector3.zero;
    Animator animator => GetComponent<Animator> ();
    NavMeshAgent navMesh => GetComponent<NavMeshAgent> ();
    AudioSource audioSource => GetComponent<AudioSource> ();

    [Header("Carlos setup")]
    [Tooltip("Get Carlos standard settings")]
    [SerializeField] private CarlosSetup carlosSetup;

    [Space(10)]
    [Header("Raycast Spots")]
    [Tooltip("Get Carlos head raycast")]
    [SerializeField] private EnemyMain headSpot;

    [Space(10)]
    [Header("Carlos music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip followingMusic;
    [SerializeField] private AudioClip backgroundMusic;

    [Space(10)]
    [Header("Player components")]
    [SerializeField] private GameObject jumpScare;

    [SerializeField] public AIstate stateAI;
    private GameObject[] wayPoints;

    [SyncVar] float searchTime = 0;
    [SyncVar] float wayPointTime = 0;
    [SyncVar] float wayPointDistance;
    [SyncVar] float coolDownCruzzEffect;
    [SyncVar] int currentWayPoint;
    [SyncVar] public bool cruzEffect = false;
    [SyncVar] Transform target = null;

    PlayersAlreadyJoined server;

    void Start()
    {
        wayPoints = GameObject.FindGameObjectsWithTag ( "WayPoint" );

        Physics.IgnoreLayerCollision ( 0, 11 );
        currentWayPoint = Random.Range ( 0, wayPoints.Length );
        stateAI = AIstate.walking;

        server = GameObject.FindGameObjectWithTag ( "NetworkManager" ).GetComponent<PlayersAlreadyJoined> ();
    }

    void Update()
    {
        if ( !server.PlayersAlreadyJoinedInServer () )
            return;

        CmdMainCode ();
        CmdCruzEffect ();
    }

    [Command ( requiresAuthority = false )]
    void CmdMainCode()
    {
        wayPointDistance = Vector3.Distance ( wayPoints[currentWayPoint].transform.position, transform.position );
        if ( headSpot )
        {
            switch ( stateAI )
            {
                case AIstate.walking:
                    Walking ();
                    break;
                case AIstate.following:
                    Following ();
                    break;
                case AIstate.lookingforTarget:
                    LookingForTarget ();
                    break;
                case AIstate.comeToPoint:
                    ComeToPoint ();
                    break;
            }
        }
    }

    void Walking()
    {
        RpcMusicBackground ();

        transform.rotation = Quaternion.Slerp ( transform.rotation, Quaternion.LookRotation ( wayPoints[currentWayPoint].transform.position - transform.position ), Time.deltaTime );
        if ( wayPointDistance < 2 )
        {
            wayPointTime += Time.deltaTime;
            SetDestinatation ( wayPoints[currentWayPoint].transform.position, 0, false, false, true, false, false );
            if ( wayPointTime > 4 )
            {
                currentWayPoint = Random.Range ( 0, wayPoints.Length );

                if ( Vector3.Distance ( wayPoints[currentWayPoint].transform.position, transform.position ) < 40 )
                    currentWayPoint = Random.Range ( 0, wayPoints.Length );

                stateAI = AIstate.walking;
                wayPointTime = 0;
            }
        }
        else
        {
            SetDestinatation ( wayPoints[currentWayPoint].transform.position, carlosSetup.minVelocity, true, false, false, false, false );
        }

        FollowPlayer ();
    }

    void Following()
    {
        if ( cruzEffect )
        {
            SetDestinatation ( target.position, carlosSetup.cruzEffectVelocity, false, false, false, false, true );
        }
        else
        {
            SetDestinatation ( target.position, carlosSetup.maxVelocity, false, true, false, false, false );
        }

        RpcMusicFollow ();
        transform.rotation = Quaternion.Slerp ( transform.rotation, Quaternion.LookRotation ( target.position - transform.position ), Time.deltaTime );

        Kill ();

        if ( !headSpot.visibleTarget.Contains ( target ) )
        {
            lastPositionKnown = target.position;
            stateAI = AIstate.lookingforTarget;
        }
    }

    void LookingForTarget()
    {
        if ( cruzEffect )
        {
            SetDestinatation ( lastPositionKnown, carlosSetup.cruzEffectVelocity, false, false, false, false, true );
        }
        else
        {
            if ( Vector3.Distance ( transform.position, lastPositionKnown ) > 3 )
            {
                SetDestinatation ( lastPositionKnown, carlosSetup.maxVelocity, false, true, false, false, false );
            }

            if ( Vector3.Distance ( transform.position, lastPositionKnown ) < 3 )
            {
                AnimationsManager ( false, false, true, false, false );
                RpcMusicBackground ();
                searchTime += Time.deltaTime;
            }

            if ( searchTime > 5 )
            {
                stateAI = AIstate.walking;
                searchTime = 0;
            }
        }

        FollowPlayer ();
    }

    public void ComeToPoint()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int sort = Random.Range(0, players.Length);

        SetDestinatation ( players[sort].transform.position, carlosSetup.maxVelocity, false, true, false, false, false );

        RpcMusicFollow ();

        FollowPlayer ();
    }


    [ClientRpc]
    void RpcMusicBackground()
    {
        audioSource.clip = backgroundMusic;
        if ( !audioSource.isPlaying )
        {
            audioSource.Play ();
        }

    }

    [ClientRpc]
    void RpcMusicFollow()
    {
        audioSource.clip = followingMusic;
        if ( !audioSource.isPlaying )
        {
            audioSource.Play ();
        }
    }

    void AnimationsManager( bool a, bool b, bool c, bool d, bool e )
    {
        animator.SetBool ( "walk", a );
        animator.SetBool ( "run", b );
        animator.SetBool ( "idle", c );
        animator.SetBool ( "attack", d );
        animator.SetBool ( "injured", e );
    }

    void Kill()
    {
        if ( Vector3.Distance ( transform.position, target.position ) < 4 )
        {
            headSpot.ClearTargets();
            SetDestinatation ( target.position, 0, false, false, false, true, false );
            coolDownCruzzEffect = 0;
            cruzEffect = false;
            if ( target.gameObject.GetComponent<PlayerController> () )
            {
                target.gameObject.GetComponent<PlayerController> ().isDead = true;
                int index = gameObject.name == "Carlos" ? 0 : 1;
                target.gameObject.GetComponent<PlayerController> ().PLayerDeadJumpScare ( index );
            }
            else
            {
                target.gameObject.GetComponent<NpcAI>().IsDead = true;
                target.gameObject.tag = "Untagged";
                target.gameObject.layer = 7;
            }

            stateAI = AIstate.lookingforTarget;
        }
    }

    void FollowPlayer()
    {
        if ( !( headSpot.visibleTarget.Count > 0 ) )
            return;

        if ( headSpot.visibleTarget.Count > 0 )
        {
            int index = 0;
            float sanity = 999;
            for (int i = 0; i < headSpot.visibleTarget.Count; i++ )
            {
                if ( !( headSpot.visibleTarget[i].gameObject.GetComponent<PlayerSanity>() ) )
                {
                    index = 0;
                    continue;
                }

                if ( headSpot.visibleTarget[i].gameObject.GetComponent<PlayerSanity>().sanity < sanity )
                {
                    sanity = headSpot.visibleTarget[i].gameObject.GetComponent<PlayerSanity>().sanity;
                    index = i;
                }   
            }

            target = headSpot.visibleTarget[index];
            lastPositionKnown = target.position;
            stateAI = AIstate.following;
        }
    }

    void SetDestinatation( Vector3 target, float speed, bool walk, bool run, bool idle, bool attack, bool injured )
    {
        AnimationsManager ( walk, run, idle, attack, injured );
        navMesh.speed = speed;
        navMesh.SetDestination ( target );
    }

    [Command ( requiresAuthority = false )]
    void CmdCruzEffect()
    {
        if ( !cruzEffect )
            return;

        coolDownCruzzEffect += Time.deltaTime;
        if ( coolDownCruzzEffect > 5 )
        {
            coolDownCruzzEffect = 0;
            cruzEffect = false;
        }
    }
}