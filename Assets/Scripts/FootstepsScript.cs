using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepScript : MonoBehaviour
{
    public GameObject Footstep; // Assign the Footstep GameObject
    public float minSpeed = 0f;
    public float maxSpeed = 5f;
    public float minPitch = 0.8f;
    public float maxPitch = 1.5f;

    private Vector3 lastPosition;
    private Vector3 actualVelocity = Vector3.zero;
    private Rigidbody rb;
    private AudioSource footstepAudio;
    private PlayerMovement playerMovement;
    private float headBobFrequency;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        footstepAudio = Footstep.GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();

        lastPosition = transform.position;
        headBobFrequency = playerMovement.headBobFrequency;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if (moveX != 0 || moveY != 0)
        {
            ActivateFootstep();
            float speed = actualVelocity.magnitude;
            Debug.Log(speed);
            float speedFactor = (speed/maxSpeed);
            playerMovement.headBobFrequency = headBobFrequency*speedFactor;
            float pitch = maxPitch*speedFactor;
            footstepAudio.pitch = pitch;
        }
        else
        {
            DeactivateFootstep();
            playerMovement.headBobFrequency = headBobFrequency;
        }
    }

    void FixedUpdate() // Use FixedUpdate for physics calculations
    {
        // Calculate the change in position
        Vector3 positionChange = transform.position - lastPosition;

        // Calculate the velocity
        actualVelocity = positionChange / Time.fixedDeltaTime;

        // Update the last position
        lastPosition = transform.position;
    }

    void ActivateFootstep()
    {
        if (!Footstep.activeSelf) // Avoid redundant activations
        {
            Footstep.SetActive(true);
        }
    }

    void DeactivateFootstep()
    {
        if (Footstep.activeSelf) // Avoid redundant deactivations
        {
            Footstep.SetActive(false);
        }
    }
}
