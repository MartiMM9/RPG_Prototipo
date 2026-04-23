using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class CharacterController : MonoBehaviour
{
    [Header("STATS")]
    [Header("Life Stat")]
    [SerializeField]
    private float life;
    [SerializeField]
    private float maxLife;
    [SerializeField]
    private float minLife;

    [Header("Speed Stat")]
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float minSpeed;
    [SerializeField]
    private float speed;

    [Header("Attack Stat")]
    [SerializeField]
    private float maxAttack;
    [SerializeField]
    private float minAttack;
    [SerializeField]
    private float attack;
    public float smallAttack;
    public float heavyAttack;

    [Header("Arcane Stat")]
    [SerializeField]
    private float maxArcane;
    [SerializeField]
    private float minArcane;
    [SerializeField]
    private float arcane;

    [Header("OTHER")]
    [Header("Roll")]
    [SerializeField]
    private float rollForce;
    [SerializeField]
    private GameObject hitbox;

    public int attackPhase;

    [Header("Camera")]
    [SerializeField]
    private Transform followTarget;
    [SerializeField]
    private float lookSpeed;

    private PlayerInput playerInput;
    private bool isRolling = false;
    private Rigidbody rb;
    public GameObject enemy;
    private Animator animator;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isRolling)
        {
            Vector2 leftStickInput = playerInput.actions["Move"].ReadValue<Vector2>();
            Vector3 movement = ((transform.forward * leftStickInput.y) + (transform.right * leftStickInput.x)) * speed;
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z); //<-------se necesita un booleano para que esto no se efectue si estas haciendo un roll
        }
    }

    private void LateUpdate()
    {
        Vector2 lookInput = playerInput.actions["Look"].ReadValue<Vector2>();
        followTarget.localEulerAngles += new Vector3(lookInput.y * lookSpeed * Time.deltaTime, 0, 0);
        transform.eulerAngles += new Vector3(0, lookInput.x * lookSpeed * Time.deltaTime, 0); // <----------- ESTO SE PONE CUANDO EL JUGADOR SE MUEVA, PARA QUE LA CAMARA NO LO GIRE
    }

    //- - - - - - COMANDOS DE STATS - - - - - -
    public void TakeStat(string _stat, float _quantity)
    {
        // COMO USAR ESTA FUNCIÓN?
        //Deberas poner TakeStat(XXX,YYY), donde pone 'XXX' deberas poner la stat que quieres cambiar, Ej: "speed", "life" y donde pone 'YYY' poner la cantidad que quitar
        switch (_stat)
        {
            case "life":
                if (!isRolling)
                {
                    if (life - _quantity >= minLife)
                    {
                        life -= _quantity;
                    }
                    else
                    {
                        life = minLife;
                    }
                }
                break;
            case "speed":
                if (speed - _quantity >= minSpeed)
                {
                    speed -= _quantity;
                }
                else
                {
                    speed = minSpeed;
                }
                break;
        }
    }

    public void GainStat(string _stat, float _quantity)
    {
        // COMO USAR ESTA FUNCIÓN?
        //Deberas poner GainStat(XXX,YYY), donde pone 'XXX' deberas poner la stat que quieres cambiar, Ej: "speed", "life" y donde pone 'YYY' poner la cantidad que aumentar
        switch (_stat)
        {
            case "life":
                if (life + _quantity <= maxLife)
                {
                    life += maxLife;
                }
                else
                {
                    life = maxLife;
                }
                break;
            case "speed":
                if (speed + _quantity <= maxSpeed)
                {
                    speed += maxSpeed;
                }
                else
                {
                    speed = maxSpeed;
                }
                break;
        }
    }

    //- - - - - - COMANDOS DE ATAQUES - - - - - -
    public void WeakAttack(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Started)
        {
            attackPhase = 1;
            hitbox.SetActive(true);
            Debug.Log("W");
        }
    }

    public void HeavyAttack(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Started)
        {
            attackPhase = 2;
            hitbox.SetActive(true);
            Debug.Log("L");
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Detecta enemigo " + attackPhase);

            if (attackPhase == 1)
            {
                enemy.gameObject.GetComponent<EnemyController>().TakeDamage(smallAttack);
            }
            else if (attackPhase == 2)
            {
                enemy.gameObject.GetComponent<EnemyController>().TakeDamage(heavyAttack);
            }
        }
    }

    //- - - - - - ROLL - - - - - -
    public void Roll(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Started)
        {
            isRolling = true; 
            rb.AddForce(transform.forward * rollForce);
        }
    }

    public void MoveAgainEvent()
    {
        isRolling = false;
    }

    public void TakePlayerDamage(float _daamage)
    {
        Debug.Log("Recibe daño");

        life -= _daamage;

        if (life <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }
    private void Die()
    {
        animator.SetTrigger("Death");
        GetComponent<Collider>().enabled = false;
    }
}
