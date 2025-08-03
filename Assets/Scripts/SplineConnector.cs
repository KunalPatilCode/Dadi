// SplineConnector.cs
using UnityEngine;
using UnityEngine.Splines;
using System.Collections.Generic;

/// <summary>
/// Defines connections between splines. Attach this to a GameObject
/// that has a SplineContainer to link it to subsequent splines.
/// </summary>
public class SplineConnector : MonoBehaviour
{
    [Tooltip("A list of splines that the follower can transition to from this one. " +
             "If more than one, it's a diverging path.")]
    public List<SplineContainer> nextSplines;
}
