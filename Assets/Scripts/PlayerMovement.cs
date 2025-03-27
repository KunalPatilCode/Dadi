using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;
    public Rigidbody rb;
    public Transform playerCamera;
    public float mouseSensitivity = 100f; // Mouse sensitivity
    public float headBobFrequency = 2f; // Adjust to control bobbing speed
    public float headBobAmplitude = 0.25f; // Adjust to control bobbing height
    public float idleBobFactor = 0.5f;
    public GameObject arms;
    public GameObject tempLight;
    public Light lanternLight;
    public GameObject houseLightBounds;
    public float maxEmissionIntensity = 5f;
    public float minEmissionIntensity = 1f;

    private float xRotation = 0f;
    private float headBobTime;
    private Vector3 originalCameraPosition;
    private Vector3 originalArmsPosition;
    private Collider cubeCollider;

    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        //tempLight.SetActive(false);

        // Find all GameObjects in the scene.
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Iterate through each GameObject.
        foreach (GameObject obj in allObjects)
        {
            // Check if the GameObject's name contains "ring" (case-insensitive).
            if (obj.name.ToLower().Contains("ring"))
            {
                GameObject defaultChild = FindDeepChild(obj, "default");

                if (defaultChild != null)
                {
                    MeshRenderer renderer = defaultChild.GetComponent<MeshRenderer>();
                    if (renderer != null)
                    {
                        renderer.enabled = false;
                    }
                }
            }

            if (obj.name.ToLower().Contains("tunnel") 
                || obj.name.ToLower().Contains("turn") 
                || obj.name.ToLower().Contains("ramp"))
            {
                if (obj != null)
                {
                    MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
                    if (renderer != null)
                    {
                        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    }
                }
            }
        }
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        originalCameraPosition = playerCamera.transform.localPosition;
        originalArmsPosition = arms.transform.localPosition;
    }

    void Update()
    {
        // Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * verticalInput + right * horizontalInput).normalized;

        Vector3 moveVelocity = moveDirection * moveSpeed;
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        // Head Bobbing
        if (true)//isGrounded)
        {
            headBobTime += Time.deltaTime * headBobFrequency * (moveDirection.magnitude == 0? idleBobFactor : 1f);
            float verticalOffset = Mathf.Sin(headBobTime) * headBobAmplitude * (moveDirection.magnitude == 0? idleBobFactor : 1f);
            float armsOffset = Mathf.Sin(headBobTime-3.14f/6f) * headBobAmplitude/3f * (moveDirection.magnitude == 0? idleBobFactor : 1f);
            playerCamera.transform.localPosition = originalCameraPosition + Vector3.up * verticalOffset;
            arms.transform.localPosition = originalArmsPosition + Vector3.up * armsOffset;
        }
        else
        {
            //playerCamera.transform.localPosition = originalCameraPosition; // Reset position when not moving
        }

        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical rotation

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Apply vertical rotation to camera
        transform.Rotate(Vector3.up * mouseX); // Apply horizontal rotation to player

        // Align with opposite of gravity (upwards)
        Quaternion targetUpRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
        rb.rotation = Quaternion.RotateTowards(rb.rotation, targetUpRotation, rotationSpeed * Time.deltaTime * 3);

        if (IsPlayerInCube())
        {
            lanternLight.intensity = maxEmissionIntensity;
        }
        else
        {
            lanternLight.intensity = minEmissionIntensity;
        }
    }

    bool IsPlayerInCube()
    {
        if(cubeCollider == null)
            cubeCollider = houseLightBounds.GetComponent<Collider>();

        if (cubeCollider.bounds.Contains(transform.position))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    GameObject FindDeepChild(GameObject parent, string childName)
    {
        foreach (Transform childTransform in parent.transform)
        {
            GameObject child = childTransform.gameObject;
            if (child.name.ToLower() == childName.ToLower())
            {
                return child;
            }

            GameObject found = FindDeepChild(child, childName);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }
}