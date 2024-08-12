using System;
using UnityEngine;

public abstract class AbstractSearchingTarget : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float detectionRange = 5.0f; // Detection range
    public LayerMask playerLayer; // Player layer mask
    public Color rayColor = Color.red; // Raycast color
    public Action attackPlayerMove; // Action delegate for attacking the player
    public Action Justmoving;
    private AbstractMovement movementScript; // Reference to AbstractMovement
    public AbstractMovement Checkmove;
    void Start()
    {
        // Find AbstractMovement on the same GameObject
        movementScript = GetComponent<AbstractMovement>();
        if (movementScript == null)
        {
            Debug.LogError("AbstractMovement is not assigned to this GameObject.");
        }
    }

    void Update()
    {
        DetectPlayer();
        DrawRaycast();
    }

    void DetectPlayer()
    {
        if (movementScript == null) return;

        Vector2 direction = movementScript.GetCurrentDirection();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Justmoving?.Invoke();
            Checkmove.isMoving = true;
            Debug.Log("Nothing");
        }
        else
        {
            attackPlayerMove?.Invoke(); // Invoke the delegate if subscribed
            Checkmove.isMoving = false;
            Debug.Log("Player");
            
            //đoạn mã trên đang ngược tính năng tạm thời đổi attackPlayer xuống dưới
        }
    }

    void DrawRaycast()
    {
        if (movementScript == null) return;

        Vector2 direction = -movementScript.GetCurrentDirection();
        Debug.DrawRay(transform.position, direction * detectionRange, rayColor);
    }
}