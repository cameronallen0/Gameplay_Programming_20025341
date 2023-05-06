using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public ButtonController buttons;
    public PlayerCamera pCam;
    //public AttributeManager playerAtm;
    public GameObject playerCamera;
    public Transform respawnPoint;

    PlayerControls controls;
    CharacterController charController;
    Animator animator;
    PlayerAttributes stats;

    public float playerSpeed;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float gravity = -9.81f;

    public bool attacking;
    public bool canAttack = false;
    public bool isAttacking;
    private float lastAttack = 0;

    [SerializeField]
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private bool isGrounded;
    [SerializeField] private float groundCheckDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    private bool isJumping;

    Vector2 move;
    Vector3 playerVelocity;
    bool runPressed;
    bool jumpPressed;
    bool buttonPressed;
    public bool canDoubleJump;


    public float GetSpeed()
    {
        return playerSpeed;
    }
    public void SetSpeed(float speed)
    {
        playerSpeed = speed;
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        charController = GetComponent<CharacterController>();
        stats = GetComponent<PlayerAttributes>();
    }
    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
        controls.Player.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
        controls.Player.Run.canceled += ctx => runPressed = false;
        controls.Player.Jump.performed += ctx => jumpPressed = ctx.ReadValueAsButton();
        controls.Player.Jump.canceled += ctx => jumpPressed = false;
        controls.Player.Interact.performed += ctx => buttonPressed = ctx.ReadValueAsButton();
        controls.Player.Interact.canceled += ctx => buttonPressed = false;
    }

    public void OnEnable()
    {
        controls.Player.Enable();
    }
    public void OnDisable()
    {
        controls.Player.Disable();
    }
    public void OnAttack()
    {
        animator.SetTrigger("Attacking");
        attacking = true;
    }
    void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        if (isGrounded)
        {
            animator.SetBool("isGrounded", true);
            isGrounded = true;
            animator.SetBool("isJumping", false);
            isJumping = false;
            animator.SetBool("isFalling", false);
        }
        else
        {
            animator.SetBool("isGrounded", false);
            isGrounded = false;
        }

        if ((isJumping && playerVelocity.y < 0) || playerVelocity.y < -2)
        {
            animator.SetBool("isFalling", true);
        }

        Vector3 movement = new Vector3(move.x, 0.0f, move.y).normalized;
        movement = playerCamera.transform.forward * movement.z + playerCamera.transform.right * movement.x;
        movement.y = 0f;
        movement = movement.normalized;

        if (movement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            charController.Move(movement * playerSpeed * Time.deltaTime);
        }
        if(stats.health == 0)
        {
            transform.position = respawnPoint.position;
            stats.health = stats.maxHealth;
        }

        if (isGrounded)
        {
            if (move != Vector2.zero && !runPressed && playerSpeed != 16)
            {
                Walk();
            }
            else if (move != Vector2.zero && runPressed && playerSpeed != 16)
            {
                Run();
            }
            else if (move != Vector2.zero && playerSpeed == 16)
            {
                animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
            }
            else if (move == Vector2.zero)
            {
                Idle();
            }
            move *= playerSpeed;
            if (jumpPressed)
            {
                Jump();
            }
            if(attacking && !runPressed && canAttack)
            {
                if(!isAttacking)
                {
                    isAttacking = true;
                }
            }
            if(buttonPressed)
            {
                if(buttons.canPress)
                {
                    buttons.WallButtonAnim();
                }
            }

        }
        playerVelocity.y += gravity * Time.deltaTime;
        charController.Move(playerVelocity * Time.deltaTime);
        attacking = false;
    }
    private void Idle()
    {
        animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
        playerSpeed = 0;
    }
    private void Walk()
    {
        playerSpeed = walkSpeed;
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }
    private void Run()
    {
        playerSpeed = runSpeed;
        animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Enemy")
        {
            canAttack = true;
            if(isAttacking)
            {
                if(Time.time >= lastAttack + stats.attackSpeed)
                {
                    lastAttack = Time.time;
                    CharacterAttributes enemyStats = other.GetComponent<CharacterAttributes>();
                    isAttacking = false;
                    Attack(enemyStats);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "DeathBarrier")
        {
            stats.health = 0;
        }
    }
    private void Attack(CharacterAttributes statsToDamage)
    {
        stats.DoDamage(statsToDamage);
    }
    private void Jump()
    {
        playerVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        animator.SetBool("isJumping", true);
        isJumping = true;
    }
}
