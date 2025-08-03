using UnityEngine;

public class AnimatedPickUpRaycast : MonoBehaviour
{
    // ... (Your existing public variables) ...
    public GameObject keyOB;
    public GameObject pickUpText;
    public GameObject requirepickUpText;
    public GameObject player;
    public AudioSource keySound;

    public float interactionDistance = 3f;
    public Camera playerCamera;

    public GameObject[] rotateObjects;
    public float[] rotateValues;
    public int rotationTime = 5;
    public OpenBoxRaycast boxScript;
    public bool isAddToInventory = true;
    
    public string requiredItemName = "Wheat"; // The name of the item to be removed

    // NEW: The object that will appear during the animation
    public GameObject animatedObject;

    private InventoryItem inventoryItem;
    private bool isPicked = false;
    private bool wasHit = false;
    private Inventory playerInventory;

    void Start()
    {
        pickUpText.SetActive(false);
        if (requirepickUpText != null)
        {
            requirepickUpText.SetActive(false);
        }
        
        // NEW: Ensure the animated object is hidden at the start
        if (animatedObject != null)
        {
            animatedObject.SetActive(false);
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
                
                if (playerInventory.HasItem(requiredItemName))
                {
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
                        
                        RemoveRequiredItem(); 

                        Invoke("stopRotation", rotationTime);
                        if(isAddToInventory)
                            Invoke("addToInventory", rotationTime);
                        Invoke("EnableSelf", rotationTime);
                    }
                }
                else
                {
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
                pickUpText.SetActive(false);
                if (requirepickUpText != null)
                {
                    requirepickUpText.SetActive(false);
                }
                wasHit = false;
            }
        }
    }

    void RemoveRequiredItem()
    {
        if (playerInventory.HasItem(requiredItemName))
        {
            playerInventory.RemoveItem(requiredItemName);
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
        
        // NEW: Hide the animated object once the animation is done
        if (animatedObject != null)
        {
            animatedObject.SetActive(false);
        }
    }

    void startRotation()
    {
        // NEW: Show the animated object when the animation starts
        if (animatedObject != null)
        {
            animatedObject.SetActive(true);
        }
        
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