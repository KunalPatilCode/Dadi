using UnityEngine;

public class AnimatedPickUpRaycast : MonoBehaviour
{
    public GameObject keyOB;
    public GameObject pickUpText;
    public GameObject player;
    public AudioSource keySound;

    public float interactionDistance = 3f;
    public Camera playerCamera;

    public GameObject[] rotateObjects;
    public float[] rotateValues;
    public int rotationTime = 5;
    public OpenBoxRaycast boxScript; // ← Reference to box script
    public bool isAddToInventory = true;

    private InventoryItem inventoryItem;

    private bool isPicked = false;
    private bool wasHit = false;

    void Start()
    {
        pickUpText.SetActive(false);
        inventoryItem = GetComponent<InventoryItem>();
        stopRotation();
    }

    void Update()
    {
        if (isPicked || (boxScript != null && !boxScript.isBoxOpen)) return; // ← Block pickup until box is open

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                wasHit = true;
                pickUpText.SetActive(true);

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
        }
        else
        {
            if (wasHit)
            {
                pickUpText.SetActive(false);
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
            rotateObjects[i].GetComponent<RotateAroundCenter>().rotationSpeed = 0f;
        }
    }

    void startRotation()
    {
        for (int i = 0; i < rotateObjects.Length; i++)
        {
            rotateObjects[i].GetComponent<RotateAroundCenter>().rotationSpeed = rotateValues[i];
        }
    }

    void DisableSelf()
    {
        keyOB.SetActive(false);
        isPicked = true;
        player.GetComponent<PlayerMovement>().animating = true;
    }
    
    void EnableSelf()
    {
        keyOB.SetActive(true);
        isPicked = false;
        player.GetComponent<PlayerMovement>().animating = false;
    }
}
