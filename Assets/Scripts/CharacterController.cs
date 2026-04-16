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

    [Header("OTHER")]
    [Header("Roll")]
    [SerializeField]
    private float rollForce;

    [Header("Camera")]
    [SerializeField]
    private Transform followTarget;
    [SerializeField]
    private float lookSpeed;

    private PlayerInput playerInput;

    private Rigidbody rb;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        Vector2 leftStickInput = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 movement = ((transform.forward * leftStickInput.y) + (transform.right * leftStickInput.x)) * speed;
        //rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z); <------- se necesita un booleano para que esto no se efectue si estas haciendo un roll
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
                if (life - _quantity >= minLife)
                {
                    life -= _quantity;
                }
                else
                {
                    life = minLife;
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
            Debug.Log("W");
        }
    }

    public void HeavyAttack(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Started)
        {
            Debug.Log("L");
        }
    }

    //- - - - - - ROLL - - - - - -
    public void Roll(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Started)
        {
            rb.AddForce(transform.forward * rollForce);
        }
    }
}
