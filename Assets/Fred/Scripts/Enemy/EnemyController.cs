using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private LosCombat _combat;
    
    [Header("AI Settings")] 
    [SerializeField] private State state;
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private float detectionRadius = 1f;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float distanceToPlayer; 
    [SerializeField] private bool debug = true;
    private enum State
    {
        Patrol,
        ChasingPlayer,
        Attacking,
        Dead
    }
    [Header("Health System")] 
    [SerializeField] private float currentHealth;
    private const float MaxHealth = 100f;
    public bool isDead;
    [Header("Movement")]
    [SerializeField] private Vector2 originalPosition;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float patrolDistance;
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool movingRight;
    [Header("Combat")] 
    [SerializeField] private Transform target;
    [SerializeField] private Transform castPoint;
    [SerializeField] private float castPointDistance;
    private static readonly int IsAttacking = Animator.StringToHash("Attack");
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    AudioSource Asource;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _combat = GetComponentInChildren<LosCombat>();
        currentHealth = MaxHealth;
        originalPosition = transform.localPosition;
        state = State.Patrol;
        Asource = GetComponent<AudioSource>();
    }
    
    private void FixedUpdate()
    {
        
        FlipCharacter();
        if (target != null)
        {
            distanceToPlayer = Vector2.Distance(transform.position, target.position);
            if (Math.Abs(target.position.x - transform.position.x) < 0.2f && target.position.y > transform.position.y)
            {
                canMove = false;
                
            }
            else canMove = true;

            if (currentHealth <= 0)
            {
                canMove = true;
                state = State.Dead;
            }
            if (!canMove) return;
        }
        
        switch (state)
        {
            case State.Patrol:
            {
                if (movingRight & transform.localPosition.x > originalPosition.x + patrolDistance) movingRight = false;
                else if (!movingRight & transform.localPosition.x < originalPosition.x - patrolDistance) movingRight = true;
                HandleDetection();
                HandlePatrol();
                break;
            }
            case State.ChasingPlayer:
            {
                if (movingRight & transform.localPosition.x > target?.position.x) movingRight = false;
                else if (!movingRight & transform.localPosition.x < target?.position.x) movingRight = true;
                HandlePlayerChase();
                break;
            }
            case State.Attacking:
            {
                if (distanceToPlayer > attackRange) state = State.Patrol;
                canMove = false;
                _animator.SetTrigger("Attack");
                break;
            }
            case State.Dead:
            {
                canMove = false;
                StartCoroutine(Death());
                break;
            }
            default:
                state = State.Patrol;
                break;
        }
  
        // DEBUG ONLY
        
        if (debug) _combat.LineOfSight();
        
    }
    
    private void FlipCharacter()
    {
       // _animator.SetBool(IsWalking, true);
        switch (movingRight)
        {
            case true:
                _spriteRenderer.flipX = true;
                castPoint.localPosition = new Vector3(castPointDistance,castPoint.localPosition.y,castPoint.localPosition.z);
                break;
            case false:
                _spriteRenderer.flipX = false;
                castPoint.localPosition = new Vector3(-castPointDistance,castPoint.localPosition.y,-castPoint.localPosition.z);
                break;
        }
    }
    
    private void HandleDetection()
    {
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectionLayer);

        foreach (var result in results)
        {
            var hit = result.transform.GetComponent<PlayerScript>();
            if (hit != null)
            {
                target = hit.gameObject.transform;
                state = State.ChasingPlayer;
            }
        }
        
    } 
    
    private void HandlePatrol()
    {
        if (canMove)
        {
            _rb.velocity = movingRight 
                ? _rb.velocity = new Vector3(movementSpeed, 0.0f, 0.0f) 
                : _rb.velocity = new Vector3(-movementSpeed, 0.0f, 0.0f);
        }
    }

    private void HandlePlayerChase()
    {
        if (distanceToPlayer > chaseRange)
        {
            state = State.Patrol;
        }
        if (distanceToPlayer <= attackRange) state = State.Attacking;
        if (canMove)
        {
            if (distanceToPlayer < chaseRange && distanceToPlayer > attackRange)
            {
                _rb.velocity = movingRight 
                    ? _rb.velocity = new Vector3(movementSpeed, 0.0f, 0.0f) 
                    : _rb.velocity = new Vector3(-movementSpeed, 0.0f, 0.0f);
            }
        }
    }
  
    public void UpdateHealth(float value)
    {
        currentHealth += value;
        if (currentHealth > 100f) currentHealth = MaxHealth;
        if (currentHealth <= 0) state = State.Dead;
    }
    
    public IEnumerator Attack()
    {
        Asource.Play();
        if (distanceToPlayer <= 0.2f) state = State.Patrol;
        if (_combat.LineOfSight())
        {
            yield return new WaitForSeconds(0.1f);
            FindObjectOfType<PlayerScript>()?.TakeDamage(100);
        }
        yield return new WaitForSeconds(0.6f);
        canMove = true;
    }
    
    private IEnumerator Death()
    {
        if (!isDead)
        {
            //_animator.SetBool(IsDead, true);
            yield return new WaitForSeconds(1.35f);
            isDead = true;
            Destroy(gameObject);
        }
    }
    // DEBUG ONLY
    private void OnDrawGizmosSelected()
    {
        if (debug)
        {
            Gizmos.color = Color.red; 
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
