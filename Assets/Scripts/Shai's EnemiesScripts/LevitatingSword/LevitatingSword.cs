using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class LevitatingSword : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, walking, attack;
    public string currentAnimation;

    [Header("General")]
    [SerializeField] float agroRange = 10; 
    [SerializeField] float attackRange = 3; 
    [SerializeField] float speed = 10;
    [SerializeField] Transform platChecker;
    [SerializeField] Collider blade;

    private GameObject target;
    private Rigidbody _rb;
    private float _distanceFromTarget;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckDistanceFromTargert();
        HandleStateMachine();
        StayOnPlatform();
    }

    


    #region StateMachine
    [HideInInspector] public enum states { Idle, Follow, Attack }
    [Header("StateMachine")]
    public states _currentState;

    void HandleStateMachine()
    {
        switch (_currentState)
        {
            case states.Idle:
                Idle();
                SetAnimation(idle, true, 1f);
                break;

            case states.Follow:
                FollowTarget();
                SetAnimation(walking, true, 1f);
                break;

            case states.Attack:
                Attack();
                break;

            default:
                Debug.Log(gameObject.name + "Entered a state that doesnt exists");
                break;
        }
    }


    [Header("Attack")]
    [SerializeField] float timeBetweenAttacks = 2;
    public int damage;
    public bool isAttacking;
    void Attack()
    {
        if (!isAttacking && IsTargetInAttackRange())
        {
            StartCoroutine(Strike(timeBetweenAttacks));
        }
        if (!IsTargetInAttackRange() && !isAttacking)
        {
            ChangeState(states.Follow);
        }
    }

    void FollowTarget()
    {
        FacePlayer();
        if (!IsNearEndOfPlatform()) //moves towards target
        {
            _rb.velocity = -transform.right * speed * EnemyTimeController.Instance.currentTimeScale;
        }
        if (IsTargetInAttackRange()) //switches to attack state
        {
            ChangeState(states.Attack);
        }
        if (!IsTargetDetected())
        {
            ChangeState(states.Idle);
        }
    }

    void Idle()
    {
        if (IsTargetDetected())// if target in agro range, switch to follow state
        {
            ChangeState(states.Follow);
        }
    }

    public void ChangeState(states state)
    {
        _currentState = state;
    }

    IEnumerator Strike(float seconds)// Stops enemy behavior for given seconds
    {
        isAttacking = true;
        SetAnimation(attack, true, 1f);
        yield return new WaitForSeconds(seconds);
        isAttacking = false;
    }

    #endregion


    #region DistanceCheckers
    /// <summary>
    /// Returns true if targte is in attack range
    /// </summary>
    /// <returns></returns>
    bool IsTargetInAttackRange()
    {
        if (_distanceFromTarget <= attackRange)
        {
            return true;
        }
        return false;
    }

    bool IsTargetDetected()
    {
        if (_distanceFromTarget <= agroRange)
        {
            return true;
        }
        return false;
    }

    void CheckDistanceFromTargert()
    {
        _distanceFromTarget = Vector3.Distance(transform.position, target.transform.position);
    }
    #endregion


    #region MiscMethods
    void FacePlayer() // Rotates the enemy 180 degrees to face the player
    {
        if (target.transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
    }

    void StayOnPlatform() // Stops moving if near the end of a platform
    {
        if (IsNearEndOfPlatform())
        {
            _rb.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// returns true if near an end of a platform
    /// </summary>
    /// <returns></returns>
    bool IsNearEndOfPlatform()
    {
        Collider[] colliders = Physics.OverlapSphere(platChecker.position, 0.5f);
        foreach (var item in colliders)
        {
            if (item.tag == "Platform")
            {
                return false;
            }
        }
        return true;
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        currentAnimation = animation.name;
    }

    #endregion


    #region Gizmos
    [Header("Gizmos")]
    [SerializeField] bool drawAgroRange;
    [SerializeField] bool drawAttackRange;
    [SerializeField] bool drawPlatCheck;

    private void OnDrawGizmosSelected()
    {
        if (drawAgroRange)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, agroRange);
        }

        if (drawAttackRange)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        if (drawPlatCheck)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(platChecker.position, 0.5f);
        }

    }
    #endregion
}
