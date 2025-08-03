using UnityEngine;

public class AnimatedPickUpRaycast : MonoBehaviour
{
    public GameObject keyOB;
    public GameObject pickUpText;
    public GameObject requirepickUpText; // New public variable for the required item text
    public GameObject player;
    public AudioSource keySound;

    public float interactionDistance = 3f;
    public Camera playerCamera;

    public GameObject[] rotateObjects;
    public float[] rotateValues;
    public int rotationTime = 5;
    public OpenBoxRaycast boxScript;
    public bool isAddToInventory = true;
    
    public string requiredItemName = "Wheat"; // The name of the item required for this interaction.

    private InventoryItem inventoryItem;
    private bool isPicked = false;
    private bool wasHit = false;
    private Inventory playerInventory; // Reference to the player's inventory

    void Start()
    {
        pickUpText.SetActive(false);
        if (requirepickUpText != null)
        {
            requirepickUpText.SetActive(false);
        }
        inventoryItem = GetComponent<InventoryItem>();
        playerInventory = player.GetComponent<Inventory>();
        stopRotation();
    }

    void Update()
    {
        if (isPicked || (boxScript != null && !boxScript.isBoxOpen)) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                wasHit = true;
                
                // Core Logic: Check if the player has the required item
                if (playerInventory.HasItem(requiredItemName))
                {
                    // Player has the required item, show the normal pickup text
                    pickUpText.SetActive(true);
                    if (requirepickUpText != null)
                    {
                        requirepickUpText.SetActive(false);
                    }
                    
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        keySound.Play();
                        pickUpText.SetActive(false);
                        startRotation();
                        DisableSelf();
                        Invoke("stopRotation", rotationTime);
                        if(isAddToInventory)
                            Invoke("addToInventory", rotationTime);
                        Invoke("EnableSelf", rotationTime);
                    }
                }
                else
                {
                    // Player does not have the required item, show the prerequisite text
                    pickUpText.SetActive(false);
                    if (requirepickUpText != null)
                    {
                        requirepickUpText.SetActive(true);
                    }
                }
            }
        }
        else
        {
            if (wasHit)
            {
                // Deactivate both texts when not looking at the object
                pickUpText.SetActive(false);
                if (requirepickUpText != null)
                {
                    requirepickUpText.SetActive(false);
                }
                wasHit = false;
            }
        }
    }

    void addToInventory()
    {
        player.GetComponent<Inventory>().AddItem(new Inventory.InventoryItem(inventoryItem.itemName, inventoryItem.itemIcon, inventoryItem.stackSize));
    }

    void stopRotation()
    {
        for (int i = 0; i < rotateObjects.Length; i++)
        {
            if (rotateObjects[i] != null && rotateObjects[i].GetComponent<RotateAroundCenter>() != null)
            {
                rotateObjects[i].GetComponent<RotateAroundCenter>().rotationSpeed = 0f;
            }
        }
    }

    void startRotation()
    {
        for (int i = 0; i < rotateObjects.Length; i++)
        {
            if (rotateObjects[i] != null && rotateObjects[i].GetComponent<RotateAroundCenter>() != null)
            {
                rotateObjects[i].GetComponent<RotateAroundCenter>().rotationSpeed = rotateValues[i];
            }
        }
    }

    void DisableSelf()
    {
        keyOB.SetActive(false);
        isPicked = true;
        if (player != null && player.GetComponent<PlayerMovement>() != null)
        {
            player.GetComponent<PlayerMovement>().animating = true;
        }
    }
    
    void EnableSelf()
    {
        keyOB.SetActive(true);
        isPicked = false;
        if (player != null && player.GetComponent<PlayerMovement>() != null)
        {
            player.GetComponent<PlayerMovement>().animating = false;
        }
    }
}