using UnityEngine;

public class RotateAroundCenter : MonoBehaviour
{
    public float rotationSpeed = 30f;
    public Vector3 rotationAxis = Vector3.up;
    public bool rotateAboutCenter = true;

    private Vector3 center;
    private Renderer[] renderers;
    private Bounds bounds;

    void Start()
    {
        if (rotateAboutCenter)
            CalculateCenter();
    }

    void Update()
    {
        // Recalculate center in Update if the object's visual bounds might change dynamically
        // CalculateCenter();

        // Calculate the direction from the center to the object's current position
        Vector3 offset = transform.position - center;

        // Rotate this offset vector around the specified axis
        offset = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, rotationAxis) * offset;

        // Update the object's position to be the rotated offset from the center
        if (rotateAboutCenter)
            transform.position = center + offset;

        // Rotate the object itself to maintain its orientation (optional, depends on desired effect)
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.World);
    }

    void CalculateCenter()
    {
        bounds = new Bounds(transform.position, Vector3.zero);
        renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }
        }
        center = bounds.center;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (rotateAboutCenter)
        {
            CalculateCenter();
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(center, 0.1f);
        }
    }
}