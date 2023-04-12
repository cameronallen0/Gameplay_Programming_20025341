using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControls controls;
    Animator animator;
    CharacterController charController;

    public static PlayerController instance;
    public ButtonController buttons;
    public PlayerCamera pCam;
    public EnemyAI enemy;
    public AttributeManager playerAtm;
    public AttributeManager enemyAtm;

    //moving variables
    public Vector2 move;
    public float speed;
    public float walkSpeed = 7.5f;
    public float runSpeed = 15f;

    //jumping variables
    private Vector3 playerVelocity;
    private float jumpHeight = 5.0f;
    private float gravity = -9.81f;

    //other variables
    private bool isRunning;
    private bool groundedPlayer;
    private bool isJumping;
    private bool isFalling;
    public bool isPress;

    void Awake()
    {
        instance = this;

        controls = new PlayerControls();
        charController = GetComponent<CharacterController>();
        animator = gameObject.GetComponentInChildren<Animator>();

        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;

        controls.Player.Run.performed += ctx => isRunning = ctx.ReadValueAsButton();
        controls.Player.Run.canceled += ctx => isRunning = false;

        controls.Player.Jump.performed += ctx => isJumping = ctx.ReadValueAsButton();
        controls.Player.Jump.canceled += ctx => isJumping = false;

        controls.Player.Interact.performed += ctx => isPress = ctx.ReadValueAsButton();
        controls.Player.Interact.canceled += ctx => isPress = false;

        //controls.Player.Attack.performed += ctx => isAttack = ctx.ReadValueAsButton();
        //controls.Player.Attack.canceled += ctx => isAttack = false;
    }
    public void OnEnable()
    {
        controls.Player.Enable();
    }
    public void OnDisable()
    {
        controls.Player.Disable();
    }

    public void FixedUpdate()
    {
        groundedPlayer = charController.isGrounded;

        Vector3 movement = new Vector3(move.x, 0.0f, move.y);
        charController.Move(movement * speed * Time.deltaTime);

        transform.Translate(movement, Space.World);

        if(movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }

        if (move != Vector2.zero && !isRunning)
        {
            Walk();
            Jump();
        }
        else if (move != Vector2.zero && isRunning)
        {
            Run();
            Jump();
        }
        else if (move == Vector2.zero)
        {
            Idle();
            Jump();
        }
        if(isPress)
        {
            if(buttons.canPress)
            {
                buttons.ButtonAnim();
            }
        }
    }
    void Jump()
    { 

        if (groundedPlayer)
        {
            playerVelocity.y = 0.0f;
            animator.SetBool("groundedPlayer", true);
            groundedPlayer = true;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
        
        if (groundedPlayer && isJumping)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -1.0f * gravity);
            animator.SetBool("isJumping", true);
            isJumping = true;
        }

        if((isJumping && playerVelocity.y < 0) || playerVelocity.y < -2)
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        charController.Move(playerVelocity * Time.deltaTime);
    }
    void Idle()
    {
        speed = 0f;
        animator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
    }
    void Walk()
    {
        speed = walkSpeed;
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }
    void Run()
    {
        speed = runSpeed;
        animator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
    }
}
