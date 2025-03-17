using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingThing : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;

    Vector3 offset;
    float zPosition;
    bool isDragging = false;
    bool onFloor = false;
    bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            // First touch
            Touch touch = Input.GetTouch(0);

            // Convert the touch point to world coordinates
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = zPosition;

            switch (touch.phase)
            {
                case TouchPhase.Began:

                    // Checking touch on object
                    if (IsTouchingObject(touchPosition))
                    {
                        // Writing offset to variable
                        offset = transform.position - touchPosition;
                        isDragging = true;

                        // Making object static
                        rb.bodyType = RigidbodyType2D.Kinematic;
                        isFalling = false;
                    }

                    break;

                case TouchPhase.Moved:
                    // Moving object if it is static
                    if (isDragging)
                    {
                        isFalling = false;
                        rb.bodyType = RigidbodyType2D.Kinematic;
                        transform.position = touchPosition + offset;
                        CameraTouchMove.main.cameraMove = false;
                    }
                    break;

                case TouchPhase.Ended:
                    // Returning the original state of the object
                    if (isDragging)
                    {
                        CameraTouchMove.main.cameraMove = true;
                        isDragging = false;
                        isFalling = false;

                        if (onFloor)
                        {
                            rb.bodyType = RigidbodyType2D.Kinematic;
                            rb.velocity = Vector2.zero;
                        }
                        else
                        {
                            isFalling = true;
                            rb.bodyType = RigidbodyType2D.Dynamic;
                        }
                    }
                    break;
            }
        }

        if (isFalling)
        {
            // Acceleration when falling
            rb.AddForce(Vector2.down * 5f, ForceMode2D.Force);
        }
    }

    bool IsTouchingObject(Vector3 touchPosition)
    {
        Collider2D hit = Physics2D.OverlapPoint(touchPosition);
        return hit != null && hit.gameObject == gameObject;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("DragArea"))
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            onFloor = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("DragArea"))
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            onFloor = false;
        }
    }
}
