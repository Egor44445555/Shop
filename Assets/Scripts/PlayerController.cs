using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerPerson : MonoBehaviour 
{	
    public CharacterController controller;
    public Animator anim;
    public float speed;
    public float gravity;
    public Transform cameraTransform;

    Vector3 moveDirection;
    Joystick joystick;    

    void Start()
    {
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
    }

    void Update() {
	   Vector2 direction = joystick.direction;
	   
	   if (controller.isGrounded) 
       {
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            moveDirection = (cameraForward * direction.y + cameraRight * direction.x).normalized;
            moveDirection = moveDirection * speed;

            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }        
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}