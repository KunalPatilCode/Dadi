using UnityEngine;

public class RaycastInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public Camera playerCamera;
    public GameObject interactionUI; // UI that shows "Press E"

    private GameObject currentTarget;

    void Start()
    {
        interactionUI.SetActive(false); // Hide the UI initially

        // Turn off all lights that are tagged as LightInteractable
        GameObject[] lights = GameObject.FindGameObjectsWithTag("LightInteractable");
        foreach (GameObject lightObj in lights)
        {
            Light lightComponent = lightObj.GetComponentInChildren<Light>();
            if (lightComponent != null)
            {
                lightComponent.enabled = false;
            }
        }
    }

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.CompareTag("LightInteractable"))
            {
                currentTarget = hit.collider.gameObject;
                interactionUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    ToggleLight(currentTarget);
                }
            }
            else
            {
                interactionUI.SetActive(false);
                currentTarget = null;
            }
        }
        else
        {
            interactionUI.SetActive(false);
            currentTarget = null;
        }
    }

    void ToggleLight(GameObject obj)
    {
        Light lightComponent = obj.GetComponentInChildren<Light>();
        if (lightComponent != null)
        {
            lightComponent.enabled = !lightComponent.enabled;
        }
    }
}
