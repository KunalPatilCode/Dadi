using UnityEngine;

public class DragObject : MonoBehaviour
{
    public Camera mainCam;
    private GameObject selectedObject;
    private Vector3 offset;
    private float zCoord;
    public float maxDistance = 5f; // Max raycast range
    public LayerMask draggableLayer; // assign to only draggable objects

    void Start()
    {
        if (mainCam == null)
            mainCam = Camera.main;
    }

    void Update()
    {
        Ray ray = new Ray(mainCam.transform.position, mainCam.transform.forward);
        RaycastHit hit;

        // Check if looking at an object within range
        if (Physics.Raycast(ray, out hit, maxDistance, draggableLayer))
        {
            Debug.DrawRay(mainCam.transform.position, mainCam.transform.forward * hit.distance, Color.green);

            if (Input.GetMouseButtonDown(0))
            {
                selectedObject = hit.collider.gameObject;
                zCoord = mainCam.WorldToScreenPoint(selectedObject.transform.position).z;
                offset = selectedObject.transform.position - GetMouseWorldPos();
            }

            // Optional: Show hint on object when looking
            // Debug.Log("Looking at: " + hit.collider.gameObject.name + " | Press LMB to Drag");
        }

        // Drag selected object
        if (Input.GetMouseButton(0) && selectedObject != null)
        {
            selectedObject.transform.position = GetMouseWorldPos() + offset;
        }

        // Release object
        if (Input.GetMouseButtonUp(0))
        {
            selectedObject = null;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = new Vector3(Screen.width / 2, Screen.height / 2, zCoord);
        return mainCam.ScreenToWorldPoint(mousePoint);
    }
}
