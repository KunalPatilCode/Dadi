using UnityEngine;

public class ConditionalKeyPickup : MonoBehaviour
{
    public GameObject keyOB;
    public GameObject pickUpText;
    public GameObject requirepickUpText;

    public GameObject player;
    public AudioSource keySound;

    public float interactionDistance = 3f;
    public Camera playerCamera;
    
    public string requiredItemName = "Key 1"; // This is the name of the first item you need.
    
    private InventoryItem inventoryItem;
    private bool isPicked = false;
    private bool wasHit = false;
    private Inventory playerInventory;

    void Start()
    {
        pickUpText.SetActive(false);
        requirepickUpText.SetActive(false);
        inventoryItem = GetComponent<InventoryItem>();
        playerInventory = player.GetComponent<Inventory>();
    }

    void OnEnable()
    {
        isPicked = false;
    }

    void Update()
    {
        // Don't do anything if the item is already picked up
        if (isPicked) return;

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
                    requirepickUpText.SetActive(false);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Pickup Logic
                        keySound.Play();
                        keyOB.SetActive(false);
                        pickUpText.SetActive(false);
                        isPicked = true;
                        playerInventory.AddItem(new Inventory.InventoryItem(inventoryItem.itemName, inventoryItem.itemIcon, inventoryItem.stackSize));
                    }
                }
                else
                {
                    // Player does NOT have the required item, show the "required" text
                    pickUpText.SetActive(false);
                    requirepickUpText.SetActive(true);
                }
            }
        }
        else
        {
            // If the raycast is not hitting the object
            if (wasHit)
            {
                // Deactivate both texts to prevent any from staying on screen
                pickUpText.SetActive(false);
                requirepickUpText.SetActive(false);
                wasHit = false;
            }
        }
    }
}