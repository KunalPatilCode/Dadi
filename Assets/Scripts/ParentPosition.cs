using UnityEngine;

public class ParentPosition : MonoBehaviour
{
    public Transform parent;

    private Transform parentTransform;
    private Vector3 initialOffset;

    void Start()
    {
        SetFollowingParent(parent);
    }

    public void SetFollowingParent(Transform parent)
    {
        parentTransform = parent;
        initialOffset = transform.position - parentTransform.position;
    }

    void LateUpdate()
    {
        if (parentTransform != null)
        {
            // Maintain the initial local position relative to the parent
            transform.position = parentTransform.position + initialOffset;

            // Optionally, you can also explicitly reset rotation and scale in each frame
            // transform.localRotation = Quaternion.identity;
            // transform.localScale = Vector3.one;
        }
    }
}
