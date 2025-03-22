using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepScript : MonoBehaviour
{
    public GameObject Player; // Assign the Player GameObject

    void Start()
    {
        if (Player == null)
        {
            Debug.LogError("Player GameObject is not assigned!");
        }
        else
        {
            Debug.Log("Player GameObject is assigned correctly.");
            Player.SetActive(false);
        }
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if (moveX != 0 || moveY != 0)
        {
            ActivatePlayer();
        }
        else
        {
            DeactivatePlayer();
        }
    }

    void ActivatePlayer()
    {
        if (!Player.activeSelf) // Avoid redundant activations
        {
            Player.SetActive(true);
            Debug.Log("Player activated");
        }
    }

    void DeactivatePlayer()
    {
        if (Player.activeSelf) // Avoid redundant deactivations
        {
            Player.SetActive(false);
            Debug.Log("Player deactivated");
        }
    }
}
