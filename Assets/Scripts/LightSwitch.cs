using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameObject onOB;
    public GameObject offOB;
    public GameObject lightsText;
    public GameObject lightOB;
    public AudioSource switchClick;
    
    private bool lightsAreOn = false;
    private bool inReach = false;

    void Start()
    {
        UpdateLightState();
        if (lightsText != null)
        {
            lightsText.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = true;
            if (lightsText != null)
            {
                lightsText.SetActive(true); 
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = false;
            if (lightsText != null)
            {
                lightsText.SetActive(false); 
            }
        }
    }

    void Update()
    {
        if (inReach && Input.GetButtonDown("Interact"))
        {
            ToggleLights();
        }
    }

    void ToggleLights()
    {
        lightsAreOn = !lightsAreOn;
        UpdateLightState();
        if (switchClick != null)
        {
            switchClick.Play();
        }
    }

    void UpdateLightState()
    {
        if (lightOB != null) lightOB.SetActive(lightsAreOn);
        if (onOB != null) onOB.SetActive(lightsAreOn);
        if (offOB != null) offOB.SetActive(!lightsAreOn);
    }
}
