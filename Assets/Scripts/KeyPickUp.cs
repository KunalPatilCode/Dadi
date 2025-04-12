using UnityEngine;

public class KeyPickUpRaycast : MonoBehaviour
{
    public GameObject keyOB;
    public GameObject invOB;
    public GameObject pickUpText;
    public AudioSource keySound;

    public float interactionDistance = 3f;
    public Camera playerCamera;

    public OpenBoxRaycast boxScript; // ← Reference to box script

    private bool isPicked = false;

    void Start()
    {
        pickUpText.SetActive(false);
        invOB.SetActive(false);
    }

    void Update()
    {
        if (isPicked || !boxScript.isBoxOpen) return; // ← Block pickup until box is open

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
                    invOB.SetActive(true);
                    pickUpText.SetActive(false);
                    isPicked = true;
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
