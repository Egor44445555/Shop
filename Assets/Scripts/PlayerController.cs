using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerControllerPerson : MonoBehaviour 
{	
    public static PlayerControllerPerson main;
    public Joystick joystick;
    public float moveSpeed;
    public float collisionCheckDistance = 0.5f;

    [HideInInspector] public string collectibleName;

    Vector3 moveDirection;
    Transform cameraTransform;

    void Awake()
    {
        main = this;
    }

    void Start()
    {
        cameraTransform = CameraPerson.main.transform; 
    }

    void Update() 
    {
        Vector2 inputDirection = joystick.direction;
        Vector3 moveDirection = CalculateMoveDirection(inputDirection);
        MoveObject(moveDirection);
    }

    Vector3 CalculateMoveDirection(Vector2 inputDirection)
    {
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        return (cameraForward * inputDirection.y + cameraRight * inputDirection.x).normalized;
    }

    void MoveObject(Vector3 direction)
    {
        Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;

        if (!CheckCollision(newPosition))
        {
            transform.position = newPosition;
        }

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    bool CheckCollision(Vector3 targetPosition)
    {
        Vector3 moveDirection = targetPosition - transform.position;
        float distance = moveDirection.magnitude;

        Collider[] hitColliders = Physics.OverlapSphere(targetPosition, collisionCheckDistance);
        Collider door = Array.Find(hitColliders, (item) => item.tag == "Door");

        if (door != null)
        {
            door.GetComponent<Animator>().SetBool("Open", true);
        }

        if (Physics.Raycast(transform.position, moveDirection.normalized, out RaycastHit hit, distance + collisionCheckDistance))
        {
            return true;
        }

        return false;
    }
}