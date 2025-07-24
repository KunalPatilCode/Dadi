using UnityEngine;

public class FlourBehaviour : MonoBehaviour
{
    public GameObject mill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnEnable()
    {
        mill.GetComponent<AnimatedPickUpRaycast>().enabled = false;
        Debug.Log(this.name + " (script) was ENABLED or its GameObject became active.");
    }

    void OnDisable()
    {
        mill.GetComponent<AnimatedPickUpRaycast>().enabled = true;
        Debug.Log(this.name + " (script) was DISABLED or its GameObject became inactive.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
