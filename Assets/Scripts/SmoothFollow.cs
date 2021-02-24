using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
    public bool follow = true;

    // Target to follow.
    public Transform target;
    public float height = 20.0f;
    public float smoothDampTime = 0.5f;

    // The point in at which the camera will be set in full view.
    [SerializeField] private Transform allMapViewPosition = null;

    private Vector3 smoothDampVel;

    void LateUpdate()
    {
        if (!target || !follow)
            return;
        SmoothDampToTarget();
    }

    void SmoothDampToTarget()
    {
        var targetPosition = target.position + Vector3.up * height;
        targetPosition = transform.TransformPoint(transform.InverseTransformPoint(targetPosition) - new Vector3(0, 0, 12));
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref smoothDampVel,
            smoothDampTime
        );

        transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, 1);
    }

    public void GotoAllMapView()
    {
        if (allMapViewPosition == null)
            return;

        transform.position = allMapViewPosition.position;
    }
}
