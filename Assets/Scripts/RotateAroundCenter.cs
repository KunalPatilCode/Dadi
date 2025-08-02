using UnityEngine;

public class RotateAroundCenter : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 30f;

    [Tooltip("The axis of rotation. Interpreted as a world-space or local-space vector based on the 'Use Relative Axis' setting.")]
    public Vector3 rotationAxis = Vector3.up;

    [Tooltip("If checked, the 'Rotation Axis' will be treated as relative to the object's local orientation.")]
    public bool useRelativeAxis = false;

    [Tooltip("If checked, the object will orbit around its calculated center. If unchecked, it will only rotate on its own axis.")]
    public bool rotateAboutCenter = true;

    private Vector3 center;
    private Renderer[] renderers;
    private Bounds bounds;

    void Start()
    {
        if (rotateAboutCenter)
        {
            CalculateCenter();
        }
        else
        {
            // If not rotating around a center, the center is the object's own pivot.
            center = transform.position;
        }
    }

    void Update()
    {
        // Determine the effective rotation axis in world space for this frame.
        // If useRelativeAxis is true, transform the local axis to a world direction.
        // Otherwise, use the world-space axis directly.
        Vector3 effectiveAxis = useRelativeAxis ? transform.TransformDirection(rotationAxis.normalized) : rotationAxis.normalized;

        // --- Orbit Calculation (if enabled) ---
        if (rotateAboutCenter)
        {
            // Calculate the vector from the center point to the object's current position.
            Vector3 offset = transform.position - center;

            // Rotate this offset vector around our effective axis.
            offset = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, effectiveAxis) * offset;

            // Apply the rotated offset back to the center to get the new position.
            transform.position = center + offset;
        }

        // --- Self-Rotation ---
        // Rotate the object itself around the same effective axis in world space.
        // This keeps the object's orientation consistent with its orbital motion.
        transform.Rotate(effectiveAxis, rotationSpeed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// Calculates the combined bounding box center of all Renderers in this object and its children.
    /// </summary>
    void CalculateCenter()
    {
        renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }
            center = bounds.center;
        }
        else
        {
            // If no renderers are found, default to the object's transform position.
            center = transform.position;
        }
    }
    
    /// <summary>
    /// Draws a gizmo in the editor to show the calculated center of rotation.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (rotateAboutCenter)
        {
            // Ensure the center is calculated for the gizmo display.
            CalculateCenter();
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(center, 0.1f);
        }
    }
}