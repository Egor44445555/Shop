using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraPerson : MonoBehaviour
{
    public static CameraPerson main;
    public Transform target;
    public Vector3 offsetPos;
    
    [SerializeField] float rotationSpeed = 60f;
    [SerializeField] float minVerticalAngle = -80f;
    [SerializeField] float maxVerticalAngle = 80f;

    Vector3 targetPos;
    Vector3 velocity = Vector3.zero;
    Vector2 touchStartPosition;
    Vector2 touchDeltaPosition;
    Vector3 cameraRotation;

    void Awake()
    {
        main = this;
    }

    void Update()
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

    void LateUpdate()
    {
        transform.position = target.transform.position + offsetPos;
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
