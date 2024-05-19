using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    public float followSpeed = 5f; // Speed at which the camera follows the player
    public Vector2 mouseOffsetMultiplier = new Vector2(0.5f, 0.5f); // Multiplier for the mouse offset
    public Vector2 offsetCap = new Vector2(2f, 2f); // Maximum offset from the player's position

    private Transform playerTransform;
    private Camera mainCamera;
    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;

    private void Start()
    {
        mainCamera = this.GetComponent<Camera>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure the z position is 0 for 2D

        Vector3 offset = (mousePosition - playerTransform.position);
        offset.x = Mathf.Clamp(offset.x * mouseOffsetMultiplier.x, -offsetCap.x, offsetCap.x);
        offset.y = Mathf.Clamp(offset.y * mouseOffsetMultiplier.y, -offsetCap.y, offsetCap.y);
        offset.z = -10f;

        desiredPosition = playerTransform.position + offset;
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.fixedDeltaTime);

        transform.position = smoothedPosition;
    }
}