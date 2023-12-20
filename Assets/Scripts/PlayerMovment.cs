using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerMovment : NetworkBehaviour
{
    private Vector3 _velocity;
    private bool _jumpPressed;

    private CharacterController _controller;

    public float PlayerSpeed = 2f;
    public Camera Camera;

    public float jumpForce = 5f;
    public float GravityValue = -9.81f;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _jumpPressed = true;
        }
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Camera = Camera.main;
            Camera.GetComponent<FirstPersonCamera>().Target = transform;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if(HasStateAuthority == false)
        {
            return;
        }

        var cameraRotation = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
        Vector3 move = cameraRotation * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * PlayerSpeed;

        _velocity.y += GravityValue * Runner.DeltaTime;
        
        if(_jumpPressed && _controller.isGrounded)
        {
            _velocity.y += jumpForce;
        }
        _controller.Move(move + _velocity * Runner.DeltaTime);

        if(move !=  Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if(_controller.isGrounded)
        {
            _velocity = new Vector3(0, -1, 0);
        }

        _jumpPressed = false;
    }
}
