using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepScript : MonoBehaviour
{
    public GameObject Footstep; // Assign the Footstep GameObject

    void Start()
    {
        if (Footstep == null)
        {
            Debug.LogError("Footstep GameObject is not assigned!");
        }
        else
        {
            Debug.Log("Footstep GameObject is assigned correctly.");
            Footstep.SetActive(false);
        }
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if (moveX != 0 || moveY != 0)
        {
            ActivateFootstep();
        }
        else
        {
            DeactivateFootstep();
        }
    }

    void ActivateFootstep()
    {
        if (!Footstep.activeSelf) // Avoid redundant activations
        {
            Footstep.SetActive(true);
            Debug.Log("Footstep activated");
        }
    }

    void DeactivateFootstep()
    {
        if (Footstep.activeSelf) // Avoid redundant deactivations
        {
            Footstep.SetActive(false);
            Debug.Log("Footstep deactivated");
        }
    }
}
