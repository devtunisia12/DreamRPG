using UnityEngine;
using UnityEngine.UI;

public class ScrollingUISprite : MonoBehaviour
{
    public Transform target1;
    public Transform target2;
    public float speed = 2f;

    private Transform currentTarget;

    void Start()
    {
        currentTarget = target1;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, currentTarget.position) < 0.01f)
        {
            if (currentTarget == target1)
            {
                currentTarget = target2;
            }
            else
            {
                currentTarget = target1;
            }
        }
    }
}