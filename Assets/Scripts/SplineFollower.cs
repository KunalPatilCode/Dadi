// SmartSplineFollower.cs
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Moves an object along a network of splines by automatically detecting the nearest
/// connection point, removing the need for a SplineConnector component.
/// It maintains a robust "ping-pong" reversing behavior.
/// </summary>
[RequireComponent(typeof(Animator))]
public class SplineFollower : MonoBehaviour
{
    [Tooltip("The Spline Container the object is currently following.")]
    public SplineContainer currentSplineContainer;

    [Tooltip("A list of ALL spline containers that are part of this network.")]
    public List<SplineContainer> splineNetwork;

    [Tooltip("The maximum distance to look for a connection before considering it a dead end.")]
    public float connectionThreshold = 2f;

    [Tooltip("The speed at which the object moves along the spline.")]
    public float speed = 5f;

    // State variables
    private float progress = 0f;
    private int currentSplineIndex = 0;
    private bool isReversed = false;
    private Animator animator;
    private Stack<SplineContainer> pathHistory = new Stack<SplineContainer>();

    void Start()
    {
        animator = GetComponent<Animator>();
        // Create a mutable copy of the network to safely add/remove items.
        splineNetwork = new List<SplineContainer>(splineNetwork);
        if (splineNetwork.Contains(currentSplineContainer))
        {
            splineNetwork.Remove(currentSplineContainer);
        }
    }

    void Update()
    {
        if (currentSplineContainer == null)
        {
            if (animator.GetBool("IsRunning")) animator.SetBool("IsRunning", false);
            return;
        }

        if (!animator.GetBool("IsRunning")) animator.SetBool("IsRunning", true);

        MoveAlongSpline();
    }

    private void MoveAlongSpline()
    {
        float distanceToTravel = speed * Time.deltaTime;
        while (distanceToTravel > 0 && currentSplineContainer != null)
        {
            float currentSplineLength = currentSplineContainer.CalculateLength(currentSplineIndex);
            if (currentSplineLength <= 0) { TransitionToNextSegment(); continue; }

            float progressThisFrame = distanceToTravel / currentSplineLength;
            if (isReversed)
            {
                if (progress - progressThisFrame <= 0f)
                {
                    distanceToTravel -= progress * currentSplineLength;
                    progress = 0f;
                    UpdateTransform();
                    TransitionToNextSegment();
                }
                else
                {
                    progress -= progressThisFrame;
                    UpdateTransform();
                    distanceToTravel = 0;
                }
            }
            else
            {
                if (progress + progressThisFrame >= 1f)
                {
                    distanceToTravel -= (1f - progress) * currentSplineLength;
                    progress = 1f;
                    UpdateTransform();
                    TransitionToNextSegment();
                }
                else
                {
                    progress += progressThisFrame;
                    UpdateTransform();
                    distanceToTravel = 0;
                }
            }
        }
    }

    private void TransitionToNextSegment()
    {
        if (isReversed)
        {
            currentSplineIndex--;
            if (currentSplineIndex >= 0)
            {
                progress = 1f;
            }
            else
            {
                if (pathHistory.Count > 0)
                {
                    splineNetwork.Add(currentSplineContainer);
                    currentSplineContainer = pathHistory.Pop();
                    currentSplineIndex = currentSplineContainer.Splines.Count - 1;
                    progress = 1f;
                }
                else
                {
                    isReversed = false;
                    currentSplineIndex = 0;
                    progress = 0f;
                }
            }
        }
        else
        {
            currentSplineIndex++;
            if (currentSplineIndex < currentSplineContainer.Splines.Count)
            {
                progress = 0f;
            }
            else
            {
                FindAndTransitionToNearestSpline();
            }
        }
    }

    private void FindAndTransitionToNearestSpline()
    {
        // Correctly get the end point of the very last spline in the current container.
        int lastSplineIndex = currentSplineContainer.Splines.Count - 1;
        Vector3 currentEndPoint = currentSplineContainer.EvaluatePosition(lastSplineIndex, 1f);
        
        SplineContainer bestNextContainer = null;
        float minDistance = float.MaxValue;

        // Find the closest START point among all splines in the network.
        foreach (var container in splineNetwork)
        {
            if (container == currentSplineContainer) continue;

            // Get the start point of the very first spline in the potential next container.
            Vector3 nextSplineStartPoint = container.EvaluatePosition(0, 0f);
            float distanceToStart = Vector3.Distance(currentEndPoint, nextSplineStartPoint);
            if (distanceToStart < minDistance)
            {
                minDistance = distanceToStart;
                bestNextContainer = container;
            }
        }

        // Check if the best found connection is a valid forward path.
        if (bestNextContainer != null && minDistance <= connectionThreshold)
        {
            pathHistory.Push(currentSplineContainer);
            splineNetwork.Add(currentSplineContainer);

            currentSplineContainer = bestNextContainer;
            splineNetwork.Remove(bestNextContainer);

            currentSplineIndex = 0;
            progress = 0f;
            isReversed = false; // Ensure we are moving forward.
        }
        else
        {
            // No valid connection found, it's a dead end. Reverse direction globally.
            isReversed = true;
            // The index is currently out of bounds, so set it to the last valid index.
            currentSplineIndex = lastSplineIndex;
            progress = 1f;
        }
    }

    private void UpdateTransform()
    {
        if (currentSplineContainer == null || currentSplineIndex < 0 || currentSplineIndex >= currentSplineContainer.Splines.Count) return;

        progress = Mathf.Clamp01(progress);
        Vector3 newPosition = currentSplineContainer.EvaluatePosition(currentSplineIndex, progress);
        Vector3 tangent = Vector3.Normalize(currentSplineContainer.EvaluateTangent(currentSplineIndex, progress));
        transform.position = newPosition;
        Vector3 lookDirection = isReversed ? -tangent : tangent;

        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }
}
