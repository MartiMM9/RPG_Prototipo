using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour
{
    private Animator animator;
    private UnityEngine.AI.NavMeshAgent agent;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float life;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    public float damage;
    [SerializeField]
    private float attackCooldown;
    private Transform targetPlayer;
    private float attackTimer;
    private bool Muerto;
    private bool playerDetected;
    [SerializeField]
    private GameObject sword;

    private CharacterController player;
    private LevelManager levelManager;
 

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;

        player = FindObjectOfType<CharacterController>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void Update()
    {
        if (Muerto == true)
        {
            return;
        }

        /*if (targetPlayer.GetComponent<CharacterController>().isDead == true)
        {
            agent.isStopped = true;
            animator.SetBool("Iddle", true);
            return;
        }*/

        if (playerDetected == true)
        {
            if (targetPlayer == null)
            {
                targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
                if (targetPlayer == null)
                {
                    return;
                }
            }

            agent.SetDestination(targetPlayer.position);
            agent.speed = speed;

            float distance = Vector3.Distance(transform.position, targetPlayer.position);

            if (distance <= attackRange)
            {
                agent.isStopped = true;
                Attack();
            }
            else
            {
                animator.SetBool("Attack", false);
                animator.SetBool("Run", true);
                agent.isStopped = false;
            }

            //cooldown

            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if ((collision.gameObject.tag == "Player"))
        {
            animator.SetTrigger("Detect");
            sword.SetActive(true);
            //Invoke("StartMoving", animator.GetCurrentAnimatorStateInfo(0).length);     
        }
    }

    public void StartMoving()
    {
        playerDetected = true;
        animator.SetBool("Run", true);
    }

    private void Attack()
    {
        if (attackTimer > 0)
        {
            return;
        }
        animator.SetBool("Attack", true);
        animator.SetBool("Run", false);
        attackTimer = attackCooldown;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<CharacterController>().TakePlayerDamage(damage);
        }
    }
    public void TakeDamage(float _damage)
    {
        Debug.Log("Recibe daño");
        animator.SetBool("Attack", false);
        animator.SetTrigger("Back");

        life -= _damage;

        if (life <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        agent.Stop();
        agent.isStopped = true;
        Muerto = true;
        animator.SetTrigger("Death");
 
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 2f);
    }
}
