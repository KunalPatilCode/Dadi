using UnityEngine;

public class OpenBoxRaycast : MonoBehaviour
{
    public Animator boxOB;
    public GameObject openText;
    public AudioSource openSound;

    public float interactionDistance = 3f;
    public Camera playerCamera;

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
            if (hit.collider.gameObject == this.gameObject)
            {
                openText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    openSound.Play();
                    boxOB.SetBool("open", true);
                    openText.SetActive(false);
                    DisableSelf();
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
