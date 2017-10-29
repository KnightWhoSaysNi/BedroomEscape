using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private Transform myTransform;
    private Vector3 myPosition;
    private Camera orthographicCamera;

    [SerializeField] private CircleCollider2D draggableArea;
    private Vector3 draggableAreaCenter;
    private Vector3 mouseStartPosition;
    private Vector3 mouseEndPosition;
    private Vector3 offset;
    private Vector3 newPosition;
    private float draggableRadius;
    private bool isDragging;
    private bool isFalling;
    private float fallingSpeed = 30;

    private void Start()
    {
        myTransform = transform;
        orthographicCamera = GameManager.Instance.orthographicCamera.GetComponent<Camera>();
        draggableAreaCenter = draggableArea.transform.position;
        draggableRadius = draggableArea.radius;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && isDragging)
        {
            DragObject();
        }

        if (isFalling)
        {
            FallIntoDraggableArea();            
        }
    }

    /// <summary>
    /// Drags object in the direction of the mouse position, from the point of contact.
    /// </summary>
    private void DragObject()
    {
        mouseEndPosition = Input.mousePosition;
        mouseEndPosition = orthographicCamera.ScreenToWorldPoint(mouseEndPosition);
        mouseEndPosition.z = 0;

        newPosition = mouseEndPosition + offset;
        myTransform.position = newPosition;
    }

    /// <summary>
    /// Moves object back into the draggable area, over time.
    /// </summary>
    private void FallIntoDraggableArea()
    {
        myTransform.position = Vector3.Lerp(myTransform.position, newPosition, fallingSpeed * Time.deltaTime);

        if (myTransform.position == newPosition)
        {
            isFalling = false;
        }
    }

    /// <summary>
    /// Signals the start of dragging behaviour.
    /// </summary>
    private void OnMouseDown()
    {
        isDragging = true;

        mouseStartPosition = Input.mousePosition;
        mouseStartPosition = orthographicCamera.ScreenToWorldPoint(mouseStartPosition);
        mouseStartPosition.z = 0;

        offset = myTransform.position - mouseStartPosition;
    }

    /// <summary>
    /// Signals the end of dragging behaviour.
    /// </summary>
    private void OnMouseUp()
    {
        isDragging = false;

        myPosition = myTransform.position;

        // If the center of the object is beyond the draggable area it should fall down into it
        if (!draggableArea.OverlapPoint(myPosition))
        {
            newPosition = (myPosition - draggableAreaCenter).normalized * draggableRadius;
            newPosition += draggableAreaCenter;
            isFalling = true;
        }
    }
}
