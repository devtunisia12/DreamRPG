using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class movement : MonoBehaviour
{
    private float horiz;
    public float speed = 8f;
    public float jumpP = 16f;
    private bool isFacingRight = true;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask GroundLayer;

    public Animator animatorpla;
    public static bool CombatMode=false;

    //item system
    public static int healthmedkit = 0;
    public static int powerItem = 0;
    public static int NewDefenseItem = 0;
    public static int NewWeapon = 0;

    //dash system
    private bool canDash = true;
    private bool IsDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCoolDown = 1f;
    [SerializeField] private TrailRenderer tr;
    public static bool isCombat = false;

    //Player sfx
    private AudioSource audioSourcesfx;

    public AudioClip jumpon,jumpoff,dashingsfx;

    public GameObject footstepsobj;
    private bool wasGrounded;
    public static bool endedwar;

    public List<GameObject> playersPos = new List<GameObject>();

    void Start()
    {
        endedwar = false;
        tr.emitting = false;
        footstepsobj.SetActive(false);
        animatorpla = GetComponent<Animator>();
        audioSourcesfx = GetComponent<AudioSource>();
        animatorpla.SetBool("run", false);
        animatorpla.SetBool("dash", false);
        animatorpla.SetBool("jump", false);
        movement.isCombat = false;
        wasGrounded = isGroundedCheck();

    }

    // Update is called once per frame
    void Update()
    {

        bool grounded = isGroundedCheck();

        if (!wasGrounded && grounded)
        {
            audioSourcesfx.PlayOneShot(jumpoff);
        }

        // Save current state for next frame
        wasGrounded = grounded;
        animatorpla.SetBool("jump", !isGroundedCheck());
        if (IsDashing)
        {
            return;
        }
        if (movement.isCombat == false)
        {
            horiz = Input.GetAxisRaw("Horizontal");
        }
        if (horiz != 0)
        {
            if (isGroundedCheck())
            {
                if (movement.isCombat == false)
                {
                    animatorpla.SetBool("run", true);
                    if (pausesystem.isPaused == false)
                    {
                        footstepsobj.SetActive(true);

                    }
                }
                else
                {
                    footstepsobj.SetActive(false);
                    animatorpla.SetBool("run", false);
                }
            }
            else
            {
                footstepsobj.SetActive(false);
            }

        }
        else
        {
            footstepsobj.SetActive(false);
            animatorpla.SetBool("run", false);
        }
        Flip();

        //jumping
        if (Input.GetButtonDown("Jump") && isGroundedCheck() && !movement.isCombat)
        {
            footstepsobj.SetActive(false);

            audioSourcesfx.PlayOneShot(jumpon);

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpP
            );
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * 0.5f
            );
        }


        //Dashing
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && movement.isCombat == false)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (IsDashing)
        {
            return;
        }

        if (movement.isCombat == false)
        {
            rb.linearVelocity = new Vector2(horiz * speed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }

    private bool isGroundedCheck()
    {
        return Physics2D.OverlapCircle(GroundCheck.position,0.2f,GroundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horiz<0f || !isFacingRight && horiz > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        Debug.Log("Dash Start");
        audioSourcesfx.PlayOneShot(dashingsfx);
        animatorpla.SetBool("dash", true);

        canDash = false;
        IsDashing = true;
        tr.emitting = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2((isFacingRight ? 1 : -1) * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        animatorpla.SetBool("dash", false);

        tr.emitting = false;
        rb.gravityScale = originalGravity;
        IsDashing = false;
        canDash = true;
    }


}
