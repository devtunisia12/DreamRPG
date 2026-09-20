using UnityEngine;

public class aifollowPlatformer : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public float speed = 2f;

    private bool goingToB = true;

    void FixedUpdate()
    {
        Transform target = goingToB ? pointB : pointA;

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.fixedDeltaTime
        );

        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            goingToB = !goingToB;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}