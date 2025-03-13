using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraPerson : MonoBehaviour
{
    public Transform target;
    public Vector3 offsetPos;
    public float moveSpeed = 5;
    public float smooth = 0.2f;

    [Header("Camera Settings")]
    [SerializeField] float rotationSpeed = 60f;
    [SerializeField] float minVerticalAngle = -80f;
    [SerializeField] float maxVerticalAngle = 80f;

    Vector3 targetPos;
    Vector3 velocity = Vector3.zero;
    Vector2 touchStartPosition;
    Vector2 touchDeltaPosition;
    Vector3 cameraRotation;

    void Update()
    {
        HandleTouchInput();
    }

    void LateUpdate()
    {
        targetPos = target.transform.position + offsetPos;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smooth);
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return;
            }

            switch (touch.phase)
            {
                case TouchPhase.Began: touchStartPosition = touch.position;
                    break;

                case TouchPhase.Moved: touchDeltaPosition = touch.position - touchStartPosition;

                    RotateCamera(touchDeltaPosition);

                    touchStartPosition = touch.position;
                    break;
            }
        }
    }

    void RotateCamera(Vector2 deltaPosition)
    {
        float deltaX = deltaPosition.x * rotationSpeed * Time.deltaTime;
        float deltaY = -deltaPosition.y * rotationSpeed * Time.deltaTime;

        cameraRotation.x += deltaY;
        cameraRotation.y += deltaX;

        cameraRotation.x = Mathf.Clamp(cameraRotation.x, minVerticalAngle, maxVerticalAngle);
        transform.rotation = Quaternion.Euler(cameraRotation);
    }
}
