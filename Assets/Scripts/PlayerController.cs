using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class PlayerController : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField]
    private Weapons mainWeapon;

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
    public float attack;

    [Header("Arcane Stat")]
    [SerializeField]
    private float maxArcane;
    [SerializeField]
    private float minArcane;
    [SerializeField]
    private float arcane;

    [Header("OTHER")]
    [Header("Technical")]
    [SerializeField]
    private Transform hand;
    private bool armed;

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

    [Header("Rayo")]
    [SerializeField]
    private GameObject rayoPrefab;
    [SerializeField]
    private Transform spawnPoint;

    [Header("Dash")]
    [SerializeField]
    private float dashForce;
    private bool isDashing = false;

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

        animator.applyRootMotion = false; //Al iniciar, el animator no debe aplicar Root Motion, para que se pueda mover al estar en Idle
    }

    void Update()
    {
        if (!isRolling && !isDashing)
        {
            Vector2 leftStickInput = playerInput.actions["Move"].ReadValue<Vector2>();
            Vector3 movement = ((transform.forward * leftStickInput.y) + (transform.right * leftStickInput.x)) * speed;
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z); //<-------se necesita un booleano para que esto no se efectue si estas haciendo un roll

            //- - - - - - ANIMACIONES - - - - - -
            animator.SetFloat("X", leftStickInput.x);
            animator.SetFloat("Y", leftStickInput.y);
        }
        if (mainWeapon != null && armed == false)
        {
            GameObject currentWeapon = Instantiate(mainWeapon.prefabMesh, hand.position, hand.rotation);
            currentWeapon.transform.parent = hand;
            armed = true;
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
        // COMO USAR ESTA FUNCI�N?
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
        // COMO USAR ESTA FUNCION?
        //Deberas poner GainStat(XXX,YYY), donde pone 'XXX' deberas poner la stat que quieres cambiar, Ej: "speed", "life" y donde pone 'YYY' poner la cantidad que aumentar
        switch (_stat)
        {
            case "life":
                if (life + _quantity <= maxLife)
                {
                    life += _quantity;
                }
                else
                {
                    life = maxLife;
                }
                break;
            case "speed":
                if (speed + _quantity <= maxSpeed)
                {
                    speed += _quantity;
                }
                else
                {
                    speed = maxSpeed;
                }
                break;
        }
    }

    public void Dash(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Started && !isDashing)
        {
            isDashing = true;

            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);

            animator.SetBool("isDashing", true);
            Invoke(nameof(FinDash), 0.3f);
        }
    }

    public void FinDash()
    {
        isDashing = false;
        animator.SetBool("isDashing", false);
    }

    //- - - - - - COMANDOS DE ATAQUES - - - - - -
    public void WeakAttack(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Started)
        {
            attackPhase = 1;
            hitbox.SetActive(true);
            Debug.Log("W");

            //- - - - - - ANIMACION - - - - - -
            animator.applyRootMotion = true;
            animator.SetTrigger("LightAttack");
        }
    }

    public void HeavyAttack(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Started)
        {
            attackPhase = 2;
            hitbox.SetActive(true);
            Debug.Log("L");

            //- - - - - - ANIMACION - - - - - -
            animator.applyRootMotion = true;
            animator.SetTrigger("HeavyAttack");
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Detecta enemigo " + attackPhase);
            float weaponDamage = 1;
            if (mainWeapon != null)
            {
                weaponDamage = mainWeapon.damage;
            }

            if (attackPhase == 1)
            {
                enemy.gameObject.GetComponent<EnemyController>().TakeDamage( weaponDamage * 2 + attack * 1.3f ); //(formula de) Ataque 'flojo'
            }
            else if (attackPhase == 2)
            {
                enemy.gameObject.GetComponent<EnemyController>().TakeDamage( (weaponDamage * 2 + attack * 1.3f) * 1.3f ); //(formula de) Ataque 'fuerte'
            }
        }
    }

    public void RayoAttack(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Started)
        {
            GameObject rayo = Instantiate(rayoPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    //- - - - - - ROLL - - - - - -
    public void Roll(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Started)
        {
            isRolling = true;
            rb.AddForce(transform.forward * rollForce);

            animator.SetTrigger("Roll");
        }
    }

    public void MoveAgainEvent()
    {
        isRolling = false;

        animator.applyRootMotion = false; //Desactiva el Root Motion al terminar las animaciones de ataque
    }

    public void TakePlayerDamage(float _daamage)
    {

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
