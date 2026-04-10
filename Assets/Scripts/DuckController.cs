using UnityEngine;
using System.Collections;

public class DuckController : MonoBehaviour
{
    public static DuckController Instance;

    [Header("Settings")]
    public float rotationSpeed = 10f;

    [Header("Ground Check")]
    public Transform grondCheckPoint;
    public float groundCheckRadius = 0.5f;

    public LayerMask groundMask;
    public LayerMask waterMask;     // <-- THIS WILL SHOW IN INSPECTOR

    private Vector3 standDirection = Vector3.up;
    Coroutine rotateCoroutine;

    public bool isInAir;

    void Awake()
    {
        Instance = this;
    }

    public void RotateToStand()
    {
        isInAir = true;

        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        rotateCoroutine = StartCoroutine(PostureRotate());
    }

    IEnumerator PostureRotate()
    {
        float threshold = 0.01f;

        while (Vector3.Angle(transform.up, standDirection) > threshold)
        {
            transform.up = Vector3.Lerp(transform.up, standDirection, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        DuckRotated();
    }

    void DuckRotated()
    {
        StartCoroutine(FallDownCor());
    }

    IEnumerator FallDownCor()
    {
        float velocity = 0f;
        float gravity = 9.81f * 6;

        while (true)
        {
            Collider2D hitGround = Physics2D.OverlapCircle(
                grondCheckPoint.position,
                groundCheckRadius,
                groundMask
            );

            Collider2D hitWater = Physics2D.OverlapCircle(
                grondCheckPoint.position,
                groundCheckRadius,
                waterMask
            );

            if (hitGround != null || hitWater != null)
            {
                isInAir = false;
                yield break;
            }

            velocity += gravity * Time.deltaTime;
            transform.position += Vector3.down * velocity * Time.deltaTime;

            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(grondCheckPoint.position, groundCheckRadius);
    }
}
