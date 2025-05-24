using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MergeZone : MonoBehaviour
{
    public List<string> requiredItems; // e.g. ["Cotton", "Thread"]
    private List<GameObject> currentObjects = new List<GameObject>();

    public GameObject dollPrefab;
    public Transform spawnPoint;

    public UnityEvent onMergeSuccess;

    private void OnTriggerEnter(Collider other)
    {
        MergeItem item = other.GetComponent<MergeItem>();
        if (item != null)
        {
            currentObjects.Add(other.gameObject);
            CheckForMerge();
        }
    }

    void CheckForMerge()
    {
        List<string> collectedNames = new List<string>();
        foreach (var obj in currentObjects)
        {
            collectedNames.Add(obj.GetComponent<MergeItem>().itemName);
        }

        // Check if all required items are present
        foreach (string required in requiredItems)
        {
            if (!collectedNames.Contains(required))
                return; // missing something, abort merge
        }

        // Merge complete: destroy items
        foreach (var obj in currentObjects)
        {
            Destroy(obj);
        }
        currentObjects.Clear();

        // Spawn final doll
        Instantiate(dollPrefab, spawnPoint.position, spawnPoint.rotation);

        // Optional: trigger merge event (sound, effect, UI etc.)
        if (onMergeSuccess != null)
            onMergeSuccess.Invoke();
    }
}
