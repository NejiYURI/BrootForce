using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public bool IsPlayer1;
    public float MoveSpeed = 5f;
    public float DashForce = 15f;
    public float DashTime = 0.5f;
    public float DashCoolDown = 1.5f;

    public float MoveSpeedLimit = 50f;
    public SpriteRenderer spriteRenderer;
    public float StompRange;
    public LayerMask StompCollisionMask;

    public AudioClip StompSound;
    public AudioClip JumpSound;
    public AudioClip FootStep_1;
    public AudioClip FootStep_2;

    private PlayerControl playerInput;

    private Rigidbody2D rg;
    private bool IsStomping;

    private Animator animator;
    [SerializeField]
    private Vector2 Movement;
    private Vector2 DashMovement;
    private bool CanDash;
    private bool CanMove;


    private void Awake()
    {
        playerInput = new PlayerControl();
    }
    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }
    private void Start()
    {
        this.rg = GetComponent<Rigidbody2D>();
        if (IsPlayer1)
        {
            playerInput.Player1.Stomp.performed += _ => StompAction();
            //playerInput.Player1.Dash.performed += _ => DashFunc();
        }
        else
        {
            playerInput.Player2.Stomp.performed += _ => StompAction();
            //playerInput.Player2.Dash.performed += _ => DashFunc();
        }
        CanMove = true;
        animator = this.GetComponent<Animator>();
        IsStomping = false;
        CanDash = true;
        if (GameEventManager.instance != null) GameEventManager.instance.GameOverEvent.AddListener(GameOverFunc);
    }

    private void Update()
    {
        if (IsStomping) return;
        Movement = IsPlayer1 ? playerInput.Player1.Movement.ReadValue<Vector2>() : playerInput.Player2.Movement.ReadValue<Vector2>();
        if (animator != null) animator.SetBool("IsMoving", Movement != Vector2.zero);
        if (spriteRenderer != null && Movement.x != 0) spriteRenderer.flipX = Movement.x > 0;

    }

    private void FixedUpdate()
    {
        if (!IsStomping && CanMove)
            this.rg.AddForce(Movement * MoveSpeed);
        if (CanDash)
            this.rg.velocity = new Vector2(Mathf.Clamp(this.rg.velocity.x, -1 * MoveSpeedLimit, MoveSpeedLimit), Mathf.Clamp(this.rg.velocity.y, -1 * MoveSpeedLimit, MoveSpeedLimit));
    }

    void StompAction()
    {
        if (!IsStomping && CanMove)
        {
            if (GameSoundController.Instance != null) GameSoundController.Instance.PlaySound(JumpSound, 0.1f);
            StartCoroutine(StompCoolDown());
            this.rg.velocity = Vector2.zero;
            if (animator != null) animator.SetTrigger("Stomp");
        }

    }

    public void StompHitGround()
    {
        if (GameSoundController.Instance != null) GameSoundController.Instance.PlaySound(StompSound, 0.1f);
        Collider2D[] hitObj = Physics2D.OverlapCircleAll(this.transform.position, StompRange, StompCollisionMask);
        foreach (var item in hitObj)
        {
            if (item.GetComponent<RootScript>() != null) item.GetComponent<RootScript>().GetStomped();
        }
    }

    public void PlayFootStep_1()
    {
        if (GameSoundController.Instance != null) GameSoundController.Instance.PlaySound(FootStep_1, 0.2f);
    }

    public void PlayFootStep_2()
    {
        if (GameSoundController.Instance != null) GameSoundController.Instance.PlaySound(FootStep_2, 0.2f);
    }

    IEnumerator StompCoolDown()
    {
        IsStomping = true;
        yield return new WaitForSeconds(0.4f);
        IsStomping = false;
    }

    void DashFunc()
    {
        Debug.Log("Dash");
        if (CanDash) StartCoroutine(DashTimer());
    }

    IEnumerator DashTimer()
    {
        CanMove = false;
        this.rg.AddForce(Movement * DashForce);
        yield return new WaitForSeconds(DashTime);
        CanMove = true;
    }

    IEnumerator DashCooldown()
    {
        CanDash = false;
        yield return new WaitForSeconds(DashCoolDown);
        CanDash = true;
    }

    void GameOverFunc()
    {
        IsStomping = true;
        playerInput.Disable();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, StompRange);
    }
}
