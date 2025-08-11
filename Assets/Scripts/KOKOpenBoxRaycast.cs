using UnityEngine;

public class kokOpenBoxRaycast : MonoBehaviour
{
    public Animator boxOB;
    public GameObject openText;
    public AudioSource openSound;

    public float interactionDistance = 3f;
    public Camera playerCamera;

    public string requiredItemName = "Key 1"; // The name of the required key
    public Inventory playerInventory; // Reference to player's inventory

    [HideInInspector]
    public bool isBoxOpen = false;

    void Start()
    {
        openText.SetActive(false);
    }

    void Update()
    {
        if (isBoxOpen) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                openText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Check if player has the required key
                    if (playerInventory != null && playerInventory.HasItem(requiredItemName))
                    {
                        // Remove the key so it can't be reused
                        playerInventory.RemoveItem(requiredItemName);

                        openSound.Play();
                        boxOB.SetBool("open", true);
                        setBoxOpen();
                        openText.SetActive(false);
                        DisableSelf();
                    }
                    else
                    {
                        Debug.Log("You need the " + requiredItemName + " to open this box!");
                    }
                }
            }
            else
            {
                openText.SetActive(false);
            }
        }
        else
        {
            openText.SetActive(false);
        }
    }

    void setBoxOpen()
    {
        isBoxOpen = true;
    }

    void DisableSelf()
    {
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }
}
