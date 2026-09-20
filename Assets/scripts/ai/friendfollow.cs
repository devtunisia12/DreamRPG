using UnityEngine;

public class friendfollow : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float followDistance=2f;
    private SpriteRenderer spriteRenderer;
    public bool isFollowing = false;
    public Animator animatorpla;
    //Player sfx
    private AudioSource audioSourcesfx;

    public AudioClip jumpon, jumpoff, dashingsfx;

    public GameObject footstepsobj;

    //improving things
    public float jumpForce = 2f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;
    [SerializeField] private Transform GroundCheck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        footstepsobj.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
        animatorpla = GetComponent<Animator>();
        animatorpla.SetBool("run", false);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(
            GroundCheck.position,
            0.2f,
            groundLayer
        );
        animatorpla.SetBool("jump", !isGrounded);

        float direction = Mathf.Sign(player.position.x - transform.position.x);
        bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 5f, 1<< player.gameObject.layer);
        if (isFollowing == true)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance > followDistance)
            {
                animatorpla.SetBool("run", true);

                if (isGrounded)
                {
                    rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
                    if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
                    {
                        footstepsobj.SetActive(true);
                    }
                    else
                    {
                        footstepsobj.SetActive(false);

                    }
                    RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, groundLayer);
                    RaycastHit2D gapAhead = Physics2D.Raycast(transform.position+ new Vector3(direction, 0,0),Vector2.down, 2f, groundLayer);
                    RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, groundLayer);

                    if (!groundInFront.collider && !gapAhead.collider)
                    {
                        footstepsobj.SetActive(false);
                        shouldJump = true;
                    }
                    else if (isPlayerAbove && platformAbove.collider)
                    {
                        footstepsobj.SetActive(false);
                        shouldJump = true;
                    }

                }




                if (direction > 0)
                {
                    spriteRenderer.flipX = false;
                }
                else if (direction < 0)
                {
                    spriteRenderer.flipX = true;

                }
            }
            else
            {
                rb.linearVelocity = new Vector2(
    0f,
    rb.linearVelocity.y
);
                animatorpla.SetBool("run", false);
                footstepsobj.SetActive(false);
            }
        }

    }

    private void FixedUpdate()
    {
        if (isGrounded && shouldJump)
        {
            shouldJump = false;
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 jumpDirection = direction * jumpForce;
            rb.AddForce(new Vector2(jumpDirection.x, jumpForce),ForceMode2D.Impulse);
        }
    }
}
