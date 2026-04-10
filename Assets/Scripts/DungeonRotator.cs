using UnityEngine;
using System.Collections;

public class DungeonRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 200f;

    private bool isRotating = false;
    private Quaternion targetRotation;

    private DuckController duck;

    void Start()
    {
        duck = DuckController.Instance;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        if (isRotating || duck.isInAir) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(RotateDungeon(90f));
            DuckController.Instance.RotateToStand();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(RotateDungeon(-90f));
            DuckController.Instance.RotateToStand();
        }
    }

    private IEnumerator RotateDungeon(float angle)
    {
        isRotating = true;

        Quaternion startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + angle);

        float rotationDuration = 90f / rotationSpeed;
        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / rotationDuration);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        isRotating = false;
    }
}
