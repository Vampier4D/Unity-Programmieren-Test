using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    public float walkingSpeed = 10f;
    public float runningSpeed = 15f;
    public float jumpSpeed = 8.0f;
    private float currentjumpSpeed;
    private float currentrunningSpeed;
    private float currentwalkingSpeed;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 1.0f;
    public float lookXLimit = 45.0f;


    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

      
       // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Check if the character is grounded
        bool isGrounded = characterController.isGrounded;

        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

       

            // Press Left Shift to run
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Jump logic
        if (Input.GetButtonDown("Jump") && canMove && isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity
        if (!isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    } 
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            switch (hit.gameObject.tag)
            {
                case "SpeedBoost":
                    walkingSpeed = 35f;
                    runningSpeed = 55f;
                    break;
                case "JumpPad":
                    jumpSpeed = 30f;
                    break;
                case "Ground":
                walkingSpeed = 10f;
                jumpSpeed = 8f;
                runningSpeed = 15f;
                    break;




            }
        }




    
}


