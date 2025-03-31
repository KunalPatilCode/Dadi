using UnityEngine;

public class DoorSoundTrigger : MonoBehaviour
{
    public AudioSource doorSound; // Assign the door sound in the Inspector

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if it's the player
        {
            if (doorSound && !doorSound.isPlaying) 
            {
                doorSound.Play(); // Play sound when the player touches the door
            }
        }
    }
}
