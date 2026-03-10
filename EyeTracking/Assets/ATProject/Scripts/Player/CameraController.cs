using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float followSpeed = 2f;
    public float yOffset = 0f;
    public float zOffset = 0f;
    public Transform target;

    private void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, yOffset, target.position.z + zOffset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
