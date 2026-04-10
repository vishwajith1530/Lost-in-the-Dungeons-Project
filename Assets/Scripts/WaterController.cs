using UnityEngine;

public class WaterController : MonoBehaviour
{
    public float changeAmount = 0.5f;
    public float minScaleY = 1f;
    public float maxScaleY = 5f;

    public Transform duck;
    public DuckController duckController;

    void Update()
    {
        float currentY = transform.localScale.y;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            float newScaleY = Mathf.Min(currentY + changeAmount, maxScaleY);
            AdjustWaterHeight(newScaleY, true);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            float newScaleY = Mathf.Max(currentY - changeAmount, minScaleY);
            AdjustWaterHeight(newScaleY, false);
        }
    }

    void AdjustWaterHeight(float newScaleY, bool rising)
    {
        Vector3 upDir = -Physics.gravity.normalized;

        float oldScaleY = transform.localScale.y;
        float delta = newScaleY - oldScaleY;

        transform.localScale = new Vector3(
            transform.localScale.x,
            newScaleY,
            transform.localScale.z
        );

        transform.position += upDir * (delta * 0.5f);

        HandleDuckFloating(upDir, rising);
    }

    void HandleDuckFloating(Vector3 upDir, bool rising)
    {
        float halfHeight = transform.localScale.y * 0.5f;
        Vector3 waterTop = transform.position + upDir * halfHeight;

        float duckHalfHeight = 0.5f;
        Vector3 duckBottom = duck.position - upDir * duckHalfHeight;

        float waterLevel = Vector3.Dot(waterTop, upDir);
        float duckLevel = Vector3.Dot(duckBottom, upDir);

        if (rising && duckLevel < waterLevel)
        {
            duck.position += upDir * (waterLevel - duckLevel);
        }

        if (!rising && duckLevel > waterLevel)
        {
            duckController.RotateToStand();
        }
    }

    public void AlignToGravity()
    {
        Vector3 downDir = Physics.gravity.normalized;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, downDir, out hit, 100f))
        {
            float halfHeight = transform.localScale.y * 0.5f;
            transform.position = hit.point - downDir * halfHeight;
        }

        // Keep surface perpendicular to gravity
        transform.rotation = Quaternion.FromToRotation(transform.up, -downDir) * transform.rotation;
    }
}
