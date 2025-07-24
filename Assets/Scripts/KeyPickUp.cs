using UnityEngine;

public class KeyPickUpRaycast : MonoBehaviour
{
    public GameObject keyOB;
    public GameObject pickUpText;
    public GameObject player;
    public AudioSource keySound;

    public float interactionDistance = 3f;
    public Camera playerCamera;

    public OpenBoxRaycast boxScript; // ← Reference to box script

    private InventoryItem inventoryItem;

    private bool isPicked = false;

    void Start()
    {
        pickUpText.SetActive(false);
        inventoryItem = GetComponent<InventoryItem>();
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
                pickUpText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    keySound.Play();
                    keyOB.SetActive(false);
                    pickUpText.SetActive(false);
                    isPicked = true;
                    player.GetComponent<Inventory>().AddItem(new Inventory.InventoryItem(inventoryItem.itemName, inventoryItem.itemIcon, inventoryItem.stackSize));
                    DisableSelf();
                }
            }
            else
            {
                pickUpText.SetActive(false);
            }
        }
        else
        {
            pickUpText.SetActive(false);
        }
    }

    void DisableSelf()
    {
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }
}
